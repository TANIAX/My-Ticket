using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.User.Commands.SignUp;
using CleanArchitecture.Application.User.Commands.User.ForgotPassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.Application.User.Commands.ResetPassword;
using CleanArchitecture.Application.User.Commands.Captcha;
using System.Net.Http;
using CleanArchitecture.Application.User.Queries.MyAccount;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Application.User.Commands.MyAccount;
using CleanArchitecture.Application.User.Queries.MyAccount.EditMyProfile;
using CleanArchitecture.Application.User.Queries.EditMyProfile;
using CleanArchitecture.Application.User.Commands.EditMyProfile;
using CleanArchitecture.Application.User.Queries.Employee.List;
using CleanArchitecture.Application.User.Commands.Employee.Create;
using CleanArchitecture.Application.User.Commands.Employee.Update;
using CleanArchitecture.Application.User.Commands.Employee.Delete;
using CleanArchitecture.Application.User.Commands.Employee.ResetPassword;
using CleanArchitecture.Application.User.Queries.GetMemberList;
using CleanArchitecture.Application.User.Queries.GetCurrentUser;
using CleanArchitecture.Application.User.Commands.Customer.Create;
using CleanArchitecture.Application.User.Commands.Customer.Edit;
using CleanArchitecture.Application.User.Queries.Customer.List;

namespace CleanArchitecture.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiController
    {
        //Post api/users/EmailExist
        [AllowAnonymous]
        [Route("EmailExist")]
        [HttpPost]
        public async Task<ActionResult<bool>> EmailExist(EmailExistCommand command)
        {
            return await Mediator.Send(command);
        }

        //Post api/users/EmailExist
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<bool>> Register(UserSignUpCommand command)
        {
            return await Mediator.Send(command);
        }
        //POST api/users/GenerateRandomCode
        [AllowAnonymous]
        [Route("GenerateRandomCode")]
        [HttpPost]
        public async Task<ActionResult<object>> GenerateRandomCode(GenerateRandomCodeCommand command)
        {
            return await Mediator.Send(command);
        }

        //POST api/users/ForgotPassword
        [AllowAnonymous]
        [Route("ForgotPassword")]
        [HttpPost]
        public async Task<ActionResult<bool>> ForgotPassword(ForgotPasswordCommand command)
        {
            return await Mediator.Send(command);
        }

        //Post api/users/ResetPassword
        [AllowAnonymous]
        [Route("ResetPassword")]
        [HttpPost]
        public async Task<ActionResult<bool>> ResetPassword(ResetPasswordCommand command)
        {
            return await Mediator.Send(command);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("CheckCaptcha")]
        public async Task<ActionResult<HttpResponseMessage>> CheckCaptcha(CaptchaCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        [Authorize]
        [Route("MyAccount")]
        public async Task<ActionResult<MyAccountDTO>> MyAccount()
        {
            return await Mediator.Send(new MyAccountQuery());
        }

        [Authorize]
        [HttpPost]
        [Route("MyAccount")]
        public async Task<int> UpdateMyAccount(MyAccountCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize]
        [HttpGet]
        [Route("EditMyProfile")]
        public async Task<ActionResult<EditMyProfileDTO>> EditMyProfile()
        {
           return await Mediator.Send(new EditMyProfileQuery());
        }

        [Authorize]
        [HttpPost]
        [Route("EditMyProfile")]
        public async Task<ActionResult<int>> EditMyProfile(EditMyProfileCommand command)
        {
            return await Mediator.Send(command);
        }

        // GET: api/users/Employees/List
        [Authorize(Roles = "Customer")]
        [Route("Employees/List")]
        [HttpGet]
        public async Task<ActionResult<MyEmployeesDTO>> GetMyEmployees()
        {
            return await Mediator.Send(new EmployeeListQuery());
        }

        // POST: api/users/Employees/Add
        [Authorize(Roles = "Customer")]
        [Route("Employees/Create")]
        [HttpPost]
        public async Task<ActionResult<object>> Create(CreateEmployeeCommand command)
        {
            return await Mediator.Send(command);
        }

        // POST: api/users/Employees/Edit
        [Authorize(Roles = "Customer")]
        [Route("Employees/Edit")]
        [HttpPost]
        public async Task<ActionResult<int>> Edit(UpdateEmployeeCommand command)
        {
            return await Mediator.Send(command);
        }
        // GET: api/users/GetMemberList
        [Authorize(Roles = "Member,Admin")]
        [Route("GetMemberList")]
        [HttpGet]
        public async Task<ActionResult<List<GetMemberListDTO>>> GetMemberList()
        {
            return await Mediator.Send(new GetMemberListQuery());
        }

        // POST: api/users/Employees/Delete
        [Authorize(Roles = "Customer")]
        [Route("Employees/Delete")]
        [HttpPost]
        public async Task<ActionResult<int>> Delete(DeleteEmployeeCommand command)
        {
            return await Mediator.Send(command);
        }

        // POST: api/users/Employees/ResetPassword
        [Authorize(Roles = "Customer")]
        [Route("Employees/ResetPassword")]
        [HttpPost]
        public async Task<ActionResult<int>> ResetPassword(ResetEmployeePasswordCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Roles = "Admin,Member")]
        [Route("Customer/Create")]
        [HttpPost]
        public async Task<ActionResult<object>> CreateCustomer(EditCustomerCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Roles = "Admin,Member")]
        [Route("Customer/Get")]
        [HttpPost]
        public async Task<ActionResult<GetCurrentUserDTO>> GetCustomer(GetCustomerQuery query)
        {
            return await Mediator.Send(query);
        }

        [Authorize(Roles = "Admin,Member")]
        [Route("Customer/Edit")]
        [HttpPost]
        public async Task<ActionResult<int>> EditCustomer(EditCustomerCommand command)
        {
            return await Mediator.Send(command);
        }
        [Authorize(Roles = "Admin,Member")]
        [Route("Customer/List")]
        [HttpGet]
        public async Task<ActionResult<object>> ListCustomer()
        {
            return await Mediator.Send(new ListCustomerQuery());
        }
        //GET: api/users/GetCurrentUser
        [Authorize]
        [Route("GetCurrentUser")]
        [HttpGet]
        public async Task<ActionResult<GetCurrentUserDTO>> GetCurrentUser()
        {
            return await Mediator.Send(new GetCustomerQuery());
        }
    }
}