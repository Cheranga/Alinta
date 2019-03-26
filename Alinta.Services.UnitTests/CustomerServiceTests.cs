using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alinta.Core;
using Alinta.DataAccess.Abstractions.Repositories;
using Alinta.DataAccess.EntityFramework.Repositories;
using Alinta.DataAccess.Models;
using Alinta.Services.Abstractions.Models;
using Alinta.Services.Abstractions.Requests;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Alinta.Services.UnitTests
{
    public class CustomerServiceTests
    {
        private CustomerService GetCustomerService()
        {
            var customerRepository = Mock.Of<ICustomerRepository>();
            var logger = Mock.Of<ILogger<CustomerService>>();

            return new CustomerService(customerRepository, logger);
        }

        #region CreateCustomer

        [Fact]
        public async Task When_Null_Customer_Is_Passed_Cannot_Create_Customer()
        {
            //
            // Arrange
            //
            var customerService = GetCustomerService();
            var createCustomerRequestWithNull = new CreateCustomerRequest(null);
            //
            // Act
            //
            var operationResult = await customerService.CreateCustomerAsync(createCustomerRequestWithNull);
            //
            // Assert
            //
            Assert.False(operationResult.Status);
        }

        [Fact]
        public async Task Cannot_Create_Customer_With_Invalid_Data()
        {
            //
            // Arrange
            //
            var customerService = GetCustomerService();
            var firstNameNullCustomer = new CreateCustomerRequest(new CustomerCreateModel(null, "lastname", DateTime.Now.AddYears(-20)));
            var lastNameNullCustomer = new CreateCustomerRequest(new CustomerCreateModel("firstname", null, DateTime.Now.AddYears(-20)));
            var firstNameEmptyCustomer = new CreateCustomerRequest(new CustomerCreateModel("", "lastname", DateTime.Now.AddYears(-20)));
            var lastNameEmptyCustomer = new CreateCustomerRequest(new CustomerCreateModel("firstname", "", DateTime.Now.AddYears(-20)));
            var firstNameWithSpacesCustomer = new CreateCustomerRequest(new CustomerCreateModel("  ", "lastname", DateTime.Now.AddYears(-20)));
            var lastNameWithSpacesCustomer = new CreateCustomerRequest(new CustomerCreateModel("firstname", "  ", DateTime.Now.AddYears(-20)));
            var futureCustomer = new CreateCustomerRequest(new CustomerCreateModel("firstname", "lastname", DateTime.Now.AddYears(1)));

            var invalidRequests = new[]
            {
                firstNameNullCustomer, lastNameNullCustomer, firstNameEmptyCustomer, lastNameEmptyCustomer,
                firstNameWithSpacesCustomer, lastNameWithSpacesCustomer, futureCustomer
            };
            //
            // Act
            //
            var tasks = invalidRequests.Select(x => customerService.CreateCustomerAsync(x));
            var operationResults = await Task.WhenAll(tasks);
            //
            // Assert
            //
            Assert.False(operationResults.All(x => x.Status));
        }

        [Fact]
        public async Task With_Valid_Data_Customer_Can_Be_Created()
        {
            //
            // Arrange
            //
            var guid = Guid.NewGuid().ToString();
            var mockedCustomerRepository = new Mock<ICustomerRepository>();
            mockedCustomerRepository.Setup(x => x.CreateCustomerAsync(It.IsAny<Customer>())).ReturnsAsync(OperationResult<Customer>.Success(new Customer
            {
                Id = guid,
                FirstName = "Cheranga",
                LastName = "Hatangala",
                DateOfBirth = new DateTime(1982, 11, 1)
            }));
            var logger = Mock.Of<ILogger<CustomerService>>();
            var customerService = new CustomerService(mockedCustomerRepository.Object, logger);
            var createCustomerRequest = new CreateCustomerRequest(new CustomerCreateModel("Cheranga", "Hatangala", new DateTime(1982, 11, 1)));
            //
            // Act
            //
            var operationResult = await customerService.CreateCustomerAsync(createCustomerRequest);
            //
            // Assert
            //
            Assert.True(operationResult.Status);
            Assert.NotNull(operationResult.Data?.Customer?.Id);
        }

        #endregion

        #region Update Customer

        [Fact]
        public async Task When_Null_Customer_Is_Passed_Cannot_Update_Customer()
        {
            //
            // Arrange
            //
            var customerService = GetCustomerService();
            var updateCustomerRequestWithNull = new UpdateCustomerRequest(null);
            //
            // Act
            //
            var operationResult = await customerService.UpdateCustomerAsync(updateCustomerRequestWithNull);
            //
            // Assert
            //
            Assert.False(operationResult.Status);
        }

        [Fact]
        public async Task Cannot_Update_Customer_With_Invalid_Data()
        {
            //
            // Arrange
            //
            var customerService = GetCustomerService();
            var idNullCustomer = new UpdateCustomerRequest(new CustomerUpdateModel(null, "firstname", "lastname", DateTime.Now.AddYears(-20)));
            var idEmptyCustomer = new UpdateCustomerRequest(new CustomerUpdateModel("", "firstname", "lastname", DateTime.Now.AddYears(-20)));
            var firstNameNullCustomer = new UpdateCustomerRequest(new CustomerUpdateModel(Guid.NewGuid().ToString(), null, "lastname", DateTime.Now.AddYears(-20)));
            var lastNameNullCustomer = new UpdateCustomerRequest(new CustomerUpdateModel(Guid.NewGuid().ToString(), "firstname", null, DateTime.Now.AddYears(-20)));
            var firstNameEmptyCustomer = new UpdateCustomerRequest(new CustomerUpdateModel(Guid.NewGuid().ToString(), "", "lastname", DateTime.Now.AddYears(-20)));
            var lastNameEmptyCustomer = new UpdateCustomerRequest(new CustomerUpdateModel(Guid.NewGuid().ToString(), "firstname", "", DateTime.Now.AddYears(-20)));
            var firstNameWithSpacesCustomer = new UpdateCustomerRequest(new CustomerUpdateModel(Guid.NewGuid().ToString(), "  ", "lastname", DateTime.Now.AddYears(-20)));
            var lastNameWithSpacesCustomer = new UpdateCustomerRequest(new CustomerUpdateModel(Guid.NewGuid().ToString(), "firstname", "  ", DateTime.Now.AddYears(-20)));
            var futureCustomer = new UpdateCustomerRequest(new CustomerUpdateModel(Guid.NewGuid().ToString(), "firstname", "lastname", DateTime.Now.AddYears(1)));

            var invalidRequests = new[]
            {
                idNullCustomer,idEmptyCustomer,
                firstNameNullCustomer, lastNameNullCustomer, firstNameEmptyCustomer, lastNameEmptyCustomer,
                firstNameWithSpacesCustomer, lastNameWithSpacesCustomer, futureCustomer
            };
            //
            // Act
            //
            var tasks = invalidRequests.Select(x => customerService.UpdateCustomerAsync(x));
            var operationResults = await Task.WhenAll(tasks);
            //
            // Assert
            //
            Assert.False(operationResults.All(x => x.Status));
        }

        [Fact]
        public async Task NonExisting_Customer_Cannot_Be_Updated()
        {
            //
            // Arrange
            //
            var customerRepository = new Mock<ICustomerRepository>();
            customerRepository.Setup(x => x.UpdateCustomerAsync(It.IsAny<Customer>())).ReturnsAsync(OperationResult<Customer>.Failure);
            var customerService = new CustomerService(customerRepository.Object, Mock.Of<ILogger<CustomerService>>());
            var updateCustomerRequest = new UpdateCustomerRequest(new CustomerUpdateModel(Guid.NewGuid().ToString(), "Cheranga", "Hatangala", new DateTime(1982, 11, 1)));
            //
            // Act
            //
            var operationResult = await customerService.UpdateCustomerAsync(updateCustomerRequest);
            //
            // Assert
            //
            Assert.False(operationResult.Status);
        }

        [Fact]
        public async Task Existing_Customer_Can_Be_Updated_With_Valid_Data()
        {
            //
            // Arrange
            //
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "cheranga",
                LastName = "hatangala",
                DateOfBirth = new DateTime(1982, 11, 1)
            };

            var mockedRepository = new Mock<ICustomerRepository>();
            mockedRepository.Setup(x => x.UpdateCustomerAsync(It.IsAny<Customer>())).ReturnsAsync(OperationResult<Customer>.Success(customer));
            var customerService = new CustomerService(mockedRepository.Object, Mock.Of<ILogger<CustomerService>>());
            var existingCustomer = new CustomerUpdateModel(customer.Id, customer.FirstName, customer.LastName, customer.DateOfBirth);
            var updateCustomerRequest = new UpdateCustomerRequest(existingCustomer);
            //
            // Act
            //
            var operationResult = await customerService.UpdateCustomerAsync(updateCustomerRequest);
            //
            // Assert
            //
            Assert.True(operationResult.Status);
            Assert.Equal(existingCustomer.Id, operationResult.Data.Customer.Id);
        }

        #endregion

        #region Search Customers

        [Fact]
        public async Task If_There_Are_No_Customers_Matching_The_Filter_Criteria_The_Operation_Must_Be_Success_And_Results_Must_Be_Empty()
        {
            //
            // Arrange
            //
            var mockedCustomerRepository  = new Mock<ICustomerRepository>();
            mockedCustomerRepository.Setup(x => x.GetCustomersByNameAsync(It.IsAny<string>())).ReturnsAsync(OperationResult<List<Customer>>.Success(new List<Customer>()));
            var customerService = new CustomerService(mockedCustomerRepository.Object, Mock.Of<ILogger<CustomerService>>());
            var searchRequest = new SearchCustomersRequest("cheranga");
            //
            // Act
            //
            var operationResult = await customerService.SearchCustomersAsync(searchRequest);
            //
            // Assert
            //
            Assert.True(operationResult.Status);
            Assert.Empty(operationResult.Data.Customers);
        }

        [Fact]
        public async Task If_There_Are_Customers_Matching_The_Filter_Criteria_They_Must_Be_Returned()
        {
            //
            // Arrange
            //
            var mockedCustomerRepository = new Mock<ICustomerRepository>();
            mockedCustomerRepository.Setup(x => x.GetCustomersByNameAsync("john")).ReturnsAsync(OperationResult<List<Customer>>.Success(new List<Customer>
            {
                new Customer {FirstName = "john"},
                new Customer {LastName = "JOHN"}
            }));
            var customerService = new CustomerService(mockedCustomerRepository.Object, Mock.Of<ILogger<CustomerService>>());
            var searchRequest = new SearchCustomersRequest("john");
            //
            // Act
            //
            var operationResult = await customerService.SearchCustomersAsync(searchRequest);
            //
            // Assert
            //
            Assert.True(operationResult.Status);
            Assert.Equal(2, operationResult.Data.Customers.Count);
        }

        #endregion


    }
}
