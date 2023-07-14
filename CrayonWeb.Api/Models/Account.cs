namespace CrayonWeb.Api.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
