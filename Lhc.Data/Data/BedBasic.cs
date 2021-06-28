using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lhc.Data.Data
{
    [Table("bed_basic")]
    public class BedBasic
    {
        [Key]
        public string BedNo { get; set; }
        public string BedDesc { get; set; }
        public string RoomNo { get; set; }
        public string StationNo { get; set; }
    }
}
