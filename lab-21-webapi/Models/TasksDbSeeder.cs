using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab_21_webapi.Models
{
    public class TasksDbSeeder
    {
        public static void Initialize(TasksDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any tasks.
            if (context.Tasks.Any())
            {
                return;   // DB has been seeded
            }

            context.Tasks.AddRange(
                new Task
                {
                    Title = "Task 1",
                    Description = "Test 1",
                    DateAdded = DateTime.Now,
                    Deadline = new DateTime(2019, 7, 10, 14, 30, 0),
                    TaskImportance = TaskImportance.Hight,
                    TaskState = (TaskState)1
                },
                new Task
                {
                    Title = "Task 2",
                    Description = "Test 2",
                    DateAdded = DateTime.Now,
                    Deadline = new DateTime(2019, 6, 25, 16, 0, 0),
                    TaskImportance = (TaskImportance)2,
                    TaskState = (TaskState)1
                }
            );
            context.SaveChanges(); // commit transaction
        }
    }
}
