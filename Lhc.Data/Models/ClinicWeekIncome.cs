using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lhc.Data.Models
{
    public class ClinicWeek
    {
        public string ClinicName { get; set; }
        public string Code { get; set; }
        public Dictionary<string, string> WeekTotalIncomes { get; set; }
        public Dictionary<string, string> WeekIncomes { get; set; }
        public Dictionary<string, string> SocialWeekIncomes { get; set; }
        public Dictionary<string, string> UsedWeekIncomes { get; set; }
    }

    public class ClinicWeekTitle
    {
        public string Week { get; set; }
        public string WeekName { get; set; }
        public string EndDate { get; set; }
    }

    public class WeekIncomes
    {
        public List<ClinicWeekTitle> Titles { get; set; }
        public List<ClinicWeek> ClinicWeeks { get; set; }
    }

    public class ClinicWeekIncome
    {
        public string Year { get; set; }
        public string Week { get; set; }
        public string EndDate { get; set; }

        public decimal WeeklyAmount { get; set; }
        public decimal SocialWeeklyAmount { get; set; }
        public decimal UsedWeeklyAmount { get; set; }
        public decimal WeeklyTotalAmount => this.UsedWeeklyAmount + this.WeeklyAmount;
    }
}
