using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using JagiCore;
using JagiCore.Admin.Data;
using JagiCore.Helpers;
using Lhc.Data.Data;
using Lhc.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Lhc.Data
{
    public class LhcService
    {
        private readonly LhcContext _context;
        private readonly string _clinicCode;

        public LhcService(Clinic clinic)
        {
            string predfinedConnectionString = "server={3};user id={0};password={1};database={2}";
            // Telephone: as database connection url
            var connectionString = predfinedConnectionString.FormatWith(clinic.DatabaseUser, clinic.DatabasePassword, clinic.Database, clinic.Telephone);
            var options = new DbContextOptionsBuilder<LhcContext>()
                  .UseNpgsql(connectionString)
                  .Options;

            _clinicCode = clinic.Code;
            _context = new LhcContext(options);
        }

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

        public Result<RegFile> GetPatient(string id)
        {
            return _context.RegFiles.Find(id).ToResult();
        }

        public List<BedPatient> GetPaged(int pageNumber, int pageSize, string nurseStation, out int totalCount)
        {
            var whereClause = string.IsNullOrEmpty(nurseStation) ? "" : $" WHERE dba.bed_basic.station_no = '{nurseStation}'";

            using (var connection = _context.Database.GetDbConnection())
            {
                var patients = connection.Query<BedPatient>(_sql1.FormatWith(DateTime.Now.ToTaiwanString(withoutSlash: true)) + _sql2 + whereClause);
                totalCount = patients.Count();
                return patients.OrderBy(r => r.RoomNo).Skip((pageNumber - 1) * pageSize).Take(pageSize)
                    .ToList();
            }
        }

        public List<BedPatient> GetInPatients()
        {
            using (var connection = _context.Database.GetDbConnection())
            {
                var patients = connection.Query<BedPatient>(_sql1.FormatWith(DateTime.Now.ToTaiwanString(withoutSlash: true)) + _sql2);
                return patients.ToList();
            }
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
                .OnBoth(result =>
                {
                    if (result.IsSuccess)
                    {
                        _context.RegFiles.Remove(result.Value);
                        _context.SaveChanges();
                        return Result.Ok();
                    }
                    return Result.Fail($"病人資料沒有此 {id} 病歷號");
                });
        }

        public List<NurseStation> GetNurseStations()
        {
            string sql = "select * from dba.ipd_file i, dba.bed_basic b, dba.reg_file r where i.out_flag = 'I' AND b.bed_no = i.bed_no AND i.reg_no = r.reg_no";
            // Nurse Station code: 1009
            List<NurseStation> result = new List<NurseStation>();
            string codeSql = "select * from dba.code_dtl where item_type = '1009'";

            using (var connection = _context.Database.GetDbConnection())
            {
                var codes = connection.Query<dynamic>(codeSql).ToList();
                if (codes.Count > 0)
                {
                    var allBeds = connection.Query<dynamic>(sql);
                    foreach (var code in codes)
                    {
                        var count = allBeds.Count(b => b.station_no == code.item_code);
                        //var count = connection.QuerySingle<int>(sql.FormatWith(code.ItemCode));
                        if (count > 0)
                        {
                            result.Add(new NurseStation
                            {
                                ItemType = "1009",
                                ItemCode = code.item_code,
                                Description = code.desc_1,
                                ParentType = "LP00",
                                ParentCode = _clinicCode,
                                Count = count
                            });
                        }
                    }
                }
            }

            return result.OrderBy(o => o.Description).ToList();
        }

        public List<ClinicWeekIncome> GetWeekIncome(int year, int endWeek, int prevYear, int startWeek)
        {
            string sql = string.Empty;
            if (year == prevYear)
                sql = $"SELECT * FROM dba.week_income_report WHERE year = '{year}' AND week >= '{startWeek.ToString("00")}' AND week <= '{endWeek.ToString("00")}'";
            else
                sql = $"SELECT * FROM dba.week_income_report WHERE (year = '{year}' AND week <= '{endWeek.ToString("00")}') OR (year = '{prevYear}' AND week >= '{startWeek.ToString("00")}')";
            IEnumerable<WeekIncome> incomesByPatient = new List<WeekIncome>();
            using (var connection = _context.Database.GetDbConnection())
            {
                incomesByPatient = connection.Query<WeekIncome>(sql);
            }
            var result = new List<ClinicWeekIncome>();

            foreach(var week in incomesByPatient.OrderByDescending(i => i.Week).Select(i => i.Week).Distinct())
            {
                var weeklyIncomes = incomesByPatient.Where(i => i.Week == week);
                result.Add(new ClinicWeekIncome
                {
                    Year = year.ToString(),
                    Week = week,
                    EndDate = weeklyIncomes.FirstOrDefault().EndDate.ToString("yyyy/MM/dd"),
                    SocialWeeklyAmount = weeklyIncomes.Sum(i => i.SocialWeeklyAmount),
                    WeeklyAmount = weeklyIncomes.Sum(i => i.WeeklyAmount),
                    UsedWeeklyAmount = weeklyIncomes.Sum(i => i.UsedWeeklyAmount),
                });
            }

            return result;
        }

        /// <summary>
        /// {0}: 必須要輸入今天的日期，格式：1070911 目的在於取出照護費用
        /// </summary>
        const string _sql1 = @"
                SELECT 
                    dba.bed_basic.station_no as StationNo, 
                    dba.bed_basic.room_no as RoomNo, 
                    dba.bed_basic.bed_no as BedNo, 
                    dba.ipd_file.fee_no as FeeNo, 
                    dba.ipd_file.in_date, 
                    dba.reg_file.reg_no as RegNo, 
                    dba.reg_file.p_name as Name, 
                    dba.reg_file.sex, 
                    dba.reg_file.birth_date,
                    dba.reg_file.month_care_amt, 		--早期舊設定病患照護費
                    (select sum(own_amt) 
		                from dba.reg_order 
		                where reg_order.reg_no = dba.reg_file.reg_no and
		                reg_order.charge = 'B' and 
	                '{0}' <= reg_order.end_date) as CareAmt --目前病患照護費抓取（已此為主，但若這邊無設定，再判斷改抓早期舊設定）
                FROM 
                    dba.bed_basic 
                ";
        const string _sql2 = @"
                LEFT JOIN dba.ipd_file ON dba.bed_basic.bed_no = dba.ipd_file.bed_no AND dba.ipd_file.out_flag = 'I' 
                INNER JOIN dba.reg_file ON dba.ipd_file.reg_no = dba.reg_file.reg_no                
                ";
    }
}