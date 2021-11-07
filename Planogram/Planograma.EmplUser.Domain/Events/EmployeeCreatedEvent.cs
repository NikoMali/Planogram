using Planograma.EmplUser.Domain.Common;
using Planograma.EmplUser.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Domain.Events
{
    public class EmployeeCreatedEvent : DomainEvent
    {
        public EmployeeCreatedEvent(Employee employee)
        {
            Employee = employee;
        }

        public Employee Employee { get; }
    }
}
