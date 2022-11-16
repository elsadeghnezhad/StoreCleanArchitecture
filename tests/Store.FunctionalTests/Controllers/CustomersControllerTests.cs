using Bogus.DataSets;
using Newtonsoft.Json;
using Store.ApplicationCore.DTOs;
using Store.ApplicationCore.Entities;
using Store.ApplicationCore.Utils;
using Store.FunctionalTests.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Store.FunctionalTests.Controllers
{
    public class CustomersControllerTests : BaseControllerTests
    {
        public CustomersControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetCustomers_ReturnsAllRecords()
        {
            var client = this.GetNewClient();
            var response = await client.GetAsync("/api/Customers");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<CustomerResponse>>(stringResponse).ToList();
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.True(result.Count == 10);
        }

        [Fact]
        public async Task GetCustomerById_ProductExists_ReturnsCorrectCustomer()
        {
            var customerId = 5;
            var client = this.GetNewClient();
            var response = await client.GetAsync($"/api/Customers/{customerId}");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SingleCustomerResponse>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.Equal(customerId.ToString(), result.Id);
            Assert.NotNull(result.Firstname);
          
        }

        [Theory]
        [InlineData(0)]
        [InlineData(20)]
        public async Task GetProductById_CustomerDoesntExist_ReturnsNotFound(int customerId)
        {
            var client = this.GetNewClient();
            var response = await client.GetAsync($"/api/Customers/{customerId}");

            var statusCode = response.StatusCode.ToString();

            Assert.Equal("NotFound", statusCode);
        }

        [Fact]
        public async Task PostProduct_ReturnsCreatedProduct()
        {
            var client = this.GetNewClient();

            // Create product

            var request = new CreateCustomerRequest
            {
                Firstname = "firstname",
                Lastname = "lastname",
                DateOfBirth = DateUtil.GetCurrentDate(),
                PhoneNumber = 9383548030,
                Email =       "email@gmail.com",
                BankAccountNumber = "123456789"
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response1 = await client.PostAsync("/api/Customers", stringContent);
            response1.EnsureSuccessStatusCode();

            var stringResponse1 = await response1.Content.ReadAsStringAsync();
            var createdCustomer = JsonConvert.DeserializeObject<SingleCustomerResponse>(stringResponse1);
            var statusCode1 = response1.StatusCode.ToString();

            Assert.Equal("Created", statusCode1);

            // Get created customer

            var response2 = await client.GetAsync($"/api/Customers/{createdCustomer.Id}");
            response2.EnsureSuccessStatusCode();

            var stringResponse2 = await response2.Content.ReadAsStringAsync();
            var result2 = JsonConvert.DeserializeObject<SingleCustomerResponse>(stringResponse2);
            var statusCode2 = response2.StatusCode.ToString();

            Assert.Equal("OK", statusCode2);
            Assert.Equal(createdCustomer.Id, result2.Id);
            Assert.Equal(createdCustomer.Firstname, result2.Firstname);
            Assert.Equal(createdCustomer.Lastname, result2.Lastname);
            Assert.Equal(createdCustomer.Email, result2.Email);
            Assert.Equal(createdCustomer.BankAccountNumber, result2.BankAccountNumber);
            Assert.Equal(createdCustomer.DateOfBirth, result2.DateOfBirth);
        }

        [Fact]
        public async Task PostCustomer_InvalidData_ReturnsErrors()
        {
            var client = this.GetNewClient();

            // Create product

            var request = new CreateCustomerRequest
            {
                Firstname = "firstname",
                Lastname = null,
                DateOfBirth = DateUtil.GetCurrentDate(),
                PhoneNumber = 938548030,
                Email = "email@gmail.com",
                BankAccountNumber = "123456789"
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/Customers", stringContent);

            var stringResponse = await response.Content.ReadAsStringAsync();
            var badRequest = JsonConvert.DeserializeObject<BadRequestModel>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("BadRequest", statusCode);
            Assert.NotNull(badRequest.Title);
            Assert.NotNull(badRequest.Errors);
            Assert.Equal(2, badRequest.Errors.Count);
            Assert.Contains(badRequest.Errors.Keys, k => k == "Lastname");
        }


        [Fact]
        public async Task PutCustomer_ReturnsUpdatedProduct()
        {
            var client = this.GetNewClient();

            // Update customer

            var customerId = 6;
            var request = new UpdateCustomerRequest
            {
                Firstname = "Newfirstname",
                Lastname = "Newlastname",
                DateOfBirth = DateUtil.GetCurrentDate(),
                PhoneNumber = 9383548030,
                Email = "email@gmail.com",
                BankAccountNumber = "123456789"
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response1 = await client.PutAsync($"/api/Customers/{customerId}", stringContent);
            response1.EnsureSuccessStatusCode();

            var stringResponse1 = await response1.Content.ReadAsStringAsync();
            var updatedCustomer = JsonConvert.DeserializeObject<SingleCustomerResponse>(stringResponse1);
            var statusCode1 = response1.StatusCode.ToString();

            Assert.Equal("OK", statusCode1);

            // Get updated customer

            var response2 = await client.GetAsync($"/api/Customers/{updatedCustomer.Id}");
            response2.EnsureSuccessStatusCode();

            var stringResponse2 = await response2.Content.ReadAsStringAsync();
            var result2 = JsonConvert.DeserializeObject<SingleCustomerResponse>(stringResponse2);
            var statusCode2 = response2.StatusCode.ToString();

            Assert.Equal("OK", statusCode2);
            Assert.Equal(updatedCustomer.Id, result2.Id);
            Assert.Equal(updatedCustomer.Firstname, result2.Firstname);
            Assert.Equal(updatedCustomer.Lastname, result2.Lastname);
            Assert.Equal(updatedCustomer.Email, result2.Email);
        }

        [Fact]
        public async Task DeleteCustomerById_ReturnsNoContent()
        {
            var client = this.GetNewClient();
            var customerId = 5;

            // Delete customer

            var response1 = await client.DeleteAsync($"/api/Customers/{customerId}");

            var statusCode1 = response1.StatusCode.ToString();

            Assert.Equal("NoContent", statusCode1);

            // Get deleted product

            var response2 = await client.GetAsync($"/api/Customers/{customerId}");

            var statusCode2 = response2.StatusCode.ToString();

            Assert.Equal("NotFound", statusCode2);
        }
    }
}