using LabIV.DTO;
using LabIV.Services;
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

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        /// <summary>
        /// Gets all comments filtered by a string
        /// </summary>
        /// <param name="filter">The keyword used to search comments</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public IEnumerable<CommentFilterDTO> GetAll([FromQuery]String filter)
        {
            return commentsService.GetAll(filter);
        }
    }
}
