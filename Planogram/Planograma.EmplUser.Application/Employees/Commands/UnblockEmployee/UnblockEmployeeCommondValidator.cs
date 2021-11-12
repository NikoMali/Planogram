using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Application.Employees.Commands.UnblockEmployee
{
    class UnblockEmployeeCommondValidator : AbstractValidator<UnblockEmployeeCommond>
    {
        public UnblockEmployeeCommondValidator()
        {
            RuleFor(v => v.EmployeeId)
                .NotEmpty();

        }
    }
}
