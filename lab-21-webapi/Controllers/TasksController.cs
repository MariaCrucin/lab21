﻿using System;
using System.Collections.Generic;
using System.Linq;
using lab_21_webapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab_21_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private TasksDbContext context;
        public TasksController(TasksDbContext context)
        {
            this.context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public IEnumerable<Models.Task> Get([FromQuery]DateTime? deadlineFrom, [FromQuery]DateTime? deadlineTo)
        {
            IQueryable<Task> result = context.Tasks.Include(t => t.Comments);

            if (deadlineFrom == null && deadlineTo == null)
                return result;

            if (deadlineFrom != null)
                result = result.Where(t => t.Deadline >= deadlineFrom);
            
            if (deadlineTo != null)
                result= result.Where(t => t.Deadline <= deadlineTo);

            return result;
        }

        // GET: api/Tasks/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var existing = context.Tasks.Include(t => t.Comments).FirstOrDefault(task => task.Id == id);
            if (existing == null)
            {
                return NotFound();
            }

            return Ok(existing);
        }

        // POST: api/Tasks
        [HttpPost]
        public void Post([FromBody] Task task)
        {
            task.DateClosed = null;
            task.DateAdded = DateTime.Now;
            context.Tasks.Add(task);
            context.SaveChanges();
        }

        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Task task)
        { 
            var existing = context.Tasks.AsNoTracking().FirstOrDefault(t => t.Id == id);
            if (existing == null)
            {
                task.DateClosed = null;
                task.DateAdded = DateTime.Now;
                context.Tasks.Add(task);
                context.SaveChanges();
                return Ok(task);
            }
            task.Id = id;
            if (existing.TaskState != TaskState.Closed && task.TaskState == TaskState.Closed)
                task.DateClosed = DateTime.Now;
            else if (existing.TaskState == TaskState.Closed && task.TaskState != TaskState.Closed)
                task.DateClosed = null;

            context.Tasks.Update(task);
            context.SaveChanges();
            return Ok(task);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = context.Tasks.Include(t => t.Comments).FirstOrDefault(t => t.Id == id);
            if (existing == null)
            {
                return NotFound();
            }
            context.Tasks.Remove(existing);
            context.SaveChanges();
            return Ok();
        }
    }
}
