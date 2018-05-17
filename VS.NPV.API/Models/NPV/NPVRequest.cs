using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VS.NPV.API.Models.NPV
{
    public class NPVRequest
    {
        public double LowerDiscount { get; set; }
        public double UpperDiscount { get; set; }
        public double DiscountIncrement { get; set; }
        public double InitialInvestment { get; set; }
        public List<CashFlow> CashFlows { get; set; }        
    }
}
