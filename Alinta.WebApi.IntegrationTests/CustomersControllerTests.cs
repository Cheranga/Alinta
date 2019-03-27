using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Alinta.WebApi.DTO.Requests;
using Alinta.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Alinta.WebApi.IntegrationTests
{
    public class CustomersControllerTests: IClassFixture<WebApplicationFactory<Startup>>, IDisposable
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
        public async Task When_There_Are_Customers_Which_Matches_The_Name_Filter_They_Will_Be_Returned()
        {
            //
            // Arrange
            //
            var commonName = Guid.NewGuid().ToString();
            var johnSnow = new CreateCustomerRequest
            {
                FirstName = commonName,
                LastName = "snow",
                DateOfBirth = new DateTime(1982, 1, 1)
            };
            var littleJohn = new CreateCustomerRequest
            {
                FirstName = "little",
                LastName = commonName,
                DateOfBirth = new DateTime(1982, 1, 1)
            };
            await _client.PostAsJsonAsync(BaseUrl, johnSnow);
            await _client.PostAsJsonAsync(BaseUrl, littleJohn);
            //
            // Act
            //
            var httpResponse = await _client.GetAsync($"{BaseUrl}?name={commonName}");
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
            var httpResponse = await _client.GetAsync($"{BaseUrl}?name={Guid.NewGuid().ToString()}");
            //
            // Assert
            //
           Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Fact]
        public async Task Invalid_Edit_Customer_Request_Must_Fail()
        {
            //
            // Arrange
            //
            var request = new UpdateCustomerRequest
            {
                Id = "",
                FirstName = ""
            };
            //
            // Act
            //
            var httpResponse = await _client.PutAsync("", null);
            //
            // Assert
            //
            Assert.False(httpResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Customers_Who_Are_Not_Existing_Cannot_Be_Updated()
        {
            //
            // Arrange
            //
            var request = new UpdateCustomerRequest
            {
                Id = "",
                FirstName = ""
            };
            //
            // Act
            //
            var httpResponse = await _client.PutAsync("", null);
            //
            // Assert
            //
            Assert.False(httpResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async Task With_Valid_Update_Customer_Request_Existing_Customer_Can_Be_Updated()
        {
            //
            // Arrange
            //
            var johnSnow = new CreateCustomerRequest
            {
                FirstName = "john",
                LastName = "snow",
                DateOfBirth = new DateTime(2000, 1, 1)
            };
            var httpResponse = await _client.PostAsJsonAsync(@"/api/v1/customers", johnSnow);
            var createdCustomerDto = JsonConvert.DeserializeObject<DisplayCustomerDto>(await httpResponse.Content.ReadAsStringAsync());
            var id = createdCustomerDto.Id;

            var updateRequest = new UpdateCustomerRequest
            {
                Id = id,
                FirstName = "John",
                LastName = "Connor",
                DateOfBirth = new DateTime(1990, 1, 1)
            };  
            var httpRequest = new HttpRequestMessage(HttpMethod.Put, BaseUrl)
            {
                Content = new StringContent(JsonConvert.SerializeObject(updateRequest), Encoding.UTF8, "application/json")
            };
            //
            // Act
            //
            httpResponse = await _client.SendAsync(httpRequest);
            //
            // Assert
            //
            httpResponse.EnsureSuccessStatusCode();
            var updatedCustomerDto = JsonConvert.DeserializeObject<DisplayCustomerDto>(await httpResponse.Content.ReadAsStringAsync());

            Assert.NotNull(updatedCustomerDto);
            Assert.Equal(id, updatedCustomerDto.Id);
            Assert.Equal("John Connor", updatedCustomerDto.Name);
        }

        [Fact]
        public async Task Invalid_Delete_Customer_Request_Must_Fail()
        {
            //
            // Arrange
            //
            var request = new DeleteCustomerRequest
            {
                CustomerId = ""
            };
            //
            // Act
            //
            var httpResponse = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, BaseUrl)
            {
                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
            });
            //
            // Assert
            //
            Assert.False(httpResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Customers_Who_Are_Not_Existing_Cannot_Be_Deleted()
        {
            //
            // Arrange
            //
            var request = new DeleteCustomerRequest
            {
                CustomerId = Guid.NewGuid().ToString()
            };
            //
            // Act
            //
            var httpResponse = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, BaseUrl)
            {
                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
            });
            //
            // Assert
            //
            Assert.False(httpResponse.IsSuccessStatusCode);
        }

        [Fact]
        public async Task With_Valid_Request_Existing_Customer_Can_Be_Deleted()
        {
            //
            // Arrange
            //
            var httpResponse = await _client.PostAsJsonAsync(BaseUrl, new CreateCustomerRequest
            {
                FirstName = "john",
                LastName = "snow",
                DateOfBirth = new DateTime(2000, 1, 1)
            });
            var createdCustomerDto = JsonConvert.DeserializeObject<DisplayCustomerDto>(await httpResponse.Content.ReadAsStringAsync());
            var deleteRequest = new DeleteCustomerRequest
            {
                CustomerId = createdCustomerDto.Id
            };
            //
            // Act
            //
            httpResponse = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, BaseUrl)
            {
                Content = new StringContent(JsonConvert.SerializeObject(deleteRequest), Encoding.UTF8, "application/json")
            });
            //
            // Assert
            //
            httpResponse.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
