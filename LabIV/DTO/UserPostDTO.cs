using LabIV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LabIV.DTO
{
    public class UserPostDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [EnumDataType(typeof(UserRole))]
        public string UserRole { get; set; }

        public static User ToUser(UserPostDTO user)
        {
            UserRole UserRole = UserRole.Regular;
            if (user.UserRole == "UserManager")
            {
                UserRole = UserRole.UserManager;
            }
            else if (user.UserRole == "Admin")
            {
                UserRole = UserRole.Admin;
            }


            return new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
                UserRole = UserRole
                
            };
        }
    }
}
    