using CrayonWeb.Api.CCP;
using CrayonWeb.Api.CCP.Dto;
using CrayonWeb.Api.Dto;
using CrayonWeb.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrayonWeb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly ILogger<PurchasesController> _logger;
        private readonly CrayonDbContext _dbContext;
        private readonly ICcpClient _ccpClient;

        public PurchasesController(ILogger<PurchasesController> logger, CrayonDbContext dbContext, ICcpClient ccpClient)
        {
            _logger = logger;
            _dbContext = dbContext;
            _ccpClient = ccpClient;
        }

        [Route("available")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Software>>> GetAvailableSoftware()
        {
            try
            {
                var softwareList = await _ccpClient.GetAvailableSoftware();
                return Ok(softwareList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [Route("order")]
        [HttpPost]
        public async Task<ActionResult<int>> Order([FromBody] OrderInputDto inputDto)
        {
            try
            {
                if (inputDto.Quantity < 1)
                {
                    return BadRequest("Order quantity has to be at least 1");
                }
                var account = _dbContext.Accounts.Find(inputDto.AccountId);
                if (account == null)
                {
                    return NotFound($"Account not found for id {inputDto.AccountId}");
                }
                var result = await _ccpClient.Order(inputDto.SoftwareId, inputDto.Quantity);
                if (result.Success)
                {
                    var newPurchase = new Purchase
                    {
                        AccountId = account.Id,
                        CcpReference = result.OrderReference,
                        IsActive = true,
                        ValidToDate = result.ValidTo,
                        Name = result.Name
                    };
                    _dbContext.Purchases.Add(newPurchase);
                    _dbContext.SaveChanges();
                    return Ok(newPurchase.Id);
                }
                else
                {
                    return BadRequest($"CCP denied order with message: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [Route("{id}/cancel")]
        [HttpPost]
        public async Task<ActionResult> Cancel(int id)
        {
            try
            {
                var purchase = _dbContext.Purchases.Find(id);
                if (purchase == null)
                {
                    return NotFound($"Purchase not found for id {id}");
                }
                var success = await _ccpClient.CancelSoftware(purchase.CcpReference);
                if (success)
                {
                    purchase.IsActive = false;
                    _dbContext.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest("Cancelation denied by CCP");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [Route("{id}/quantity")]
        [HttpPost]
        public async Task<ActionResult> ChangeQuantity(int id, [FromBody]int newQuantity)
        {
            try
            {
                if (newQuantity < 1)
                {
                    return BadRequest("Order quantity has to be at least 1");
                }
                var purchase = _dbContext.Purchases.FirstOrDefault(p => p.Id == id && p.IsActive);
                if (purchase == null)
                {
                    return NotFound($"Purchase not found for id {id}");
                }
                var success = await _ccpClient.ChangeQuantity(purchase.CcpReference, newQuantity);
                if (success)
                {
                    purchase.Quantity = newQuantity;
                    if (newQuantity <= 0)
                    {
                        purchase.IsActive = false;
                    }
                    _dbContext.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest("Quantity change denied by CCP");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [Route("{id}/extend")]
        [HttpPost]
        public async Task<ActionResult> ExtendLicense(int id, [FromBody]DateTime validTo)
        {
            try
            {
                var purchase = _dbContext.Purchases.FirstOrDefault(p => p.Id == id && p.IsActive);
                if (purchase == null)
                {
                    return NotFound($"Purchase not found for id {id}");
                }
                if (validTo <= purchase.ValidToDate)
                {
                    return BadRequest("Extend date cannot be earlier than the current validToDate.");
                }
                var success = await _ccpClient.ExtendLicense(purchase.CcpReference, validTo);
                if (success)
                {
                    purchase.ValidToDate = validTo;
                    _dbContext.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest("License extend denied by CCP.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
