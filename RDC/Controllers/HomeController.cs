using RDC.Models;
using RDC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RDC.Controllers
{
    public class HomeController : Controller
    {
        private TaskApplicationContext db = new TaskApplicationContext();
        public ActionResult Index()
        {
            var tasksBacklog = db.Tasks.Count(m => m.Status == TaskStatus.BACKLOG);
            var tasksProgress = db.Tasks.Count(m => m.Status == TaskStatus.PROGRESS);
            var tasksDone = db.Tasks.Count(m => m.Status == TaskStatus.DONE);

            var viewModel = new TasksStatusChartViewModel
            {
                TasksBacklog = tasksBacklog,
                TasksProgress = tasksProgress,
                TasksDone = tasksDone
            };

            return View(viewModel);
        }
    }
}