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
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly CrayonDbContext _dbContext;
        private readonly IMapper _mapper;
        public CustomersController(ILogger<CustomersController> logger, CrayonDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{id}/accounts")]
        public ActionResult<IEnumerable<AccountDto>> GetAccounts(int id)
        {
            try
            {
                var customer = _dbContext.Customers
                        .Include(c => c.Accounts)
                        .FirstOrDefault(c => c.Id == id);
                if (customer == null)
                {
                    return NotFound();
                }
                var accountDtos = customer.Accounts?.Select(a => _mapper.Map<AccountDto>(a)).ToList();
                return Ok(accountDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
