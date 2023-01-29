using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using StockBot.Controllers;
using StockBot.Services;
using StockBot.Services.Implementations;
using StockBot.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using Assert = NUnit.Framework.Assert;

namespace StockBotTests
{
    public class StockServiceTests
    {
        private StockService _service;
        private Mock<HttpClient> _httpClient;

        [SetUp]
        public void TestInitialize()
        {
            _httpClient = new Mock<HttpClient>();
            _service = new StockService(_httpClient.Object);
        }

        [Test]
        public void Call_to_get_stock_returns_stock_correctly()
        {
            string code = "AAPL.US";
            var result = _service.GetStock(code);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Call_to_get_stock_throws_exception()
        {
            string code = "some-unexistant-stock-code";

            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When($"https://stooq.com/q/l/?s={code}&f=sd2t2ohlcv&h&e=csv")
                    .Respond(HttpStatusCode.BadRequest);

            var client = new HttpClient(mockHttp);

            var service2 = new StockService(client);

            try
            {
                service2.GetStock(code);
            }
            catch (HttpResponseException e)
            {
                Assert.AreEqual(e.StatusCode, 502);
            }

        }
    }
}