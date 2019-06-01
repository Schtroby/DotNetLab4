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
    public class UsersController : ControllerBase
    {

        private IUsersService _userService;

        public UsersController(IUsersService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginPostDTO login)
        {
            var user = _userService.Authenticate(login.Username, login.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        //[HttpPost]
        public IActionResult Register([FromBody]RegisterPostDTO registerModel)
        {
            var user = _userService.Register(registerModel);
            if (user == null)
            {
                return BadRequest(new { ErrorMessage = "Username already exists." });
            }
            return Ok(user);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,UserManager")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [Authorize(Roles = "Admin,UserManager")]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public void Post([FromBody] UserPostDTO user)
        {
            
            _userService.Create(user);
        }

        [Authorize(Roles = "Admin,UserManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            User currentLogedUser = _userService.GetCurrentUser(HttpContext);
           
            
            var result = _userService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            else if (currentLogedUser.UserRole == UserRole.UserManager)
            {
                User getUser = _userService.GetById(id);
                if(getUser.UserRole == UserRole.Admin)
                {
                    return Forbid();
                }
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,UserManager")]
        public IActionResult Get(int id)
        {
            var found = _userService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }

            return Ok(found);
        }
        [Authorize(Roles = "Admin,UserManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserPostDTO user)
        {
            User currentLogedUser = _userService.GetCurrentUser(HttpContext);
            User getUser = _userService.GetById(id);
            var regDate = currentLogedUser.RegistrationDate;
            var currentDate = DateTime.Now;
            var minDate = currentDate.Subtract(regDate).Days / (365 / 12);

            if (currentLogedUser.UserRole == UserRole.UserManager && getUser.UserRole == UserRole.Admin && minDate < 6 )
            {
                return Forbid();

            }
            else if (currentLogedUser.UserRole == UserRole.UserManager && minDate >= 6)
            {
                var result = _userService.Upsert(id, user);
                return Ok(result);

            }
            UserPostDTO newUser = new UserPostDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
                UserRole = getUser.UserRole.ToString()
            };
            var result1 = _userService.Upsert(id, newUser);
            return Ok(result1);



        }
    }
}
