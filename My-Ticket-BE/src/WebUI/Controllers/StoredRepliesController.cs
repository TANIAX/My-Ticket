using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.StoredReply.Commands.Create;
using CleanArchitecture.Application.StoredReply.Commands.Update;
using CleanArchitecture.Application.StoredReply.Commands.Delete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoredRepliesController : ApiController
    {
        //api/StoredReplies/Create
        [Authorize]
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<int>> Create(CreateStoredReplyCommand command)
        {
            return await Mediator.Send(command);
        }

        //api/StoredReplies/Edit
        [Authorize]
        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult<int>> Update(UpdateStoredReplyCommand command)
        {
            return await Mediator.Send(command);
        }

        //api/StoredReplies/Delete
        [Authorize]
        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult<int>> Delete(DeleteStoredReplyCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}