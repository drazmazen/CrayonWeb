using AutoMapper;
using CrayonWeb.Api.Dto;
using CrayonWeb.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrayonWeb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly CrayonDbContext _dbContext;
        private readonly IMapper _mapper;

        public AccountsController(ILogger<AccountsController> logger, CrayonDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("{id}/purchases")]
        public ActionResult<IEnumerable<PurchaseDto>> GetPurchases(int id) 
        {
            try
            {
                var account = _dbContext.Accounts
                        .Include(a => a.Purchases)
                        .FirstOrDefault(a => a.Id == id);
                if (account == null)
                {
                    return NotFound();
                }
                var purchaseDtos = account.Purchases.Select(p => _mapper.Map<PurchaseDto>(p)).ToList();
                return Ok(purchaseDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
