using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DataAccessLibrary.Models
{
    public class Zhuansheng
    {
        [Key]
        public long ObjID { get; set; }
        [Required]
        public long SpeciesID { get; set; }

        [Required]
        [MaxLength(2000)]
        public String detail { get; set; }
        [Required]
        [DefaultValue(1)]
        public int status { get; set; }
    }
}
