using Planograma.EmplUser.Application.Mappings;
using Planograma.EmplUser.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Application.Employees.Queries.GetEmployee
{
    public class EmployeeResponse : IMapFrom<Employee>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public bool Done { get; set; }
    }
}
