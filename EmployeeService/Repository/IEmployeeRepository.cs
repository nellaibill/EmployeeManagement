using EmployeeService.DTO;

namespace EmployeeService.Repository
{
    public interface IEmployeeRepository
    {
       public  Task<List<EmployeeDTO>> GetAllEmployees();

    }
}
