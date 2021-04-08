using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VitoshaBank.Data.MessageModels;
using VitoshaBank.Services.CalculatorService.Interfaces;
using VitoshaBank.Data.RequestModels;

namespace VitoshaBank.Controllers
{
    [ApiController]
    [Route("api/calculator")]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculatorService _calculatorService;

        public CalculatorController(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        [HttpGet("test")]
        [AllowAnonymous]
        public async Task<ActionResult<MessageModel>> Test(CalculatorRequestModel request)
        {
            var cur1 = request.Curr1;
            var cur2 = request.Curr2;

            return await _calculatorService.Calculate(cur1, cur2);
        }

    }
}
