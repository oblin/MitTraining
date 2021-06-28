using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lhc.Data.Data
{
    public class CodeFile
    {
        [Key]
        public string ItemType { get; set; }
        public string TypeName { get; set; }
    }

    public class CodeDetail
    {
        public string ItemType { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
    }
}
