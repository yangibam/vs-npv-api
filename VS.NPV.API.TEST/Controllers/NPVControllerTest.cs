using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VS.NPV.API.Controllers;
using VS.NPV.API.Models.NPV;
using VS.NPV.API.Services.NPV;

namespace VS.NPV.API.TEST.Controllers
{
    public class NPVControllerTest
    {
        private INPVCalculatorService _npvService;
        private NPVController _npvController;

        [SetUp]
        public void Setup()
        {
            _npvService = A.Fake<INPVCalculatorService>();
            _npvController = new NPVController(_npvService);
        }

        [Test]
        public void GivenNPVRequestAndParametersAreValidReturnResult()
        {
            var request = new NPVRequest
            {
                CashFlows = new List<CashFlow>
                {
                    new CashFlow { Amount = 200 },
                    new CashFlow { Amount = 300 },
                },
                DiscountIncrement = 1,
                InitialInvestment = 1000,
                LowerDiscount = 1,
                UpperDiscount = 5
            };

            A.CallTo(() => _npvService.Calculate(A<NPVRequest>.Ignored)).Returns(new NPVResponse());

            var result = _npvController.Calculate(request);
            var content = result as OkObjectResult;
            var response = content.Value as NPVResponse;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);   
            Assert.IsNotNull(response);
        }

        [Test]
        public void GivenNPVRequestAndCashFlowIsMissingReturnBadRequest()
        {
            var request = new NPVRequest
            {
                CashFlows = new List<CashFlow>(),
                DiscountIncrement = 1,
                InitialInvestment = 1000,
                LowerDiscount = 1,
                UpperDiscount = 5
            };

            A.CallTo(() => _npvService.Calculate(A<NPVRequest>.Ignored)).Throws(new ArgumentException("Cashflow missing"));

            var result = _npvController.Calculate(request);
            var content = result as BadRequestObjectResult;
            var response = content.Value as string;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.IsNotNull(response);
            Assert.AreEqual(response, "Cashflow missing");
        }

        [TestCase(0,5)]
        [TestCase(1,200)]
        [TestCase(0,105)]
        public void GivenNPVRequestAndDiscountRateIsInvalidReturnBadRequest(double lower, double upper)
        {
            var request = new NPVRequest
            {
                CashFlows = new List<CashFlow>
                {
                    new CashFlow { Amount = 200 },
                    new CashFlow { Amount = 300 },
                },
                DiscountIncrement = 1,
                InitialInvestment = 1000,
                LowerDiscount = lower,
                UpperDiscount = upper
            };

            A.CallTo(() => _npvService.Calculate(A<NPVRequest>.Ignored)).Throws(new ArgumentException("Invalid discount rate"));

            var result = _npvController.Calculate(request);
            var content = result as BadRequestObjectResult;
            var response = content.Value as string;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.IsNotNull(response);
            Assert.AreEqual(response, "Invalid discount rate");
        }
    }
}
