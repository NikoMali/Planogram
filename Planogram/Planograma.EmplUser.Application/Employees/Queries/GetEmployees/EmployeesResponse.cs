using Planograma.EmplUser.Application.Mappings;
using Planograma.EmplUser.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Application.Employees.Queries.GetEmployees
{
    public class EmployeesResponse : IMapFrom<Employee>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string MobileNumber { get; set; }
        //public string Username { get; set; }
        public bool Done { get; set; }
    }
}
