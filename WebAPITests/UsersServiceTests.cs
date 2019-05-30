using LabIV.Models;
using LabIV.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;

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

    }
}