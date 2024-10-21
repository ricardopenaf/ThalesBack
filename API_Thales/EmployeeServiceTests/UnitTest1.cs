using System.Threading.Tasks; 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccess;
using Entities;
using Moq;
using BusinessLayer;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;

namespace EmployeeServiceTests
{
    [TestClass]
    public class EmployeeServiceTests
    {
        private Mock<HttpMessageHandler>? _httpMessageHandlerMock;
        private HttpClient? _httpClient;
        private EmployeeService? _employeeService;

        [TestInitialize]
        public void Setup()
        {
            // Mockear el HttpMessageHandler
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://hub.dummyapis.com/employee")
            };

            _employeeService = new EmployeeService(_httpClient);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_ShouldReturnEmployee_WhenEmployeeExists()
        {
            // Arrange
            int employeeId = 1;
            var employees = new List<Employees>
            {
                new Employees { id = 1, salary = 5000 }
            };

            var responseContent = JsonConvert.SerializeObject(employees);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(request =>
                        request.Method == HttpMethod.Get &&
                        request.RequestUri.AbsolutePath == "/employee"), // Verifica que la ruta sea la correcta
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                });
            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(employeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employeeId, result.id);
            Assert.AreEqual(5000, result.salary);
        }

        [TestMethod]
        public async Task GetEmployeeByIdAsync_ShouldReturnNull_WhenEmployeeDoesNotExist()
        {
            // Arrange
            int employeeId = 2;
            var employees = new List<Employees>
            {
                new Employees { id = 1, salary = 5000 }
            };

            var responseContent = JsonConvert.SerializeObject(employees);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                });

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(employeeId);

            // Assert
            Assert.IsNull(result);
        }
    }
}