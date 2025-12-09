using System.Net;
using System.Net.Http.Json;
using Application.DTOs.Employee;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Test_Express
{
    public class EmployeesControllerTests : IClassFixture<WebApplicationFactory<API.Controllers.EmployeesController>>
    {
        private readonly HttpClient _client;

        public EmployeesControllerTests(WebApplicationFactory<API.Controllers.EmployeesController> factory)
        {
            // Create an in-memory client for testing
            _client = factory.CreateClient();
        }

        [Fact(DisplayName = "GET /api/employees/{id} returns employee when exists")]
        public async Task GetById_ShouldReturnEmployee_WhenEmployeeExists()
        {
            // Arrange
            var testEmployeeId = 1; // Make sure this exists in your seeded DB

            // Act
            var response = await _client.GetAsync($"/api/employees/{testEmployeeId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var employee = await response.Content.ReadFromJsonAsync<EmployeeDto>();
            Assert.NotNull(employee);
            Assert.Equal(testEmployeeId, employee.Id);
        }

        [Fact(DisplayName = "PUT /api/employees/{id} returns 404 when employee does not exist")]
        public async Task Update_ShouldReturn404_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var invalidEmployeeId = 9999;
            var updateDto = new UpdateEmployeeDto
            {
                FirstName = "Test",
                LastName = "Test",
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/employees/{invalidEmployeeId}", updateDto);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
