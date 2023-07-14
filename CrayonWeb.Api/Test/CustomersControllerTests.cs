using AutoMapper;
using CrayonWeb.Api.Controllers;
using CrayonWeb.Api.Dto;
using CrayonWeb.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CrayonWeb.Api.Test
{
    [TestClass]
    public class CustomersControllerTests
    {
        [TestMethod]
        public void GetAccountsTest()
        {
            var customer1 = new Customer
            {
                Id = 1,
                Name = "TestCustomer1",
                Accounts = new List<Account>
                {
                    new Account
                    {
                        Id=1,
                        Name = "TestAccount",
                    }
                }
            };

            var customer2 = new Customer
            {
                Id = 2,
                Name = "TestCustomer2"
            };

            var data = new List<Customer> { customer1, customer2 }.AsQueryable();
            var mockDbSetCustomer = new Mock<DbSet<Customer>>();

            var mockDbContext = new Mock<CrayonDbContext>();

            mockDbSetCustomer.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSetCustomer.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSetCustomer.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSetCustomer.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockDbContext.Setup(c => c.Customers).Returns(mockDbSetCustomer.Object);
            var mockLogger = new Mock<ILogger<CustomersController>>();
            var autoMapperConfig = new MapperConfiguration(options =>
            {
                options.AddProfile<AutoMapperProfile>();
            });
            var mapper = autoMapperConfig.CreateMapper();
            var customersController = new CustomersController(mockLogger.Object, mockDbContext.Object, mapper);

            var result1 = customersController.GetAccounts(1);
            var okResult = result1.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult?.StatusCode, 200);

            var result2 = customersController.GetAccounts(2);
            var noContentResult = result2.Result as OkObjectResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(noContentResult?.StatusCode, 200);

            var result3 = customersController.GetAccounts(3);
            var notFoundResult = result3.Result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(notFoundResult.StatusCode, 404);
        }
    }
}
