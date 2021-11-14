using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planograma.Authorization.Application.Models.Users;
using Planograma.Authorization.Application.Services;
using Planograma.EmplUser.API.Helpers;
using Planograma.EmplUser.Application.Employees.Commands.CreateEmployee;
using Planograma.EmplUser.Application.Employees.Commands.DeleteEmployee;
using Planograma.EmplUser.Application.Employees.Commands.UnblockEmployee;
using Planograma.EmplUser.Application.Employees.Commands.UpdateEmployee;
using Planograma.EmplUser.Application.Employees.Queries.GetEmployee;
using Planograma.EmplUser.Application.Employees.Queries.GetEmployees;
using Planograma.EmplUser.Application.Models.Users;
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
            return Ok(new GenericResponseWithData<AuthenticateResponse>(response,true));
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("unblock")]
        public async Task<IActionResult> UnblockEmployee(UnblockEmployeeCommond command)
        {
            return Ok(
                new GenericResponseWithData<UnblockEmployeeCommond>(
                await Mediator.Send(command),true));
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployee([FromQuery] GetEmployeesQuery query)
        {
            
            return Ok(
                new GenericResponseWithDataList<EmployeesResponse>(
                await Mediator.Send(query)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetEmployeeDetail(int id)
        {
            return Ok(
                new GenericResponseWithData<EmployeeResponse>(
                await Mediator.Send(new GetEmployeeQuery { Id = id })));
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Create(CreateEmployeeCommond command)
        {
            return Ok(
                new GenericResponseWithData<CreateEmployeeCommond>(
                await Mediator.Send(command)));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteEmployeeCommond { Id = id });

            return Ok(new GenericResponse(true));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateEmployeeCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            

            return Ok(
                new GenericResponseWithData<UpdateEmployeeCommand>
                (await Mediator.Send(command))
                );
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
