using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Application.Employees.Commands.UpdateEmployee
{
    class UpdateEmployeeCommondValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommondValidator()
        {
            RuleFor(v => v.FirstName)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(v => v.LastName)
                .MaximumLength(200)
                .NotEmpty();

            

            RuleFor(v => v.password)
                .MaximumLength(200)
                .NotEmpty();
        }
    }
}
