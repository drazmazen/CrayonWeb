using CrayonWeb.Api.CCP.Dto;
using CrayonWeb.Api.Dto;
using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;

namespace CrayonWeb.Api.CCP
{
    public class CcpClientProd : ICcpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _baseAddress;
        private readonly string _cancelSoftwareEndpoint;
        private readonly string _changeQuantityEndpoint;
        private readonly string _extendLicenseEndpoint;
        private readonly string _getAvailableSoftwareEndpoint;
        private readonly string _orderEndpoint;


        public CcpClientProd(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;            
            _baseAddress = configuration["Ccp:BaseAddress"];
            _cancelSoftwareEndpoint = configuration["Ccp:CancelSoftwareEndpoint"];
            _changeQuantityEndpoint = configuration["Ccp:ChangeQuantityEndpoint"];
            _extendLicenseEndpoint = configuration["Ccp:ExtendLicenseEndpoint"];
            _getAvailableSoftwareEndpoint = configuration["Ccp:GetAvailableSoftwareEndpoint"];
            _orderEndpoint = configuration["Ccp:OrderEndpoint"];
        }


        public async Task<bool> CancelSoftware(string orderReference)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_baseAddress}{_cancelSoftwareEndpoint}");
            httpRequestMessage.Content = JsonContent.Create(orderReference);
            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                //process the message
                return true;
            }
            return false;
        }

        public async Task<bool> ChangeQuantity(string orderReference, int newQuantity)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_baseAddress}{_changeQuantityEndpoint}");
            httpRequestMessage.Content = JsonContent.Create(new { OrderReference = orderReference, Quantity = newQuantity });
            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                //process the message
                return true;
            }
            return false;
            
        }

        public async Task<bool> ExtendLicense(string orderReference, DateTime validTo)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_baseAddress}{_extendLicenseEndpoint}");
            httpRequestMessage.Content = JsonContent.Create(new { OrderReference = orderReference, ValidTo = validTo.ToString("yyyy-MM-dd") });
            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                //process the message
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Software>> GetAvailableSoftware()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_baseAddress}{_getAvailableSoftwareEndpoint}");
            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                //process the message
                return new List<Software>();
            }
            return null;
        }

        public async Task<OrderResultDto> Order(string id, int quantity)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_baseAddress}{_orderEndpoint}");
            httpRequestMessage.Content = JsonContent.Create(new { Id = id, Quantity = quantity });
            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                //process the message
                return new OrderResultDto();
            }
            return null;
        }
    }
}
