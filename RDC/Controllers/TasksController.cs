using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RDC.Models;
using RDC.ViewModels;

namespace RDC.Controllers
{
    public class TasksController : Controller
    {
        private TaskApplicationContext db = new TaskApplicationContext();

        private static string Filter = "None";
        private static string Sort = "None";

        private List<string> StatusList = new List<string> { TaskStatus.BACKLOG, TaskStatus.PROGRESS, TaskStatus.DONE };
        private List<string> StatusFiltersList = new List<string> { TaskStatus.BACKLOG, TaskStatus.PROGRESS, TaskStatus.DONE, TaskStatus.NONE };


        // GET: Tasks
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult TasksPagePartial()
        {
            Filter = "None";
            Sort = "None";
            return PartialView("_TasksPagePartial");
        }

        
        public ActionResult ViewAllTasks()
        {
            var tasks = db.Tasks.AsQueryable();

            var viewModel = new TaskViewModel
            {
                StatusList = StatusFiltersList,
                TasksList = tasks.ToList()
            };

            return View(viewModel);
        }

        [Route("Tasks/ViewAllTasksTable/{sort?}/{filter?}")]
        public PartialViewResult ViewAllTasksTable(string sort, string filter)
        {
            var tasks = db.Tasks.AsQueryable();

            if (String.IsNullOrEmpty(filter))
            {
                filter = Filter; // get the last filter value
            }

            if (!String.IsNullOrEmpty(filter))
            {
                if (filter == TaskStatus.BACKLOG || filter == TaskStatus.PROGRESS || filter == TaskStatus.DONE)
                {
                    tasks = tasks.Where(t => t.Status == filter);
                }

                Filter = filter;
            }

            if (String.IsNullOrEmpty(sort))
            {
                sort = Sort; // get the last sort value
            }

            if (!String.IsNullOrEmpty(sort))
            {
                if (sort == Sorting.ASEC)
                {
                    tasks = tasks.OrderBy(t => t.Title);
                }
                else if (sort == Sorting.DESC)
                {
                    tasks = tasks.OrderByDescending(t => t.Title);
                }

                Sort = sort;
            }

            var viewModel = new TaskViewModel
            {
                TasksList = tasks.ToList()
            };

            return PartialView("_ViewAllTasksTablePartial", viewModel);
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            var viewModel = new TaskViewModel()
            {
                StatusList = StatusList
            };

            return View(viewModel);
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Task task)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("ViewAllTasks");
            }

            var viewModel = new TaskViewModel()
            {
                StatusList = StatusList,
                Date = task.Date,
                Description = task.Description,
                Status = task.Status,
                Title = task.Title
            };

            return View(viewModel);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Find(id);

            if (task == null)
            {
                return HttpNotFound();
            }

            var viewModel = new TaskViewModel()
            {
                Id = task.Id,
                StatusList = StatusList,
                Date = task.Date,
                Description = task.Description,
                Status = task.Status,
                Title = task.Title
            };

            return View(viewModel);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewAllTasks");
            }

            var viewModel = new TaskViewModel()
            {
                StatusList = StatusList,
                Date = task.Date,
                Description = task.Description,
                Status = task.Status,
                Title = task.Title
            };

            return View(viewModel);
        }

        // POST: Tasks/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            db.Tasks.Remove(task);
            db.SaveChanges();

            return RedirectToAction("ViewAllTasks");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
