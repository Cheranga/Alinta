using System;
using System.Linq;
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

        [Fact]
        public async Task If_Invalid_Data_Is_Sent_To_Delete_Customer_It_Will_Return_Failure()
        {
            //
            // Arrange
            //
            var customerRepository = await GetCustomerRepository();
            //
            // Act
            //
            var result = await customerRepository.DeleteCustomerAsync(0);
            //
            // Assert
            //
            Assert.False(result.Status);
        }

        [Fact]
        public async Task If_The_Customer_Does_Not_Exist_It_Cannot_Be_Deleted()
        {
            //
            // Arrange
            //
            var customerRepository = await GetCustomerRepository();
            //
            // Act
            //
            var result = await customerRepository.DeleteCustomerAsync(100);
            //
            // Assert
            //
            Assert.False(result.Status);
        }

        [Fact]
        public async Task If_Customer_Exists_It_Can_Be_Deleted()
        {
            //
            // Arrange
            //
            var customerRepository = await GetCustomerRepository();
            await customerRepository.CreateCustomerAsync(new Customer
            {
                Id = 1,
                FirstName = "Cheranga",
                LastName = "Hatangala",
                DateOfBirth = new DateTime(1982, 11, 1)
            });
            //
            // Act
            //
            var result = await customerRepository.DeleteCustomerAsync(1);
            //
            // Assert
            //
            Assert.True(result.Status);
        }

        [Fact]
        public async Task If_Customer_Does_Not_Exist_Cannot_Update()
        {
            //
            // Arrange
            //
            var customerRepository = await GetCustomerRepository();
            //
            // Act
            //
            var result = await customerRepository.UpdateCustomerAsync(new Customer
            {
                Id = 1,
                FirstName = "Cheranga",
                LastName = "Hatangala",
                DateOfBirth = new DateTime(1982, 11, 1)
            });
            //
            // Assert
            //
            Assert.False(result.Status);
        }

        [Fact]
        public async Task An_Existing_Customer_Can_Be_Updated()
        {
            //
            // Arrange
            //
            var customerRepository = await GetCustomerRepository();
            await customerRepository.CreateCustomerAsync(new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Terminator",
                DateOfBirth = new DateTime(2000, 1, 1)
            });
            //
            // Act
            //
            var result = await customerRepository.UpdateCustomerAsync(new Customer
            {
                Id = 1,
                FirstName = "Cheranga",
                LastName = "Hatangala",
                DateOfBirth = new DateTime(1982, 11, 1)
            });
            //
            // Assert
            //
            Assert.True(result.Status);

            var getCustomerResult = await customerRepository.GetCustomersByNameAsync("Cheranga");
            Assert.True(getCustomerResult.Status);
            var customers = getCustomerResult.Data;

            Assert.Single(customers);
            Assert.True(customers.First().FirstName == "Cheranga");
            Assert.True(customers.First().LastName == "Hatangala");
            Assert.True(DateTime.Compare(customers.First().DateOfBirth, new DateTime(1982,11,1)) == 0);
        }

        [Fact]
        public async Task If_Invalid_Data_Is_Provided_Cannot_Create_Customer()
        {
            //
            // Arrange
            //
            var customerRepository = await GetCustomerRepository();
            var noFirstNamecustomer = new Customer {LastName = "Hatangala"};
            Customer nullCustomer = null;
            //
            // Act
            //
            var noFirstNamecustomerResult = await customerRepository.CreateCustomerAsync(noFirstNamecustomer);
            var nullCustomerResult = await customerRepository.CreateCustomerAsync(nullCustomer);
            //
            // Assert
            //
            Assert.False(noFirstNamecustomerResult.Status);
            Assert.False(nullCustomerResult.Status);
        }

        [Fact]
        public async Task When_Valid_Data_Is_Provided_Can_Create_Customer()
        {
            //
            // Arrange
            //
            var customerRepository = await GetCustomerRepository();
            var customer = new Customer
            {
                Id = 1,
                FirstName = "Cheranga",
                LastName = "Hatangala",
                DateOfBirth = new DateTime(1982, 11, 1)
            };
            //
            // Act
            //
            var result = await customerRepository.CreateCustomerAsync(customer);
            //
            // Assert
            //
            Assert.True(result.Status);

            var getCustomerOperation = await customerRepository.GetCustomersByNameAsync("Cheranga");
            Assert.True(getCustomerOperation.Status);
            Assert.Same("Cheranga", getCustomerOperation.Data.First().FirstName);
        }
    }
}