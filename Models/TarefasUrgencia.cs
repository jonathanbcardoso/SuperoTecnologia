using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperoTarefas.Models
{
    public class TarefasUrgencia
    {
        [Display(Name = "Id")]
        public virtual int  Id { get; set; }
        //[Display(Name = "Nível Urgência")]
        //public virtual int NivelUrgencia { get; set; }
        [Display(Name = "Urgência")]
        public virtual string Urgencia { get; set; }        
    }

}