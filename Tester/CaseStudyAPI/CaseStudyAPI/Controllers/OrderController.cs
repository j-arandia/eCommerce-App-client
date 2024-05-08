using CaseStudyAPI.DAL;
using CaseStudyAPI.DAL.DAO;
using CaseStudyAPI.DAL.DomainClasses;
using CaseStudyAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace CaseStudyAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        readonly AppDbContext _ctx;
        public OrderController(AppDbContext context) // injected here
        {
            _ctx = context;
        }
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<string>> Index(OrderHelper helper)
        {
            string retVal;
            try
            {
                CustomerDAO cDao = new(_ctx);
                Customer? basketOwner = await cDao.GetByEmail(helper.Email);
                OrderDAO oDao = new(_ctx);
                int orderId = await oDao.AddOrder(basketOwner!.Id, helper.Selections!);
                retVal = orderId > 0
                ? "Order " + orderId + " saved!"
                : "Order not saved";

            }
            catch (Exception ex)
            {
                retVal = "Order not saved " + ex.Message;
            }
            return retVal;
        }
        [Route("{email}")]
        [HttpGet]
        //[AllowAnonymous]
        public async Task<ActionResult<List<Order>>> List(string email)
        {
            List<Order> orders; ;
            CustomerDAO cDao = new(_ctx);
            Customer? orderOwner = await cDao.GetByEmail(email);
            OrderDAO oDao = new(_ctx);
            orders = await oDao.GetAll(orderOwner!.Id);
            return orders;
        }
        [Route("{orderid}/{email}")]
        [HttpGet]
        //[AllowAnonymous]
        public async Task<ActionResult<List<OrderHelper>>> GetOrderDetails(int orderid, string email)
        {
            OrderDAO dao = new(_ctx);
            return await dao.GetOrderDetails(orderid, email);
        }
    }
}
