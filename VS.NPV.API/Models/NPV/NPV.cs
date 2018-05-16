using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VS.NPV.API.Models.NPV
{
    public class NPV
    {
        private double _amount;

        public double Rate { get; set; }
        public double Amount
        {
            get { return _amount; }
            set { _amount = Math.Round(value, 2, MidpointRounding.AwayFromZero); }
        }        
    }
}
