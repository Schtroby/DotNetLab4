using LabIV.DTO;
using LabIV.Models;
using LabIV.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabIV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentsService commentsService;
        private IUsersService usersService;

        public CommentsController(ICommentsService commentsService, IUsersService usersService)
        {
            this.commentsService = commentsService;
            this.usersService = usersService;
        }

        /// <summary>
        /// Gets all comments filtered by a string
        /// </summary>
        /// <param name="filter">The keyword used to search comments</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        //[Authorize(Roles = "Admin,Regular")]
        public IEnumerable<CommentFilterDTO> GetAll([FromQuery]String filter)
        {
            return commentsService.GetAll(filter);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Regular")]
        public IActionResult Get(int id)
        {
            var found = commentsService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }

            return Ok(found);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Regular")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public void Post([FromBody] CommentPostDTO comment)
        {
            User addedBy = usersService.GetCurrentUser(HttpContext);
            commentsService.Create(comment, addedBy);
        }

        [Authorize(Roles = "Admin,Regular")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Comment comment)
        {
            var result = commentsService.Upsert(id, comment);
            return Ok(result);
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Regular")]
        public IActionResult Delete(int id)
        {
            var result = commentsService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


    }
}
