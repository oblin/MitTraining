using System;
using System.Collections.Generic;
using System.IO;
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
        public ActionResult<string> Get(string id)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "APP_FILES", $"template{id}.html");
            
            using (var file = new StreamReader(filePath))
            {
                return file.ReadToEnd();
            }

            //            return @"
            //<p>
            //  Dynamic works!  <button class=""btn"" type=""button"" (click)=""cancel()"">Go Back</button>
            //</p>
            //<p>
            //  Get Patient  <button class=""btn"" type=""button"" (click)=""getPatient()"">Get</button>
            //</p>
            //<table class=""table table-condensed table-striped"">
            //  <tr>
            //    <th>病歷號</th>
            //    <th>姓名</th>
            //    <th>年齡</th>
            //  </tr>
            //  <tbody>
            //    <tr *ngFor=""let patient of patients"">
            //      <td><button (click)=""clickPatient(patient)"">{{patient.RegNo}}</button></td>
            //      <td>{{patient.Name}}</td>
            //      <td>{{patient.BirthDate}}</td>
            //    </tr>
            //  </tbody>
            //</table>

            //            ";
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
