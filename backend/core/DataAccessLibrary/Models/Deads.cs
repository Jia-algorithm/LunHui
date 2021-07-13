using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DataAccessLibrary.Models
{
    public class Deads
    {
        [Key]
        public long ObjID { get; set; }

        public long SpeciesID { get; set; }

        [MaxLength(50)]
        public string name { get; set; }

        public DateTime birthTime { get; set; }
        public DateTime deathTime { get; set; }

        [MaxLength(500)]
        public string reason { get; set; }

        [DefaultValue(0)]
        public int value { get; set; }
        [DefaultValue(-1)]
        public long tarSpeciesID { get; set; }
        [DefaultValue(1)]
        public int status { get; set; }

        public void Clone(Deads deads)
        {
            this.ObjID = deads.ObjID;
            this.name = deads.name;
            this.birthTime = deads.birthTime;
            this.deathTime = deads.deathTime;
            this.reason = deads.reason;
            this.value = deads.value;
            this.SpeciesID = deads.tarSpeciesID;
            this.status = deads.status;
        }
    }
}
