using System.ComponentModel.DataAnnotations;

namespace CrayonWeb.Api.Dto
{
    public class OrderInputDto
    {
        [Required]
        public int AccountId { get; set; }
        [Required]
        public string SoftwareId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
