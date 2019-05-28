using System;
using System.Collections.Generic;
using System.Linq;
using LabIV.DTO;
using LabIV.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task = LabIV.Models.Task;


namespace LabIV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TasksController : ControllerBase
    {
        private ITasksService tasksService;

        public TasksController(ITasksService tasksService)
        {
            this.tasksService = tasksService;
        }

        // GET: api/Tasks
        /// <summary>
        /// Gets all the tasks in the database.
        /// </summary>

        /// <param name="from">Optional, filter by minimum deadline.</param>
        /// <param name="to">Optional, filter by maximum deadline.</param>
        /// <returns>A list of Task objects.</returns>
        [HttpGet]
        public PaginatedList<TaskGetDTO> Get([FromQuery]DateTime? from, [FromQuery]DateTime? to, [FromQuery]int page = 1)
        {

            return tasksService.GetAll(page, from, to);
        }

        // GET: api/Tasks/5
        /// <summary>
        /// Get a task by a given ID.
        /// </summary>
        /// 
        /// <param name="id">Task ID</param>
        /// <returns>A task with a given ID</returns>
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var found = tasksService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }

            return Ok(found);
        }

        // POST: api/Tasks
        /// <summary>
        /// Add a task
        /// </summary>
        ///  <remarks>
        /// Sample request:
        ///
        ///     POST /tasks
        ///    {
        ///         "title": "Expedia Conference",
        ///         "description": "Attending Expedia Conference at Hilton",
        ///         "dateAdded": "2019-05-17T10:08:57.3616694",
        ///         "deadline": "2019-06-01T12:00:00",
        ///         "taskImportance": 2,
        ///         "taskState": 0,
        ///         "dateClosed": null,
        ///         "comments": [
        ///	            {
        ///		            "text": "Write down some important questions!",
        ///		            "important": true
        ///             },
        ///	            {
        ///		            "text": "Get all the relevant info for the market!",
        ///		            "important": false
        ///	            }
        ///	                    ]
        ///     }
        ///
        /// </remarks>
        /// <param name="task">The task to add.</param>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
       // [Authorize]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public void Post([FromBody] Task task)
        {
            tasksService.Create(task);
        }

        // PUT: api/Tasks/5
        /// <summary>
        /// Update a task with the given ID, or create a new task if the ID does not exist.
        /// </summary>
        ///<remarks>
        /// Sample request:
        ///
        ///     PUT /tasks
        ///    {
        ///         "title": "Expedia Conference",
        ///         "description": "Attending Expedia Conference at Hilton",
        ///         "dateAdded": "2019-05-17T10:08:57.3616694",
        ///         "deadline": "2019-06-01T12:00:00",
        ///         "taskImportance": 2,
        ///         "taskState": 0,
        ///         "dateClosed": null,
        ///         "comments": [
        ///	            {
        ///		            "text": "Write down some important questions!",
        ///		            "important": true
        ///             },
        ///	            {
        ///		            "text": "Get all the relevant info for the market!",
        ///		            "important": false
        ///	            }
        ///	                    ]
        ///     }
        ///
        /// </remarks>
        /// <param name="id">task ID</param>
        /// <param name="task">The object Task</param>
        /// <returns>The updated task/new created task.</returns>
        //[Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Task task)
        {
            var result = tasksService.Upsert(id, task);
            return Ok(result);
        }

        // DELETE: api/ApiWithActions/5
        /// <summary>
        /// Delet a task by a given ID
        /// </summary>
        /// <param name="id">ID of the task to be deleted.</param>
        /// <returns>The deleted task object.</returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = tasksService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

    }
}