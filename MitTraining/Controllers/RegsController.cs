using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JagiCore.Admin;
using JagiCore.Services;
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
        private readonly LhcContext _context;

        public RegsController(IUserResolverService user, CodeService codeService, LhcContext context)
        {
            _codeService = codeService;
            _user = user;
            _context = context;
        }

        // GET api/byContext
        [HttpGet("byContext")]
        public ActionResult<IEnumerable<RegFile>> GetByContext()
        {
            return _context.RegFiles.Take(10).ToList();
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
