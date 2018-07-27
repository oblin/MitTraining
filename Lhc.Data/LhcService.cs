using System;
using System.Collections.Generic;
using System.Linq;
using Lhc.Data.Data;
using Microsoft.AspNetCore.Mvc;

namespace Lhc.Data
{
    public class LhcService
    {
        private readonly LhcContext _context;

        public LhcService(LhcContext context)
        {
            _context = context;
        }

        public List<RegFile> GetInHospitalPatients()
        {
            return _context.RegFiles.Join(_context.IpdFiles.Where(i => i.OutFlag == "I"),
                r => r.RegNo, i => i.RegNo, (r, i) => r)
                .ToList();
        }

        public RegFile GetPatient(string id)
        {
            return _context.RegFiles.Find(id);
        }
    }
}