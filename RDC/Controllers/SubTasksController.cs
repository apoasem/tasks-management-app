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

        public ActionResult ViewAll()
        {
            //var taskId = Convert.ToInt32(TempData["taskId"]);

            var subTasks = db.SubTasks.Include(s => s.Task).Where(s => s.TaskId == taskId).AsQueryable();

            var viewModel = new SubTaskViewModel
            {
                PriorityList = PriorityFiltersList,
                StatusList = StatusFiltersList,
                SubTasksList = subTasks.ToList()
            };

            return View(viewModel);
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
                return RedirectToAction("ViewAll");
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
                return RedirectToAction("ViewAll");
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
            return RedirectToAction("ViewAll");
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
