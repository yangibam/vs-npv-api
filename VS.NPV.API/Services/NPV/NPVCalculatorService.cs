using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.NPV.API.Models.NPV;

namespace VS.NPV.API.Services.NPV
{
    public class NPVCalculatorService : INPVCalculatorService
    {
        public NPVResponse Calculate(NPVRequest request)
        {
            ValidateRequest(request);

            var rates = GetRates(request.LowerDiscount, request.UpperDiscount, request.DiscountIncrement);

            var npvs = new List<Models.NPV.NPV>();
            
            foreach(var rate in rates)
            {
                double npv = 0;
                int period = 1;
                foreach(var cashflow in request.CashFlows)
                {
                    npv += cashflow.Amount / Math.Pow((1 + (rate / 100d)), period);

                    period++;
                }

                npvs.Add(new Models.NPV.NPV { Amount = npv - request.InitialInvestment, Rate = rate});
            }

            return new NPVResponse { NPV = npvs };
        }

        private List<double> GetRates(double lower, double upper, double increment)
        {
            List<double> rates = new List<double>();

            for(double i = lower; i <= upper; i += increment)
            {
                rates.Add(i);
            }

            return rates;
        }

        private void ValidateRequest(NPVRequest request)
        {
            if (request?.CashFlows == null || request.CashFlows.Count == 0)
            {
                throw new ArgumentException("Cashflow missing");
            }

            if (request?.LowerDiscount < 1 || request?.UpperDiscount > 100 || request?.LowerDiscount > request?.UpperDiscount)
            {
                throw new ArgumentException("Invalid discount rate");
            }

            if(request?.DiscountIncrement < 1 || request?.DiscountIncrement > 100)
            {
                throw new ArgumentException("Invalid discount increment");
            }
        }
    }
}
