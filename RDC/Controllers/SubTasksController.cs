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
    public class SubTasksController : Controller
    {
        private TaskApplicationContext db = new TaskApplicationContext();

        private static string StatusFilter = "None";
        private static string PriorityFilter = "None";
        private static string Sort = "None";

        private List<string> StatusList = new List<string> { TaskStatus.BACKLOG, TaskStatus.PROGRESS, TaskStatus.DONE};
        private List<string> StatusFiltersList = new List<string> { TaskStatus.BACKLOG, TaskStatus.PROGRESS, TaskStatus.DONE, TaskStatus.NONE };

        private List<string> PriorityList = new List<string> { SubTaskPriority.LOW, SubTaskPriority.MID, SubTaskPriority.HIGH };
        private List<string> PriorityFiltersList = new List<string> { SubTaskPriority.LOW, SubTaskPriority.MID, SubTaskPriority.HIGH, SubTaskPriority.NONE };

        private static int taskId = 0;

        // GET: SubTasks
        public ActionResult Index(int id)
        {
            taskId = id;
            return View();
        }

        public ActionResult ViewAllSubTasks()
        {
            var subTasks = db.SubTasks.Include(s => s.Task).Where(s => s.TaskId == taskId).AsQueryable();

            var viewModel = new SubTaskViewModel
            {
                PriorityList = PriorityFiltersList,
                StatusList = StatusFiltersList,
                SubTasksList = subTasks.ToList()
            };

            return View(viewModel);
        }

        [Route("SubTasks/ViewAllSubTasksTable/{sort?}/{statusFilter?}/{priorityFilter?}")]
        public ActionResult ViewAllSubTasksTable(string sort, string statusFilter, string priorityFilter)
        {
            var subTasks = db.SubTasks.AsQueryable();

            if (String.IsNullOrWhiteSpace(statusFilter) || statusFilter == "null")
            {
                statusFilter = StatusFilter; // get the last filter value
            }

            if (String.IsNullOrWhiteSpace(priorityFilter) || priorityFilter == "null")
            {
                priorityFilter = PriorityFilter; // get the last filter value
            }

            if (statusFilter == "None")
            {
                StatusFilter = statusFilter;
            }

            if (priorityFilter == "None")
            {
                PriorityFilter = priorityFilter;
            }

            if (statusFilter != "None" && priorityFilter != "None")
            {
                subTasks = subTasks.Where(t => t.Priority == priorityFilter).Where(t => t.Status == statusFilter);
                StatusFilter = statusFilter; // get the last filter value
                PriorityFilter = priorityFilter; // get the last filter value

            } else if ((!String.IsNullOrWhiteSpace(statusFilter) || statusFilter != "null") && statusFilter != "None")
            {
                if (statusFilter == TaskStatus.BACKLOG || statusFilter == TaskStatus.PROGRESS || statusFilter == TaskStatus.DONE)
                {
                    subTasks = subTasks.Where(t => t.Status == statusFilter);
                }

                StatusFilter = statusFilter;

            } else if ((!String.IsNullOrWhiteSpace(priorityFilter) || priorityFilter != "null") && priorityFilter != "None")
            {
                if (priorityFilter == SubTaskPriority.LOW || priorityFilter == SubTaskPriority.MID || priorityFilter == SubTaskPriority.HIGH)
                {
                    subTasks = subTasks.Where(t => t.Priority == priorityFilter);
                }

                PriorityFilter = priorityFilter;
            }

            if (String.IsNullOrWhiteSpace(sort) || sort == "null")
            {
                sort = Sort; // get the last sort value
            }

            if ((!String.IsNullOrWhiteSpace(sort) || sort != "null") && sort != "None")
            {
                if (sort == Sorting.ASEC)
                {
                    subTasks = subTasks.OrderBy(t => t.Title);
                }
                else if (sort == Sorting.DESC)
                {
                    subTasks = subTasks.OrderByDescending(t => t.Title);
                }

                Sort = sort;
            }

            var viewModel = new SubTaskViewModel
            {
                SubTasksList = subTasks.ToList()
            };

            return PartialView("_ViewAllSubTasksTablePartial", viewModel);
        }

        // GET: SubTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubTask subTask = db.SubTasks.Find(id);
            if (subTask == null)
            {
                return HttpNotFound();
            }
            return View(subTask);
        }

        // GET: SubTasks/Create
        public ActionResult Create()
        {
            var viewModel = new SubTaskViewModel
            {
                TaskId = taskId,
                PriorityList = PriorityList,
                StatusList = StatusList,
            };

            return View(viewModel);
        }

        // POST: SubTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubTask subTask)
        {
            if (ModelState.IsValid)
            {
                db.SubTasks.Add(subTask);
                db.SaveChanges();
                return RedirectToAction("ViewAllSubTasks");
            }

            var viewModel = new SubTaskViewModel()
            {
                StatusList = StatusList,
                PriorityList = PriorityList,
                Date = subTask.Date,
                Description = subTask.Description,
                Status = subTask.Status,
                Title = subTask.Title,
                Priority = subTask.Priority,
                TaskId = subTask.TaskId
            };

            //ViewBag.TaskId = new SelectList(db.Tasks, "Id", "Title",subTask.TaskId);
            return View(viewModel);
        }

        // GET: SubTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubTask subTask = db.SubTasks.Find(id);
            if (subTask == null)
            {
                return HttpNotFound();
            }

            var viewModel = new SubTaskViewModel()
            {
                Id = subTask.Id,
                StatusList = StatusList,
                Date = subTask.Date,
                Description = subTask.Description,
                Status = subTask.Status,
                Title = subTask.Title,
                PriorityList = PriorityList,
                Priority = subTask.Priority,
                TaskId = subTask.TaskId
            };

            return View(viewModel);
        }

        // POST: SubTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubTask subTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewAllSubTasks");
            }

            var viewModel = new SubTaskViewModel()
            {
                Id = subTask.Id,
                StatusList = StatusList,
                Date = subTask.Date,
                Description = subTask.Description,
                Status = subTask.Status,
                Title = subTask.Title,
                PriorityList = PriorityList,
                Priority = subTask.Priority,
                TaskId = subTask.TaskId
            };

            return View(viewModel);
        }

        // POST: SubTasks/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SubTask subTask = db.SubTasks.Find(id);

            if (subTask == null)
            {
                return HttpNotFound();
            }

            db.SubTasks.Remove(subTask);
            db.SaveChanges();
            return RedirectToAction("ViewAllSubTasks");
        }


        ~SubTasksController() {

            StatusFilter = "None";
            PriorityFilter = "None";
            Sort = "None";
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
