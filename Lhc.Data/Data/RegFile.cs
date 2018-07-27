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
    public class RegFile
    {
        [StringLength(10), Required, Key]
        public string RegNo { get; set; }

        [StringLength(20), Required]
        public string Name { get; set; }

        [StringLength(1), Required]
        public string Sex { get; set; }

        [StringLength(7), Required]
        public string birth_date { get; private set; }

        [StringLength(10), Required]
        public string IdNo { get; set; }

        [NotMapped]
        public DateTime BirthDate
        {
            get { return (DateTime)this.birth_date.ConvertChineseToDateTime(); }
            set
            {
                this.birth_date = value.ToTaiwanString(true);
            }
        }
    }
}
