using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VS.NPV.API.Models.NPV;
using VS.NPV.API.Services.NPV;

namespace VS.NPV.API.TEST.Services
{
    public class NPVCalculatorServiceTest
    {
        [TestCase(5, -537.41)]
        [TestCase(10, -570.25)]
        [TestCase(15, -599.24)]
        public void GivenNPVRequestAndParametersAreValidThenReturnResult(double rate, double expectedResult)
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
                LowerDiscount = rate,
                UpperDiscount = rate
            };

            var result = new NPVCalculatorService().Calculate(request);

            Assert.AreEqual(result.NPV.Count, 1);
            Assert.AreEqual(result.NPV[0].Amount, expectedResult);
            Assert.AreEqual(result.NPV[0].Rate, rate);
        }

        [Test]
        public void GivenNPVRequestAndCashFlowIsMissingThenThrowException()
        {
            var request = new NPVRequest {
                CashFlows = null,
                DiscountIncrement = 1,
                InitialInvestment = 1000,
                LowerDiscount = 1,
                UpperDiscount = 5
            };

            var exception = Assert.Throws<ArgumentException>(() => new NPVCalculatorService().Calculate(request));

            Assert.AreEqual(exception.Message, "Cashflow missing");
        }

        [TestCase(1, 101)]
        [TestCase(0, 5)]
        public void GivenNPVRequestAndDiscountRateIsInvalidThenThrowException(double lower, double upper)
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

            var exception = Assert.Throws<ArgumentException>(() => new NPVCalculatorService().Calculate(request));

            Assert.AreEqual(exception.Message, "Invalid discount rate");
        }

        [Test]
        public void GivenNPVRequestAndDiscountIncrementIsInvalidThenThrowException()
        {
            var request = new NPVRequest
            {
                CashFlows = new List<CashFlow>
                {
                    new CashFlow { Amount = 200 },
                    new CashFlow { Amount = 300 },
                },
                DiscountIncrement = 0,
                InitialInvestment = 1000,
                LowerDiscount = 1,
                UpperDiscount = 5
            };

            var exception = Assert.Throws<ArgumentException>(() => new NPVCalculatorService().Calculate(request));

            Assert.AreEqual(exception.Message, "Invalid discount increment");
        }
    }
}
