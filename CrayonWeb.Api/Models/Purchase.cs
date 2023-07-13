namespace CrayonWeb.Api.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidToDate { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
