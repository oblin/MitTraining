using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JagiCore.Admin;
using JagiCore.Admin.Data;

namespace MitTraining.Services
{
    public class FakeUserResolverService : IUserResolverService
    {
        private readonly AdminContext _context;
        private readonly string _userId;
        private Dictionary<string, string> clinics;

        public FakeUserResolverService(AdminContext context)
        {
            _context = context;
            _userId = "1";
            clinics = new Dictionary<string, string>();
            clinics.Add("0000", "測試機構");
        }

        public Dictionary<string, string> GetClinicCodeNames()
        {
            return clinics;
        }

        public List<string> GetClinicCodes()
        {
            return clinics.Keys.ToList();
        }

        public List<Clinic> GetClinics()
        {
            throw new NotImplementedException();
        }

        public List<Clinic> GetDefaultClinics()
        {
            throw new NotImplementedException();
        }

        public string GetDisplayName(string email)
        {
            return "測試員";
        }

        public IList<string> GetRoles()
        {
            throw new NotImplementedException();
        }

        public ApplicationUser GetUser()
        {
            return new ApplicationUser { Id = "1", UserName = "Tester", Email = "tester@example.com", ClinicId = 1 };
        }

        public string GetUserGroupLevel()
        {
            throw new NotImplementedException();
        }

        public string GetUserName(string username)
        {
            return "測試員";
        }
    }
}
