using Microsoft.AspNetCore.Mvc;
using Planograma.EmplUser.Application.Employees.Commands.CreateEmployee;
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

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateEmployeeCommond command)
        {
            return await Mediator.Send(command);
        }
    }
}
