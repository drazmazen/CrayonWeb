using CrayonWeb.Api.CCP.Dto;
using CrayonWeb.Api.Dto;

namespace CrayonWeb.Api.CCP
{
    public class CcpClientProd : ICcpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _cancelSoftwareEndpoint;
        private readonly string _changeQuantityEndpoint;
        private readonly string _extendLicenseEndpoint;
        private readonly string _getAvailableSoftwareEndpoint;
        private readonly string _orderEndpoint;


        public CcpClientProd(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new();
            _httpClient.BaseAddress = new Uri(configuration["ccpBaseAddress"]);
            _cancelSoftwareEndpoint = configuration["cancelSoftwareEndpoint"];
            _changeQuantityEndpoint = configuration["changeQuantityEndpoint"];
            _extendLicenseEndpoint = configuration["extendLicenseEndpoint"];
            _getAvailableSoftwareEndpoint = configuration["getAvailableSoftwareEndpoint"];
            _orderEndpoint = configuration["orderEndpoint"];
        }


        public async Task<bool> CancelSoftware(string orderReference)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ChangeQuantity(string orderReference, int newQuantity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExtendLicense(string orderReference, DateTime validTo)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Software>> GetAvailableSoftware()
        {
            throw new NotImplementedException();
        }

        public async Task<OrderResultDto> Order(string Id, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
