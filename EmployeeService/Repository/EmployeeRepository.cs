using EmployeeService.DTO;
using Newtonsoft.Json;

namespace EmployeeService.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IConfiguration configuration;

        public EmployeeRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<List<EmployeeDTO>> GetAllEmployees()
        {
            var employees = new  List<EmployeeDTO>();

            using (var client = new HttpClient())
            {
                var URL = configuration["GoRest:URL"];
                var Token = configuration["GoRest:Token"];
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(Token);

                using (HttpResponseMessage httpResponseMessage = await client.GetAsync("users"))
                {
                    try
                    {
                        var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                        employees = JsonConvert.DeserializeObject<List<EmployeeDTO>>(responseContent);

                    }
                    catch(Exception ex)
                    {
                        employees = null;
                    }
                }
            }
            return employees;
        }
    }
}
