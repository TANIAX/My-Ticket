using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.Group.Commands.Create;
using CleanArchitecture.Application.Group.Commands.Delete;
using CleanArchitecture.Application.Group.Commands.Update;
using CleanArchitecture.Application.Group.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("List")]
        public async Task<ActionResult<List<GroupDTO>>> List()
        {
            return await Mediator.Send(new GroupListQuery());
        }

        [Authorize]
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<int>> Create(CreateGroupCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize]
        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult<int>> Update(UpdateGroupCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize]
        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult<int>> Delete(DeleteGroupCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}