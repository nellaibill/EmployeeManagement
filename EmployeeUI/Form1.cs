using EmployeeService.DTO;
using EmployeeService.Models;
using EmployeeService.Repository;

namespace EmployeeUI
{
    public partial class Form1 : Form
    {
        private readonly EmployeeRepository employeeRepository;
        int Id;
        public Form1(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private async void LoadEmployees()
        {
            var employees = await employeeRepository.GetAllEmployees();
            dataGridView1.DataSource = employees;
        }
        private async void LoadEmployeesById(int id)
        {
            var employees = await employeeRepository.GetEmployee(id);
            txtId.Text=Convert.ToString(employees.Id);
            txtEmail.Text = employees.Email;
            txtGender.Text = employees.Gender;
            txtName.Text = employees.Name;
            txtStatus.Text = employees.Status;
            LoadEmployees();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            LoadEmployeesById(Id);
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateRequestDTO employee = new UpdateRequestDTO()
            {
                Id =Convert.ToInt32(txtId.Text),
                Email = txtEmail.Text,
                Gender = txtGender.Text,
                Name = txtName.Text,
                Status = txtStatus.Text,
            };
            var employees = await employeeRepository.UpdateEmployee(employee);
            //dataGridView1.DataSource = employees;
        }
    }
}