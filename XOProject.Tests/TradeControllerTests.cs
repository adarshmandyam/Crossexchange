using System;
using System.Threading.Tasks;
using XOProject.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;

namespace XOProject.Tests
{
    public class TradeControllerTests
    {
        public readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();
        private readonly Mock<IPortfolioRepository> _portRepositoryMock = new Mock<IPortfolioRepository>();
        private readonly Mock<ITradeRepository> _tradeRepositoryMock = new Mock<ITradeRepository>();

        private readonly TradeController _tradeController;

        public TradeControllerTests()
        {
            _tradeController = new TradeController(_shareRepositoryMock.Object, _tradeRepositoryMock.Object, _portRepositoryMock.Object);
        }

        [Test]
        public async Task Get_GetTradeMinMaxValues()
        {
            // Act
            var result = await _tradeController.GetAnalysis("REL");
            var okresult = result as OkObjectResult;

            Assert.NotNull(okresult);
            Assert.AreEqual(200, okresult.StatusCode);

            var result1 = await _tradeController.GetAnalysis("CBI");
            var okresult1 = result1 as OkObjectResult;

            Assert.NotNull(okresult1);
            Assert.AreEqual(200, okresult1.StatusCode);
        }

    }
}
