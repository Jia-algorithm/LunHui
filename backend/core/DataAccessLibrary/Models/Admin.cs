using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DataAccessLibrary.Models
{
    public class Admin
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int AdminName { get; set; }
        [Required]
        public string password { get; set; }
    }
}
