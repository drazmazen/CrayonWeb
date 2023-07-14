using AutoMapper;
using CrayonWeb.Api.Controllers;
using CrayonWeb.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CrayonWeb.Api.Test
{
    [TestClass]
    public class AccountsControllerTests
    {
        [TestMethod]
        public void GetPurchasesTest()
        {
            var account = new Account
            {
                Id = 1,
                Name = "test",
                Purchases = new List<Purchase>
                {
                    new Purchase
                    {
                        CcpReference = "testReference", 
                        Name = "testName", 
                        IsActive = true, 
                        Quantity = 1, 
                        ValidToDate = DateTime.Now.AddYears(1)
                    }
                }
            };
            var data = new List<Account> { account }.AsQueryable();
            var mockDbSetAccount = new Mock<DbSet<Account>>();
            var mockDbContext = new Mock<CrayonDbContext>();

            mockDbSetAccount.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSetAccount.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSetAccount.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSetAccount.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockDbContext.Setup(c => c.Accounts).Returns(mockDbSetAccount.Object);
            var mockLogger = new Mock<ILogger<AccountsController>>();
            var mockAutoMapper = new Mock<IMapper>();

            var accountsController = new AccountsController(mockLogger.Object, mockDbContext.Object, mockAutoMapper.Object);
            var result1 = accountsController.GetPurchases(1);
            var okResult = result1.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult?.StatusCode, 200);

            var result2 = accountsController.GetPurchases(99);
            var notFoundResult = result2.Result as NotFoundResult;
            Assert.IsNotNull(result2.Result);
            Assert.AreEqual(notFoundResult?.StatusCode, 404);
        }
    }
}
