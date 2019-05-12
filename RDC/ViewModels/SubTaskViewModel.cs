using RDC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RDC.ViewModels
{
    public class SubTaskViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Priority { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        [Required]
        public string Status { get; set; }

        public int TaskId { get; set; }

        public Task Task { get; set; }

        public List<string> StatusList { get; set; }
        public List<string> PriorityList { get; set; }
        public IEnumerable<SubTask> SubTasksList { get; set; }
    }
}