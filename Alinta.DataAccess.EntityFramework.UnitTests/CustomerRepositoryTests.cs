using System;
using System.Threading.Tasks;
using Alinta.DataAccess.Abstractions.Repositories;
using Alinta.DataAccess.EntityFramework.Contexts;
using Alinta.DataAccess.EntityFramework.Repositories;
using Alinta.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Alinta.DataAccess.EntityFramework.UnitTests
{
    public class CustomerRepositoryTests
    {
        private async Task<ICustomerRepository> GetCustomerRepository()
        {
            var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase("InMemoryAlintaDb")
                .Options;

            var context = new CustomerDbContext(options);
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            return new CustomerRepository(context, new Mock<ILogger<CustomerRepository>>().Object);
        }

        [Fact]
        public async Task If_There_Is_No_Name_Specified_To_Search_It_Must_Return_Failure()
        {
            //
            // Arrange
            //
            string noValue = null;
            var emptyValue = string.Empty;
            var customerRepository = await GetCustomerRepository();
            //
            // Act
            //
            var noValueResult = await customerRepository.GetCustomersByNameAsync(noValue);
            var emptyValueResult = await customerRepository.GetCustomersByNameAsync(emptyValue);
            //
            // Assert
            //
            Assert.False(noValueResult.Status);
            Assert.False(emptyValueResult.Status);
        }

        [Fact]
        public async Task When_There_Are_Customers_With_Matching_Names_Must_Return_Them()
        {
            //
            // Arrange
            //
            var customerRepository = await GetCustomerRepository();
            await customerRepository.CreateCustomerAsync(new Customer
            {
                Id = 1,
                FirstName = "john",
                LastName = "snow",
                DateOfBirth = new DateTime(1982, 11, 1)
            });
            await customerRepository.CreateCustomerAsync(new Customer
            {
                Id = 2,
                FirstName = "little",
                LastName = "john",
                DateOfBirth = new DateTime(1982, 11, 1)
            });
            //
            // Act
            //
            var result = await customerRepository.GetCustomersByNameAsync("john");
            //
            // Assert
            //
            Assert.True(result.Status && result.Data.Count == 2);

        }

        [Fact]
        public async Task If_There_Are_No_Matching_Records_It_Must_Return_An_Empty_List()
        {
            //
            // Arrange
            //
            var customerRepository = await GetCustomerRepository();
            await customerRepository.CreateCustomerAsync(new Customer
            {
                Id = 1,
                FirstName = "aaa",
                LastName = "hatangala",
                DateOfBirth = new DateTime(1982, 11, 1)
            });
            //
            // Act
            //
            var result = await customerRepository.GetCustomersByNameAsync("cheranga");
            //
            // Assert
            //
            Assert.True(result.Status);
            Assert.Empty(result.Data);

        }
    }
}