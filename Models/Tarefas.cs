using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SuperoTarefas.Models;

namespace SuperoTarefas.Models
{
    public class Tarefas
    {
        [Display(Name = "Id")]
        public virtual int Id { get; set; }
        [Display(Name = "Título Tarefa")]
        public virtual string TituloTarefa { get; set; }
        [Display(Name = "Descrição Tarefa")]
        public virtual string DesTarefa { get; set; }
        [Display(Name = "Data Criação")]
        public virtual DateTime DataCriacao { get; set; }
        [Display(Name = "Data Última Edição")]
        public virtual DateTime? DataEdicao { get; set; }
        public virtual string DataEdicaoStr { get; set; }
        [Display(Name = "Data Exclusão")]
        public virtual DateTime? DataExclusao { get; set; }
        [Display(Name = "Status")]
        public virtual int TarefasStatusId { get; set; }
        [Display(Name = "Urgencia")]
        public virtual int TarefasUrgenciaId { get; set; }

        public virtual TarefasStatus TarefasStatus { get; set; }
        public virtual TarefasUrgencia TarefasUrgencia { get; set; }

        public virtual List<TarefasStatus> ListTarefasStatus { get; set; }
        public virtual List<TarefasUrgencia> ListTarefasUrgencia { get; set; }
    }
}