using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
