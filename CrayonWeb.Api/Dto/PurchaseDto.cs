namespace CrayonWeb.Api.Dto
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidToDate { get; set; }
    }
}
