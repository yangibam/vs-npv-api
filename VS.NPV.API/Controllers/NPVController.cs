using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VS.NPV.API.Models.NPV;
using VS.NPV.API.Services.NPV;

namespace VS.NPV.API.Controllers
{
    [Route("api/NPV")]
    public class NPVController : Controller
    {
        private readonly INPVCalculatorService _npvCalculatorService;

        public NPVController(INPVCalculatorService npvCalculatorService)
        {
            _npvCalculatorService = npvCalculatorService;
        }

        public IActionResult Post([FromBody]NPVRequest request)
        {
            try
            {
                var result = _npvCalculatorService.Calculate(request);

                if(result != null)
                {
                    return Ok(result);
                }
            }
            catch
            {
                return BadRequest("Something went wrong.");
            }

            return BadRequest("Something went wrong.");
        }
    }
}