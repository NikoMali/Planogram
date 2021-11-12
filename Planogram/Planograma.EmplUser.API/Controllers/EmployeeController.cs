using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planograma.Authorization.Application.Models.Users;
using Planograma.Authorization.Application.Services;
using Planograma.EmplUser.Application.Employees.Commands.CreateEmployee;
using Planograma.EmplUser.Application.Employees.Commands.DeleteEmployee;
using Planograma.EmplUser.Application.Employees.Commands.UnblockEmployee;
using Planograma.EmplUser.Application.Employees.Commands.UpdateEmployee;
using Planograma.EmplUser.Application.Employees.Queries.GetEmployee;
using Planograma.EmplUser.Application.Employees.Queries.GetEmployees;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Planograma.EmplUser.API.Controllers
{
    //[Authorize]
    [Authorize(Policy = "RoleWithPermissions")]
    public class EmployeeController : ApiControllerBase
    {
        private IUserService _userService;

        public EmployeeController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response =await _userService.Authenticate(model, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("unblock")]
        public async Task<IActionResult> UnblockEmployee(UnblockEmployeeCommond command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployee([FromQuery] GetEmployeesQuery query)
        {
            Log.Error("Test Error");
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetEmployeeDetail(int id)
        {
            return Ok(await Mediator.Send(new GetEmployeeQuery { Id = id }));
        }
        [AllowAnonymous]
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










        // helper methods

        private void setTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
