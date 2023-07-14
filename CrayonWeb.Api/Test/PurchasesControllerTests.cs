using CrayonWeb.Api.CCP;
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
    public class PurchasesControllerTests
    {
        private OrderInputDto _orderInputDto;
        private Mock<ILogger<PurchasesController>> _mockLogger;
        private Mock<CrayonDbContext> _mockDbContext;
        private ICcpClient _mockCcpClient;
        private PurchasesController _ctrl;
        

        [TestInitialize]
        public void Setup() 
        { 
            _orderInputDto = new OrderInputDto
            {
                AccountId = 1,
                SoftwareId = "mswindowsid",
                Quantity = 1
            };

            _mockLogger = new Mock<ILogger<PurchasesController>>();
            _mockDbContext = new Mock<CrayonDbContext>();
            _mockCcpClient = new CcpClientDev();
            var purchase = new Purchase
            {
                Id = 1,
                CcpReference = "testReference",
                Name = "testName",
                IsActive = true,
                Quantity = 1,
                ValidToDate = DateTime.Now.AddYears(1)
            };
            var purchases = new List<Purchase> { purchase };
            var purchasesData = purchases.AsQueryable();
            var account = new Account
            {
                Id = 1,
                Name = "testAccount",
                Purchases = purchases
            };
            var data = new List<Account> { account }.AsQueryable();
            var mockDbSetAccount = new Mock<DbSet<Account>>();            
            mockDbSetAccount.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSetAccount.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSetAccount.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSetAccount.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockDbSetPurchases = new Mock<DbSet<Purchase>>();
            mockDbSetPurchases.As<IQueryable<Purchase>>().Setup(m => m.Provider).Returns(purchasesData.Provider);
            mockDbSetPurchases.As<IQueryable<Purchase>>().Setup(m => m.Expression).Returns(purchasesData.Expression);
            mockDbSetPurchases.As<IQueryable<Purchase>>().Setup(m => m.ElementType).Returns(purchasesData.ElementType);
            mockDbSetPurchases.As<IQueryable<Purchase>>().Setup(m => m.GetEnumerator()).Returns(purchasesData.GetEnumerator());



            _mockDbContext.Setup(c => c.Accounts).Returns(mockDbSetAccount.Object);
            _mockDbContext.Setup(c => c.Purchases).Returns(mockDbSetPurchases.Object);
            _mockDbContext.Setup(c => c.Accounts.Find(It.Is<int>(i => i == 1))).Returns(account);
            _mockDbContext.Setup(c => c.Purchases.Find(It.Is<int>(i => i == 1))).Returns(purchase);
            _ctrl = new PurchasesController(_mockLogger.Object, _mockDbContext.Object, _mockCcpClient);
        }

        [TestMethod]
        public void Order_BadRequestQuantity()
        {
            _orderInputDto.Quantity = 0;
            var response = _ctrl.Order(_orderInputDto);
            var invalidQuantityResult = response.Result.Result as BadRequestObjectResult;
            Assert.AreEqual(invalidQuantityResult.StatusCode, 400);

        }
        [TestMethod]
        public void Order_Ok()
        {
            var response = _ctrl.Order(_orderInputDto);
            var okResult = response.Result.Result as OkObjectResult;
            Assert.AreEqual(okResult.StatusCode, 200);
        }
        [TestMethod]
        public void Order_NotFound()
        {
            _orderInputDto.AccountId = 99;
            var response = _ctrl.Order(_orderInputDto);
            var notFoundResult = response.Result.Result as NotFoundObjectResult;
            Assert.AreEqual(notFoundResult.StatusCode, 404);
        }


        [TestMethod]
        public void Cancel_Ok()
        {
            var response = _ctrl.Cancel(1);
            var okResult = response.Result as OkResult;
            Assert.AreEqual(okResult.StatusCode, 200);
        }

        [TestMethod]
        public void Cancel_NotFound()
        {
            var response = _ctrl.Cancel(99);
            var result = response.Result as NotFoundObjectResult;
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void ChangeQuantity_Ok()
        {
            var response = _ctrl.ChangeQuantity(1, 5);
            var result = response.Result as OkResult;
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void ChangeQuantity_NotFound()
        {
            var response = _ctrl.ChangeQuantity(99, 5);
            var result = response.Result as NotFoundObjectResult;
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void ChangeQuantity_BadRequest()
        {
            var response = _ctrl.ChangeQuantity(1, 0);
            var result = response.Result as BadRequestObjectResult;
            Assert.AreEqual(result.StatusCode, 400);
        }

        [TestMethod]
        public void ExtendLicense_Ok()
        {
            var response = _ctrl.ExtendLicense(1, DateTime.Now.AddYears(2));
            var result = response.Result as OkResult;
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void ExtendLicense_NotFound()
        {
            var response = _ctrl.ExtendLicense(99, DateTime.Now.AddYears(2));
            var result = response.Result as NotFoundObjectResult;
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void ExtendLicense_BadRequest()
        {
            var response = _ctrl.ExtendLicense(1, DateTime.Now.AddYears(-1));
            var result = response.Result as BadRequestObjectResult;
            Assert.AreEqual(result.StatusCode, 400);
        }
    }
}
