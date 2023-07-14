using CrayonWeb.Api.CCP.Dto;
using CrayonWeb.Api.Dto;

namespace CrayonWeb.Api.CCP
{
    public class CcpClientDev : ICcpClient
    {
        private readonly List<Software> _softwareList = new List<Software>
            {
                new Software {Id = "mswindowsid", Name = "Microsoft Windows 11 Pro"},
                new Software {Id = "msofficeid", Name = "Microsoft Office"},
                new Software {Id = "pizzaid", Name = "Pizza"},
                new Software {Id = "justiceid", Name = "Justice"}
            };
        public async Task<bool> CancelSoftware(string orderReference)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<bool> ChangeQuantity(string orderReference, int newQuantity)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<bool> ExtendLicense(string orderReference, DateTime validTo)
        {
            await Task.Run(() => { });
            return true;
        }

        public async Task<IEnumerable<Software>> GetAvailableSoftware()
        {
            await Task.Run(() => { });
            return _softwareList;
        }

        public async Task<OrderResultDto> Order(string Id, int quantity)
        {
            await Task.Run(() => { });
            var software = _softwareList.Where(p => p.Id == Id).FirstOrDefault();
            if (software == null)
            {
                return new OrderResultDto 
                { 
                    OrderReference = string.Empty, 
                    Name = string.Empty, 
                    ValidTo = DateTime.MinValue,
                    Success = false, 
                    ErrorMessage = "Unknown software id" 
                };
            }

            return new OrderResultDto 
            { 
                OrderReference = Guid.NewGuid().ToString(), 
                Name = software.Name, 
                Success = true,
                ValidTo = DateTime.Now.AddYears(1),
                ErrorMessage = string.Empty
            };
        }

        public OrderResultDto OrderSync(string Id, int quantity)
        {
            var software = _softwareList.Where(p => p.Id == Id).FirstOrDefault();
            if (software == null)
            {
                return new OrderResultDto
                {
                    OrderReference = string.Empty,
                    Name = string.Empty,
                    ValidTo = DateTime.MinValue,
                    Success = false,
                    ErrorMessage = "Unknown software id"
                };
            }

            return new OrderResultDto
            {
                OrderReference = Guid.NewGuid().ToString(),
                Name = software.Name,
                Success = true,
                ValidTo = DateTime.Now.AddYears(1),
                ErrorMessage = string.Empty
            };
        }
    }
}
