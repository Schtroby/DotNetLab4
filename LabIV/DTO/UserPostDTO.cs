using LabIV.Models;
using LabIV.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
                Password = ComputeSha256Hash(user.Password),
                UserRole = UserRole
                
            };
        }

        private static string ComputeSha256Hash(string password)
        {
            // Create a SHA256   
            // TODO: also use salt
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
    