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
    public class IpdFile
    {
        [StringLength(12), Required, Key]
        public string FeeNo { get; set; }

        [StringLength(12), Required]
        public string RegNo { get; set; }

        [StringLength(20), Required]
        public string Name { get; set; }

        [StringLength(7)]
        public string birth_date { get; private set; }

        [StringLength(1)]
        public string Sex { get; set; }

        [StringLength(7), Required]
        public string in_date { get; private set; }

        [StringLength(4)]
        public string InSource { get; set; }

        public DateTime? OutDate { get; set; }

        [StringLength(1), Required]
        public string OutFlag { get; set; }

        [NotMapped]
        public DateTime InDateAccess
        {
            get { return (DateTime)this.in_date.ConvertChineseToDateTime(); }
            set
            {
                this.in_date = value.ToTaiwanString(true);
            }
        }

        [NotMapped]
        public DateTime? BirthDateAccess
        {
            get { return this.birth_date.ConvertChineseToDateTime(); }
            set
            {
                this.birth_date = (value != null ? ((DateTime)value).ToTaiwanString(true) : null);
            }
        }
    }
}
