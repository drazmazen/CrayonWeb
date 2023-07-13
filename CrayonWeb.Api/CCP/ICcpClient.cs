using CrayonWeb.Api.CCP.Dto;
using CrayonWeb.Api.Dto;

namespace CrayonWeb.Api.CCP
{
    public interface ICcpClient
    {
        Task<IEnumerable<Software>> GetAvailableSoftware();
        Task<OrderResultDto> Order(string Id, int quantity);
        Task<bool> ChangeQuantity(string orderReference, int newQuantity);
        Task<bool> CancelSoftware(string orderReference);
        Task<bool> ExtendLicense(string orderReference, DateTime validTo);

    }
}
