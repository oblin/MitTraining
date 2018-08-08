using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JagiCore;
using JagiCore.Admin;
using JagiCore.Services;
using Lhc.Data;
using Lhc.Data.Data;
using Microsoft.AspNetCore.Mvc;

namespace MitTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegsController : ControllerBase
    {
        private readonly CodeService _codeService;
        private readonly IUserResolverService _user;
        private readonly LhcService _lhcService;

        public RegsController(IUserResolverService user, CodeService codeService, LhcService lhcService)
        {
            _codeService = codeService;
            _user = user;
            _lhcService = lhcService;
        }

        // GET api/byContext
        [HttpGet("InPatient")]
        public ActionResult<List<RegFile>> GetInPatient()
        {
            return _lhcService.GetInHospitalPatients();
        }

        [HttpGet("GetPaged")]
        public ActionResult GetPaged(int pageNumber, int pageSize)
        {
            var list = _lhcService.GetPaged(pageNumber, pageSize, out int count);
            return Ok(new { list = list, pageCount = count });
        }

        // GET api/values/5
        [HttpGet("patient/{id}")]
        public ActionResult<RegFile> GetPatient(string id)
        {
            return _lhcService.GetPatient(id).Value;
        }

        [HttpPost("AddPatient")]
        public ActionResult<RegFile> AddPatient([FromBody] RegFile model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            return _lhcService.AddPatient(model);
        }

        [HttpPut("UpdatePatient/{id}")]
        public IActionResult UpdatePatient(string id, [FromBody] RegFile model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Thread.Sleep(1000);
            return _lhcService.UpdatePatient(id, model)
                .OnBoth(result => result.IsFailure ? NotFound(result.Error) : (IActionResult)Ok());
        }

        // DELETE api/values/5
        [HttpDelete("DeletePatient/{id}")]
        public IActionResult DeletePatient(string id)
        {
            Thread.Sleep(1000);
            return _lhcService.DeletePatient(id)
                .OnBoth(result => result.IsFailure ? NotFound(result.Error) : (IActionResult)Ok());
        }
    }
}
