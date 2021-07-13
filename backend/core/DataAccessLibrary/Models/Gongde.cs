using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DataAccessLibrary.Models
{
    public class Gongde
    {
        public long ObjID { get; set; }
        public long eventID { get; set; }

        [Required]
        public DateTime eventTime { get; set; }

        [MaxLength(5000)]
        public string eventContent { get; set; }

        [Required]
        public int eventAssess { get; set; }
        
        [DefaultValue(1)]
        public int status { get; set; }

        
    }
}
