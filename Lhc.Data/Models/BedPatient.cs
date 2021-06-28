using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JagiCore.Helpers;

namespace Lhc.Data.Models
{
    public class BedPatient
    {
        public string FeeNo { get; set; }
        public string RegNo { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }

        public string in_date { get; private set; }
        public string birth_date { get; private set; }

        public string RoomNo { get; set; }
        public string BedNo { get; set; }
        public string StationNo { get; set; }
        public int month_care_amt { get; private set; }
        private int _careAmt;
        public int CareAmt { get {
                if (_careAmt > 0)
                    return _careAmt;
                else
                    return month_care_amt;
            } set { _careAmt = value; } }
        public DateTime InDate
        {
            get { return (DateTime)(this.in_date.ConvertChineseToDateTime()); }
            set
            {
                this.in_date = value.ToTaiwanString(true);
            }
        }
        public DateTime? BirthDate
        {
            get { return this.birth_date.ConvertChineseToDateTime(); }
            set
            {
                this.birth_date = (value != null ? ((DateTime)value).ToTaiwanString(true) : null);
            }
        }
    }
}
