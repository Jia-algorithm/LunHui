using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DataAccessLibrary.Models
{
    public class Species
    {
        [Key]
        public long SpeciesID { get; set; }

        [Required]
        [MaxLength(50)]
        public string name { get; set; }

        [Required]
        public int line { get; set; }

        [DefaultValue(1)]
        public int status { get; set; }

        public void Clone(Species s)
        {
            this.SpeciesID = s.SpeciesID;
            this.name = s.name;
            this.line = s.line;
        }
    }
}
