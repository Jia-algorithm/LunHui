using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DataAccessLibrary.Models
{
    public class Todos
    {
        [Key]
        public long id { get; set; }

        [Required(ErrorMessage ="待办内容不能为空")]
        [MaxLength(1000)]
        public string content { get; set; }

        [Required(ErrorMessage = "待办日期不能为空")]
        public DateTime date { get; set; }

        [DefaultValue(false)]
        public bool isCompleted { get; set; }

        public void Clone(Todos t)
        {
            this.id = t.id;
            this.content = t.content;
            this.date = t.date;
            this.isCompleted = t.isCompleted;
        }
    }
}
