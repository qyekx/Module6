using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Models;

namespace EmployeeManagement.Services
{
    public class FirebaseService : ViewModels.EmployeeViewModel
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseService()
        {
            _firebaseClient = new FirebaseClient("https://employeemanagementapp-c475a-default-rtdb.firebaseio.com/");
        }

        // Добавление сотрудника
        public async Task AddEmployee(Employee employee)
        {
            await _firebaseClient
                .Child("Employees")
                .PostAsync(employee);
        }

        // Получение всех сотрудников
        public async Task<List<Employee>> GetEmployees()
        {
            var employees = await _firebaseClient
                .Child("Employees")
                .OnceAsync<Employee>();

            List<Employee> employeeList = new List<Employee>();

            foreach (var emp in employees)
            {
                employeeList.Add(new Employee
                {
                    Id = emp.Key,
                    FirstName = emp.Object.FirstName,
                    LastName = emp.Object.LastName,
                    Position = emp.Object.Position,
                    Salary = emp.Object.Salary
                });
            }

            return employeeList;
        }

        // Обновление сотрудника
        public async Task UpdateEmployee(string id, Employee employee)
        {
            await _firebaseClient
                .Child("Employees")
                .Child(id)
                .PutAsync(employee);
        }

        // Удаление сотрудника
        public async Task DeleteEmployee(string id)
        {
            await _firebaseClient
                .Child("Employees")
                .Child(id)
                .DeleteAsync();
        }
    }
}
