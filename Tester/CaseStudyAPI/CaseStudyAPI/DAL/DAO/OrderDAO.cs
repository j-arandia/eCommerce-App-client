using CaseStudyAPI.DAL.DomainClasses;
using CaseStudyAPI.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CaseStudyAPI.DAL.DAO
{
    public class OrderDAO
    {
        private readonly AppDbContext _db;
        public OrderDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public async Task<int> AddOrder(int customerid, OrderSelectionHelper[] selections)
        {
            int orderId = -1;
            // we need a transaction as multiple entities involved
            using (var _trans = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    Order order = new();
                    order.CustomerId = customerid;
                    order.OrderDate = System.DateTime.Now;
                    order.OrderAmount = 0;

                    // calculate the totals and then add the order row to the table
                    foreach (OrderSelectionHelper selection in selections)
                    {
                        
                        order.OrderAmount += Convert.ToDecimal(selection.Item!.MSRP * selection.Qty);
                    }
                    await _db.Orders!.AddAsync(order);
                    await _db.SaveChangesAsync();

                    // then add each item to the orderlineitems table
                    foreach (OrderSelectionHelper selection in selections)
                    {
                        OrderLineItem oItem = new();                    
                        oItem.Product = _db.Product!.FirstOrDefault(p => p.Id == selection.Item!.Id);
                        oItem.OrderId = order.Id;


                        if (selection.Qty > oItem.Product!.QtyOnHand)  // not enough stock
                        {
                            oItem.QtySold = oItem.Product!.QtyOnHand;
                            oItem.Product.QtyOnHand = 0;
                            oItem.Product.QtyOnBackOrder = oItem.Product.QtyOnBackOrder +
                                    (selection.Qty - oItem.Product.QtyOnHand);
                            oItem.QtyOrdered = selection.Qty;
                            oItem.QtyBackOrdered = selection.Qty - oItem.QtySold;
                            oItem.SellingPrice = oItem.Product.MSRP;

                        }
                        else // enough stock
                        {
                            oItem.Product.Id = selection.Item!.Id;
                            oItem.Product!.QtyOnHand = selection.Qty - oItem.Product.QtyOnHand;
                            oItem.QtySold = selection.Qty;
                            oItem.QtyOrdered = selection.Qty;
                            oItem.QtyBackOrdered = selection.Qty - oItem.Product.QtyOnHand;
                            oItem.SellingPrice = oItem.Product.MSRP;
                        }

                        order.OrderLineItems.Add(oItem);                            
                            await _db.SaveChangesAsync();
                        
                    }
                    // test trans by uncommenting out these 3 lines
                    //int x = 1;
                    //int y = 0;
                    //x = x / y;
                    await _trans.CommitAsync();
                    orderId = order.Id;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await _trans.RollbackAsync();
                }
            }
            return orderId;
        }
        public async Task<List<Order>> GetAll(int id)
        {
            return await _db.Orders!.Where(order => order.CustomerId == id).ToListAsync<Order>();
        }

        public async Task<List<OrderHelper>> GetOrderDetails(int oid, string email)
        {

            Customer? cust = _db.Customer!.FirstOrDefault(cust => cust.Email == email);
            List<OrderHelper> allDetails = new();

            // LINQ way of doing INNER JOINS
            var results = from order in _db.Orders
                          join item in _db.LineItem! on order.Id equals item.OrderId
                          join product in _db.Product! on item.ProductId equals product.Id
                          where (order.CustomerId == cust!.Id && order.Id == oid)
                          select new OrderHelper
                          {
                              OrderAmount = order.OrderAmount,
                              OrderId = order.Id,
                              ProductName = item.Product!.ProductName,
                              SellingPrice = item.SellingPrice,
                              QtySold = item.QtySold,
                              QtyBackOrdered = item.QtyBackOrdered,
                              QtyOrdered = item.QtyOrdered,
                              OrderDate = order.OrderDate.ToString("yyyy/MM/dd - hh:mm tt")
                          };
            allDetails = await results.ToListAsync();
            return allDetails;
        }
    }
}