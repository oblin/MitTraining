using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JagiCore.Angular;
using JagiCore.Helpers;

namespace Lhc.Data.Data
{
    public class RegFile
    {
        [StringLength(10), Required, Key]
        public string RegNo { get; set; }

        [StringLength(20), Required]
        public string Name { get; set; }

        [Required, Dropdown("Sex")]
        public string Sex { get; set; }

        [StringLength(7), Required]
        public string birth_date { get; private set; }

        [StringLength(10), Required]
        public string IdNo { get; set; }

        [NotMapped]
        public DateTime BirthDate
        {
            get {
                if (this.birth_date.Length == 7)
                    return (DateTime)this.birth_date.ConvertChineseToDateTime();
                else
                    return DateTime.Today;
            }
            set
            {
                this.birth_date = value.ToTaiwanString(true);
            }
        }
    }
}
