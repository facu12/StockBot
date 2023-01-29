using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using StockBot.Controllers;
using StockBot.Services.Interfaces;
using System;

namespace StockBotTests
{
    public class StockControllerTests
    {
        private Mock<IStockService> _service;
        private StockController _controller;

        [SetUp]
        public void TestInitialize()
        {
            _service = new Mock<IStockService>();
            _controller = new StockController(_service.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                    }
                }
            };
        }

        [Test]
        public void Call_to_get_stock_returns_stock_correctly()
        {
            string code = "AAPL.US";

            var result = _controller.GetStock(code);

            _service.Verify(x => x.GetStock(code), Times.Once);

            NUnit.Framework.Assert.NotNull(result);
        }

        [Test]
        public void Call_to_get_stock_returns_null()
        {
            string code = "some-unexistant-stock-code";

            var result = _controller.GetStock(code);

            _service.Verify(x => x.GetStock(code), Times.Once);
            
            NUnit.Framework.Assert.Null(result.Value);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void Call_to_get_stock_throws_exception()
        {
            string code = "some-code";

            _service.Setup(x => x.GetStock(code)).Throws<ArgumentException>();
            _controller.GetStock(code);

            _service.Verify(x => x.GetStock(code), Times.Once);
        }
    }
}