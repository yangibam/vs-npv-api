using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.NPV.API.Models.NPV;

namespace VS.NPV.API.Services.NPV
{
    public class NPVCalculatorService : INPVCalculatorService
    {
        public List<Models.NPV.NPV> Calculate(NPVRequest request)
        {
            if(request?.CashFlows == null || request.CashFlows.Count == 0)
            {
                throw new Exception("Cashflow missing");
            }

            var rates = GetRates(request.LowerDiscount, request.UpperDiscount, request.DiscountIncrement);

            var npvs = new List<Models.NPV.NPV>();
            
            foreach(var rate in rates)
            {
                double npv = 0;
                int period = 1;
                foreach(var cashflow in request.CashFlows)
                {
                    npv += cashflow.Amount / Math.Pow((1 + rate), period);

                    period++;
                }

                npvs.Add(new Models.NPV.NPV { Amount = npv - request.InitialInvestment, Rate = Math.Round(rate, 2)});
            }

            return npvs;
        }

        private List<double> GetRates(double lower, double upper, double increment)
        {
            List<double> rates = new List<double>();

            for(double i = lower; i <= upper; i += increment)
            {
                var rate = (i / 100d);

                rates.Add(rate);
            }

            return rates;
        }
    }
}
