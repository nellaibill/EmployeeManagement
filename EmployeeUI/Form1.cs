using EmployeeService.Repository;

namespace EmployeeUI
{
    public partial class Form1 : Form
    {
        private readonly EmployeeRepository employeeRepository;
        public Form1(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
            InitializeComponent();
        }

        private  void Form1_Load(object sender, EventArgs e)
        {
           LoadEmployees();
        }

        private async void LoadEmployees()
        {
            var employees = await employeeRepository.GetAllEmployees();
            dataGridView1.DataSource = employees;

        }
    }
}