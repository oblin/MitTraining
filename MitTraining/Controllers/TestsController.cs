using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JagiCore.Admin;
using JagiCore.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MitTraining.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly CodeService _codeService;
        private readonly IUserResolverService _user;

        public TestsController(IUserResolverService user, CodeService codeService)
        {
            _codeService = codeService;
            _user = user;
        }

        [HttpGet("Test")]
        public ActionResult<string> Test()
        {
            return "test";
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var codeName = _codeService.GetDescription("Sex", "1").Value;
            return new string[] { "Sex = 1", codeName };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
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
