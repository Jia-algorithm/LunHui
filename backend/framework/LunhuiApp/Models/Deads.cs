using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace LunhuiApp.Models
{
    public class Deads
    {
        public long ObjID { get; set; }
        public long SpeciesID { get; set; }
        public string name { get; set; }
        public DateTime birthTime { get; set; }
        public DateTime deathTime { get; set; }
        public string reason { get; set; }

        [DefaultValue(0)]
        public int value { get; set; }
        [DefaultValue(-1)]
        public long tarSpeciesID { get; set; }
        [DefaultValue(1)]
        public int status { get; set; }
        
    }
}