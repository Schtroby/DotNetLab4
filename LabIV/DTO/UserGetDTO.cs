using LabIV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabIV.DTO
{
    public class UserGetDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }

        public static UserGetDTO FromUser(User user)
        {
            return new UserGetDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.UserRole

            };

        }
    }
}
