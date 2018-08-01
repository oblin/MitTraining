using JagiCore;
using JagiCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace MitTraining.Controllers
{
    [Route("api/[controller]")]
    public class CodesController : ControllerBase
    {
        private readonly CodeService _codeService;

        public CodesController(CodeService codeService)
        {
            _codeService = codeService;
        }

        [HttpGet]
        public IActionResult Get(string code, string parentCode)
        {
            return _codeService.GetCodeDetails(code, parentCode)
                .OnBoth(result => result.IsSuccess ? Ok(result.Value) : (IActionResult)NotFound());
        }
    }
}
