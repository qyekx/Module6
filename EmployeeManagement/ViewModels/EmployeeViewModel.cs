using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using EmployeeManagement.Models;
using EmployeeManagement.Services;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeViewModel
    {
        private FirebaseService _firebaseService;
        public ObservableCollection<Employee> Employees { get; set; }

        public ICommand AddEmployeeCommand { get; set; }
        public ICommand EditEmployeeCommand { get; set; }
        public ICommand DeleteEmployeeCommand { get; set; }

        public EmployeeViewModel()
        {
            _firebaseService = new FirebaseService();
            Employees = new ObservableCollection<Employee>();
            LoadEmployees();

            // Определение команд
            AddEmployeeCommand = new RelayCommand(async () => await AddEmployee());
            EditEmployeeCommand = new RelayCommand<Employee>(async (emp) => await EditEmployee(emp));
            DeleteEmployeeCommand = new RelayCommand<Employee>(async (emp) => await DeleteEmployee(emp));
        }

        private async Task LoadEmployees()
        {
            var employees = await _firebaseService.GetEmployees();
            foreach (var employee in employees)
            {
                Employees.Add(employee);
            }
        }

        private async Task AddEmployee()
        {
            var newEmployee = new Employee
            {
                FirstName = "New",
                LastName = "Employee",
                Position = "Position",
                Salary = 1000
            };

            await _firebaseService.AddEmployee(newEmployee);
            Employees.Add(newEmployee);
        }

        private async Task EditEmployee(Employee employee)
        {
            employee.FirstName = "Updated";
            await _firebaseService.UpdateEmployee(employee.Id, employee);
        }

        private async Task DeleteEmployee(Employee employee)
        {
            await _firebaseService.DeleteEmployee(employee.Id);
            Employees.Remove(employee);
        }

        public class RelayCommand : ICommand
        {
            private readonly Action _execute;
            private readonly Func<bool> _canExecute;

            public RelayCommand(Action execute, Func<bool> canExecute = null)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

            public void Execute(object parameter) => _execute();

            public event EventHandler CanExecuteChanged;

            public void RaiseCanExecuteChanged()
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public class RelayCommand<T> : ICommand
        {
            private readonly Action<T> _execute;
            private readonly Func<T, bool> _canExecute;

            public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter) => _canExecute == null || _canExecute((T)parameter);

            public void Execute(object parameter) => _execute((T)parameter);

            public event EventHandler CanExecuteChanged;

            public void RaiseCanExecuteChanged()
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

