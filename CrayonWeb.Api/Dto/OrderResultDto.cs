namespace CrayonWeb.Api.Dto
{
    public class OrderResultDto
    {
        public string OrderReference { get; set; }
        public string Name { get; set; }
        public DateTime ValidTo { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
