using AutoMapper;
using CrayonWeb.Api.Models;

namespace CrayonWeb.Api.Dto
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountDto>();
        }
    }
}
