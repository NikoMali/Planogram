using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Application.Employees.Commands.DeleteEmployee
{
    class DeleteEmployeeCommondValidator : AbstractValidator<DeleteEmployeeCommond>
    {
        public DeleteEmployeeCommondValidator()
        {
            RuleFor(v => v.Id).NotEmpty();
        }
    }
}
