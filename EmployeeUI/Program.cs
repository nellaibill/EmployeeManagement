using AutoMapper;
using EmployeeService.DTO;
using EmployeeService.Models;
using EmployeeService.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.AccessControl;

namespace EmployeeUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDTO>();
            });

            var mapper = new Mapper(mapperConfiguration);


            var serviceProvider = new ServiceCollection()
                 .AddSingleton<IConfiguration>(configuration)
                 .AddSingleton<IMapper>(mapper)
                .AddLogging(builder => builder.AddConsole())
                .AddTransient<EmployeeRepository>()  
                .BuildServiceProvider();

            var employeeRepository = serviceProvider.GetRequiredService<EmployeeRepository>();
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(employeeRepository));

        }
    }
}