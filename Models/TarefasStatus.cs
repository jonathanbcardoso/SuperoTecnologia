using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperoTarefas.Models
{
    public class TarefasStatus
    {
        [Display(Name = "Id")]
        public virtual int Id { get; set; }
        [Display(Name = "Status")]
        public virtual string Status { get; set; }        
    }

}