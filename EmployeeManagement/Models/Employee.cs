namespace EmployeeManagement.Models
{
    public class Employee : ViewModels.EmployeeViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public double Salary { get; set; }
    }
}
