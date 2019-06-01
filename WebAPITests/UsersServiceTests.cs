using LabIV.DTO;
using LabIV.Models;
using LabIV.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class UsersServiceTests
    {
        private IOptions<AppSettings> config;

        [SetUp]
        public void Setup()
        {
            config = Options.Create(new AppSettings
            {
                Secret = "THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING"
            });

        }

        [Test]
        public void ValidRegisterShouldCreateANewUser()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidRegisterShouldCreateANewUser))// "ValidRegisterShouldCreateANewUser")
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var usersService = new UsersService(context, config);
                var added = new LabIV.DTO.RegisterPostDTO

                {
                    
                    FirstName = "Julia",
                    LastName = "Bush",
                    Username = "julia",
                    Email = "julia@gmail.com",
                    Password = "1234567"                    
                };
                var result = usersService.Register(added);

                Assert.IsNotNull(result);
                Assert.AreEqual(added.Username, result.Username);

            }
        }

        [Test]
        public void AuthenticateShouldLogInAUser()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(AuthenticateShouldLogInAUser))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var usersService = new UsersService(context, config);
                var added = new LabIV.DTO.RegisterPostDTO

                {

                    FirstName = "Julia",
                    LastName = "Bush",
                    Username = "julia",
                    Email = "julia@gmail.com",
                    Password = "1234567"
                };
                var result = usersService.Register(added);
                var authenticated = new LabIV.DTO.LoginPostDTO
                {
                    Username = "julia",
                    Password = "1234567"
                };
                var authresult = usersService.Authenticate(added.Username, added.Password);

                Assert.IsNotNull(authresult);
                Assert.AreEqual(1, authresult.Id);
                Assert.AreEqual(authenticated.Username, authresult.Username);
            }
        }

        [Test]
        public void GetAllShouldReturnAllUser()
        {
            var options = new DbContextOptionsBuilder<TasksDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(AuthenticateShouldLogInAUser))
              .Options;

            using (var context = new TasksDbContext(options))
            {
                var usersService = new UsersService(context, config);
                var added = new LabIV.DTO.RegisterPostDTO

                {

                    FirstName = "Julia",
                    LastName = "Bush",
                    Username = "julia",
                    Email = "julia@gmail.com",
                    Password = "1234567"
                };
                var result = usersService.Register(added);

                var users = usersService.GetAll();
               
                Assert.IsNotEmpty(users);
                Assert.AreEqual(1, users.Count());
                //Assert.IsEmpty(users);

            }
        }


    }
}