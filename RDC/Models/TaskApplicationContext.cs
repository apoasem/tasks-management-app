using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RDC.Models
{
    public class TaskApplicationContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }

        public TaskApplicationContext()
            :base("name=DefaultConnection")
        {

        }

    }
}