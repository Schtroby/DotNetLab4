using LabIV.DTO;
using LabIV.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Task = LabIV.Models.Task;


namespace LabIV.Services
{
    public interface IUsersService
    {
        LogInGetDTO Authenticate(string username, string password);
        LogInGetDTO Register(RegisterPostDTO registerInfo);
        User GetCurrentUser(HttpContext httpContext);
        User Create(UserPostDTO user);
        User Delete(int id);
        User GetById(int id);
        User Upsert(int id, UserPostDTO user);
        IEnumerable<UserGetDTO> GetAll();
    }

    public class UsersService : IUsersService
    {
        private TasksDbContext context;
        private readonly AppSettings appSettings;

        public UsersService(TasksDbContext context, IOptions<AppSettings> appSettings)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
        }

        public LogInGetDTO Authenticate(string username, string password)
        {
            var user = context.Users
                .SingleOrDefault(x => x.Username == username &&
                                 x.Password == ComputeSha256Hash(password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, user.UserRole.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = new LogInGetDTO
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Token = tokenHandler.WriteToken(token)
            };
            // remove password before returning
            return result;
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            // TODO: also use salt
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public LogInGetDTO Register(RegisterPostDTO registerInfo)
        {
            User existing = context.Users.FirstOrDefault(u => u.Username == registerInfo.Username);
            if (existing != null)
            {
                return null;
            }

            context.Users.Add(new User
            {
                Email = registerInfo.Email,
                LastName = registerInfo.LastName,
                FirstName = registerInfo.FirstName,
                Password = ComputeSha256Hash(registerInfo.Password),
                Username = registerInfo.Username,
                UserRole = UserRole.Regular
            });
            context.SaveChanges();
            return Authenticate(registerInfo.Username, registerInfo.Password);
        }

        public User GetCurrentUser(HttpContext httpContext)
        {
            string username = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            //string accountType = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.AuthenticationMethod).Value;
            //return _context.Users.FirstOrDefault(u => u.Username == username && u.AccountType.ToString() == accountType);
            return context.Users.FirstOrDefault(u => u.Username == username);
        }

        public IEnumerable<UserGetDTO> GetAll()
        {
            // return users without passwords
            return context.Users.Select(user => new UserGetDTO
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Role = user.UserRole
            });
        }
        public User Create(UserPostDTO user)
        {
            User userAdd = UserPostDTO.ToUser(user);
            context.Users.Add(userAdd);
            context.SaveChanges();
            return userAdd;
        }

        public User Delete(int id)
        {
            var existing = context.Users.FirstOrDefault(user => user.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Users.Remove(existing);
            context.SaveChanges();
            return existing;
        }

        public User GetById(int id)
        {
            return context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User Upsert(int id, UserPostDTO user)
        {
            var existing = context.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);
            if (existing == null)
            {
                User UserAdd = UserPostDTO.ToUser(user);
                context.Users.Add(UserAdd);
                context.SaveChanges();
                return UserAdd;

            }
            User UserUp = UserPostDTO.ToUser(user);
            user.Id = id;
            context.Users.Update(UserUp);
            context.SaveChanges();
            return UserUp;
        }

    }

}
