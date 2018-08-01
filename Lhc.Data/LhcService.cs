﻿using System;
using System.Collections.Generic;
using System.Linq;
using JagiCore;
using JagiCore.Helpers;
using JagiCore.Interfaces;
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

        public Result<List<RegFile>> GetInHospitalPatients()
        {
            return _context.RegFiles.Join(_context.IpdFiles.Where(i => i.OutFlag == "I"),
                r => r.RegNo, i => i.RegNo, (r, i) => r)
                .ToList()
                .ToResult();
        }

        public Result<RegFile> GetPatient(string id)
        {
            return _context.RegFiles.Find(id).ToResult();
        }

        public List<RegFile> GetPaged(int pageNumber, int pageSize, out int totalCount)
        {
            totalCount = _context.RegFiles.Count();
            var regFiles = _context.RegFiles.OrderBy(r => r.RegNo).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return regFiles.ToList();
        }

        public RegFile AddPatient(RegFile model)
        {
            _context.RegFiles.Add(model);
            _context.SaveChanges();
            return model;
        }

        public Result UpdatePatient(string id, RegFile model)
        {
            var result = GetPatient(id);
            if (result.IsSuccess)
            {
                var patient = result.Value;
                model.CopyTo(patient);
                _context.RegFiles.Update(patient);
                _context.SaveChanges();
                return Result.Ok();
            }

            return Result.Fail($"病人資料沒有此 {id} 病歷號");
        }

        public Result DeletePatient(string id)
        {
            return GetPatient(id)
                .OnBoth(result => {
                    if (result.IsSuccess)
                    {
                        _context.RegFiles.Remove(result.Value);
                        _context.SaveChanges();
                        return  Result.Ok();
                    }
                    return Result.Fail($"病人資料沒有此 {id} 病歷號");
                });
        }
    }
}