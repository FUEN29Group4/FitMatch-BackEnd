using FitMatch_BackEnd.Models;
using Newtonsoft.Json.Linq;

namespace FitMatch_BackEnd.Controllers
{
    public class CEmployeeWrap
    {
        private Employee _employee;
        public Employee product
        {
            get { return _employee; }
            set { _employee = value; }
        }
        public CEmployeeWrap()
        {
            _employee = new Employee();
        }
        public int EmployeeID
        {
            get { return _employee.EmployeeId; }
            set { _employee.EmployeeId = value; }
        }

        public string? EmployeeName
        {
            get { return _employee.EmployeeName; }
            set { _employee.EmployeeName = value; }
        }

        public bool? Gender
        {
            get { return _employee.Gender; }
            set { _employee.Gender = value; }
        }

        public DateTime? Birth
        {
            get { return _employee.Birth; }
            set { _employee.Birth = value; }
        }

        public string? Phone
        {
            get { return _employee.Phone; }
            set { _employee.Phone = value; }
        }

        public string? Address
        {
            get { return _employee.Address; }
            set { _employee.Address = value; }
        }

        public string? Email
        {
            get { return _employee.Email; }
            set { _employee.Email = value; }
        }

        public string? Position
        {
            get { return _employee.Position; }
            set { _employee.Position = value; }
        }

        public string? Password
        {
            get { return _employee.Password; }
            set { _employee.Password = value; }
        }

        public DateTime? CreatedAt
        {
            get { return _employee.CreatedAt; }
            set { _employee.CreatedAt = value; }
        }
        public bool? Status
        {
            get { return _employee.Status; }
            set { _employee.Status = value; }
        }
        public string? Photo
        {
            get { return _employee.Photo; }
            set { _employee.Photo = value; }
        }
        public IFormFile photo { get; set; }
    }
}
