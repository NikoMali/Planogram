using Microsoft.AspNetCore.Mvc;
using Planograma.EmplUser.Application.Employees.Commands.CreateEmployee;
using Planograma.EmplUser.Application.Employees.Commands.DeleteEmployee;
using Planograma.EmplUser.Application.Employees.Commands.UpdateEmployee;
using Planograma.EmplUser.Application.Employees.Queries.GetEmployee;
using Planograma.EmplUser.Application.Employees.Queries.GetEmployees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planograma.EmplUser.API.Controllers
{
    public class EmployeeController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetEmployee(GetEmployeesQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetEmployeeDetail(int id)
        {
            return Ok(await Mediator.Send(new GetEmployeeQuery { Id = id }));
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateEmployeeCommond command)
        {
            return await Mediator.Send(command);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteEmployeeCommond { Id = id });

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateEmployeeCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }
    }
}
