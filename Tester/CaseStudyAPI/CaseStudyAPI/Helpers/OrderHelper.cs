namespace CaseStudyAPI.Helpers
{
    public class OrderHelper
    {
        public string? Email { get; set; }
        public OrderSelectionHelper[]? Selections { get; set; }
        public decimal OrderAmount { get; set; }
        public int OrderId { get; set; }
        public string? ProductName { get; set; }
        public decimal SellingPrice { get; set; }
        public int QtySold { get; set; }
        public int QtyOrdered { get; set; }
        public int QtyBackOrdered { get; set; }
        public string? OrderDate { get; set; }

    }
}
