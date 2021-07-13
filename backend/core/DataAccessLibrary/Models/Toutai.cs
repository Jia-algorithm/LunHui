using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DataAccessLibrary.Models
{
    public class Toutai
    {
        [Required]
        public long FromObjID { get; set; }
        [Required]
        public long ToObjID { get; set; }

        [Required]
        public DateTime date { get; set; }
        [Required]
        
        [DefaultValue(-1)]
        public int WellID { get; set; }
        [DefaultValue(1)]
        public int status { get; set; }
    }
}
