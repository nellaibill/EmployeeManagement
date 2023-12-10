using AutoMapper;
using EmployeeService.DTO;
using EmployeeService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EmployeeService.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly ILogger<EmployeeRepository> logger;

        public EmployeeRepository(IConfiguration configuration,IMapper mapper,
            ILogger<EmployeeRepository> logger)
        {
            this.configuration = configuration;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<List<EmployeeDTO>> GetAllEmployees()
        {
            var employees = new  List<Employee>();

            using (var client = new HttpClient())
            {
                GetDefaultHeaders(client);

                using (HttpResponseMessage httpResponseMessage = await client.GetAsync("users"))
                {
                    try
                    {
                        var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                        employees = JsonConvert.DeserializeObject<List<Employee>>(responseContent);
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex.ToString());
                        return null;
                    }
                }
            }
            return mapper.Map<List<EmployeeDTO>>(employees);
        }
        [HttpGet]
        
        public async Task<EmployeeDTO> GetEmployee(int id)
        {
            var employees = new Employee();
            using (var client = new HttpClient())
            {
                GetDefaultHeaders(client);

                using (HttpResponseMessage httpResponseMessage = await client.GetAsync($"users/{id}"))
                {
                    try
                    {
                        var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                        employees = JsonConvert.DeserializeObject<Employee>(responseContent);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.ToString());
                        return null;
                    }
                }
            }
            return mapper.Map<EmployeeDTO>(employees);
        }

        private void GetDefaultHeaders(HttpClient client)
        {
            var URL = configuration["GoRest:URL"];
            var Token = configuration["GoRest:Token"];
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Token);
        }
    }
}
