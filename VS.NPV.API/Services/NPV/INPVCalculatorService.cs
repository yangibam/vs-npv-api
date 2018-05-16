using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VS.NPV.API.Models.NPV;

namespace VS.NPV.API.Services.NPV
{
    public interface INPVCalculatorService
    {
        List<Models.NPV.NPV> Calculate(NPVRequest request);
    }
}
