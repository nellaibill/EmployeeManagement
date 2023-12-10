using AutoMapper;
using EmployeeService.DTO;
using EmployeeService.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;

namespace EmployeeService.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly ILogger<EmployeeRepository> logger;

        public EmployeeRepository(IConfiguration configuration, IMapper mapper,
            ILogger<EmployeeRepository> logger)
        {
            this.configuration = configuration;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<List<EmployeeDTO>> GetAllEmployees()
        {
            var employees = new List<Employee>();

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
                    catch (Exception ex)
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

        [HttpPut]
        public async Task<EmployeeDTO> UpdateEmployee(UpdateRequestDTO updateRequestDTO)
        {
            var employees = new EmployeeDTO();
            using (var client = new HttpClient())
            {
                // GetDefaultHeaders(client);

                var URL = configuration["GoRest:URL"];
                //var Token = configuration["GoRest:Token"];


                client.BaseAddress = new Uri(URL);

                using (HttpResponseMessage httpResponseMessage = await client.PatchAsync($"users/{updateRequestDTO.Id}", updateRequestDTO))
                {
                  
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "73ed4a9a3c3b7cbe767fc4f00ea7549d4842b62346aa7a0062cb7790fd04bc1b");
                    try
                    {
                        var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                        employees = JsonConvert.DeserializeObject<EmployeeDTO>(responseContent);
                    }
                    //var response = await client.PutAsJsonAsync($"users/{updateRequestDTO.Id}", updateRequestDTO).Result;
                    //employees = JsonConvert.DeserializeObject<EmployeeDTO>(response.Content.ToString());
                    /* if (response.IsSuccessStatusCode)
                     {
                         Console.Write("Success");
                     }
                     else
                         Console.Write("Error");
                 }*/
                    catch (Exception ex)
                    {
                        return null;
                    }
                }

            }
            return employees;
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
