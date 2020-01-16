using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskService.Models;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SimpleTasksController : ControllerBase
    {
        private readonly TasksContext _context;

        public SimpleTasksController(TasksContext context)
        {
            _context = context;
        }

        // Create an end point called GetTasks at the default route that returns all Task objects
        [HttpGet("GetTasks")]
        public async Task<ActionResult<List<SimpleTasks>>>  GetTasks()
        {
            return await _context.SimpleTasks.ToListAsync();
        }

        // Create an end point called GetTask at the default route that takes an Id as the parameter and returns a Task object
        [HttpGet("GetTask/{id}")]
        public async Task<ActionResult<SimpleTasks>> GetTask(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var simpleTasks = await _context.SimpleTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (simpleTasks == null)
            {
                return NotFound();
            }

            return simpleTasks;
        }


        // Create an end point called CreateTask that takes a Task object as the parameter and returns an Id
        [HttpPost("CreateTask")]        
        public async Task<ActionResult<int>> CreateTask([FromBody]SimpleTasks simpleTasks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(simpleTasks);
                await _context.SaveChangesAsync();
                return simpleTasks.Id;
            }
            return -1;
        }

        // Create an end point called UpdateTask that takes a Task object as the parameter and returns a boolean for pass or fail
        [HttpPut("UpdateTask")]        
        public async Task<ActionResult<bool>> UpdateTask([FromBody]SimpleTasks simpleTasks) 
        {
            if (!SimpleTasksExists(simpleTasks.Id))
            {
                return true;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(simpleTasks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SimpleTasksExists(simpleTasks.Id))
                    {
                        return false;
                    }
                    else
                    {
                        throw;
                    }
                }
                return true;
            }
            return false;
        }

        // Create an end point called DeleteTask that takes a Task object as the parameter and returns a boolean for pass or fail
        [HttpDelete("DeleteTask")]
        public async Task<ActionResult<bool>> DeleteTask([FromBody]SimpleTasks simpleTasks)
        {
            if (simpleTasks == null)
            {
                return false;
            }

            if (!SimpleTasksExists(simpleTasks.Id))
            {
                return false;
            }
            
            _context.SimpleTasks.Remove(simpleTasks);
            await _context.SaveChangesAsync();

            return true;
        }

     
        private bool SimpleTasksExists(int id)
        {
            return _context.SimpleTasks.Any(e => e.Id == id);
        }
    }
}
