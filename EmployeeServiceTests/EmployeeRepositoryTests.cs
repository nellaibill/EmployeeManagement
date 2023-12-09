﻿using AutoMapper;
using EmployeeService.Mappings;
using EmployeeService.Models;
using EmployeeService.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace EmployeeServiceTests
{
    [TestClass]
    public class EmployeeRepositoryTests
    {
        private IMapper _mockMapper;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<ILogger<EmployeeRepository>> _mockLogger;
        private Mock<HttpClient> _mockHttpClient;

        [TestInitialize]
        public void InitializeTest()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<EmployeeRepository>>();
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            _mockMapper = mockMapper.CreateMapper();
            _mockHttpClient = new Mock<HttpClient>();
        }

        [TestMethod]
        public void GetAllEmployees_ResultShould_NotBeNull()
        {
            _mockConfiguration.Setup(c => c["GoRest:URL"]).Returns("http:example.com");
            _mockConfiguration.Setup(c => c["GoRest:Token"]).Returns("xyz");
            var employeeRepository = new EmployeeRepository(_mockConfiguration.Object, _mockMapper, _mockLogger.Object);
            List<Employee> employees = new List<Employee>();
            Employee employee = new Employee()
            {
                Email = "abc@gmail.com",
                Gender = "Male",
                Id = 1,
                Name = "Test",
                Status = "Test Status"
            };
            employees.Add(employee);
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonSerializer.Serialize(employee)),
                StatusCode = System.Net.HttpStatusCode.OK
            };
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                 "SendAsync",
                 ItExpr.IsAny<HttpRequestMessage>(),
                 ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);
            var result = employeeRepository.GetAllEmployees();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetAllEmployees_HandlesExceptionAndReturnsNull()
        {
            _mockConfiguration.Setup(c => c["GoRest:URL"]).Returns("https://www.example.com/");
            _mockConfiguration.Setup(c => c["GoRest:Token"]).Returns("xyz");
            var employeeRepository = new EmployeeRepository(_mockConfiguration.Object, _mockMapper, _mockLogger.Object);


            var handlerMock = new Mock<HttpMessageHandler>();
            ;
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                 "SendAsync",
                 ItExpr.IsAny<HttpRequestMessage>(),
                 ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception("Simulated exception"));


            // Act
            var result = await employeeRepository.GetAllEmployees();

            // Assert
            Assert.IsNull(result);
        }
    }
}
