using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JagiCore.Helpers;

namespace Lhc.Data.Data
{
    public class WeekIncome
    {
        [StringLength(4), Required]
        public string Year { get; set; }

        [StringLength(2), Required]
        public string Week { get; set; }

        [StringLength(10), Required]
        public string RegNo { get; set; }

        [StringLength(7), Required]
        public string beg_date { get; private set; }

        [StringLength(7), Required]
        public string end_date { get; private set; }

        public decimal used_week_amt { get; set; }

        public decimal ca_week_amt { get; set; }


        public decimal al_month_amt { get; set; }
        public decimal ca_month_amt { get; set; }
        public decimal al_week_amt { get; set; }

        [NotMapped]
        public DateTime BeginDate
        {
            get
            {
                if (this.beg_date.Length == 7)
                    return (DateTime)this.beg_date.ConvertChineseToDateTime();
                else
                    return DateTime.Today;
            }
            set
            {
                this.beg_date = value.ToTaiwanString(true);
            }
        }

        [NotMapped]
        public DateTime EndDate
        {
            get
            {
                if (this.end_date.Length == 7)
                    return (DateTime)this.end_date.ConvertChineseToDateTime();
                else
                    return DateTime.Today;
            }
            set
            {
                this.end_date = value.ToTaiwanString(true);
            }
        }

        [NotMapped]
        public decimal MothlyAmount => this.ca_month_amt;

        [NotMapped]
        public decimal SocialMothlyAmount => this.al_month_amt;

        [NotMapped]
        public decimal WeeklyAmount => this.ca_week_amt;

        [NotMapped]
        public decimal SocialWeeklyAmount => this.al_week_amt;

        [NotMapped]
        public decimal UsedWeeklyAmount => this.used_week_amt;
    }
}
