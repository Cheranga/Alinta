using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Alinta.WebApi.DTO.Requests;
using Alinta.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Alinta.WebApi.IntegrationTests
{
    public class CustomersControllerTests: IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string BaseUrl = @"/api/v1/customers";
        private readonly HttpClient _client;

        public CustomersControllerTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Valid_Create_Customer_Request_Must_Be_Successful()
        {
            //
            // Arrange
            //
            var request = new CreateCustomerRequest
            {
                FirstName = "a",
                LastName = "b",
                DateOfBirth = new DateTime(1982, 1, 1)
            };
            //
            // Act
            //
            var httpResponse = await _client.PostAsJsonAsync(BaseUrl, request);
            //
            // Assert
            //
            httpResponse.EnsureSuccessStatusCode();

            var responseModel =  JsonConvert.DeserializeObject<DisplayCustomerDto>(await httpResponse.Content.ReadAsStringAsync());
            Assert.NotNull(responseModel);
        }

        [Fact]
        public async Task Invalid_Create_Customer_Request_Must_Fail()
        {
            //
            // Arrange
            //
            var request = new CreateCustomerRequest
            {
                FirstName = null,
                LastName = "b",
                DateOfBirth = new DateTime(1982, 1, 1)
            };
            //
            // Act
            //
            var httpResponse = await _client.PostAsJsonAsync(BaseUrl, request);
            //
            // Assert
            //
            Assert.False(httpResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async Task When_There_are_Customers_Which_Matches_The_Name_Filter_They_Will_Be_Returned()
        {
            //
            // Arrange
            //
            var johnSnow = new CreateCustomerRequest
            {
                FirstName = "john",
                LastName = "snow",
                DateOfBirth = new DateTime(1982, 1, 1)
            };
            var littleJohn = new CreateCustomerRequest
            {
                FirstName = "little",
                LastName = "john",
                DateOfBirth = new DateTime(1982, 1, 1)
            };
            await _client.PostAsJsonAsync(BaseUrl, johnSnow);
            await _client.PostAsJsonAsync(BaseUrl, littleJohn);
            //
            // Act
            //
            var httpResponse = await _client.GetAsync($"{BaseUrl}?name=john");
            //
            // Assert
            //
            httpResponse.EnsureSuccessStatusCode();
            var customers = JsonConvert.DeserializeObject<List<DisplayCustomerDto>>(await httpResponse.Content.ReadAsStringAsync());
            Assert.NotNull(customers);
            Assert.True(customers.Count == 2);
        }

        [Fact]
        public async Task When_Invalid_Request_Is_Sent_To_Search_For_Customers_It_Must_Return_Failure()
        {
            //
            // Act
            //
            var httpResponse = await _client.GetAsync($"{BaseUrl}?name=");
            //
            // Assert
            //
            Assert.False(httpResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async Task When_There_Are_No_Customers_Found_It_Must_Return_As_NotFound()
        {
            //
            // Act
            //
            var httpResponse = await _client.GetAsync($"{BaseUrl}?name=john");
            //
            // Assert
            //
           Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }
    }
}
