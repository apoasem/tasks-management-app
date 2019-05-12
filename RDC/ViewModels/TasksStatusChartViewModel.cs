using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDC.ViewModels
{
    public class TasksStatusChartViewModel
    {
        public int TasksBacklog { get; set; }
        public int TasksProgress { get; set; }
        public int TasksDone { get; set; }

    }
}