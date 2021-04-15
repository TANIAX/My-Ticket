using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.Satisfaction.Commands;
using CleanArchitecture.Application.Ticket.Commands.Close;
using CleanArchitecture.Application.Ticket.Commands.CreateCustomer;
using CleanArchitecture.Application.Ticket.Commands.CreateReplyTicketByMail;
using CleanArchitecture.Application.Ticket.Commands.CreateStaff;
using CleanArchitecture.Application.Ticket.Commands.Delete;
using CleanArchitecture.Application.Ticket.Commands.Reply;
using CleanArchitecture.Application.Ticket.Commands.Update;
using CleanArchitecture.Application.Ticket.Queries.Create;
using CleanArchitecture.Application.Ticket.Queries.GetTicket;
using CleanArchitecture.Application.Ticket.Queries.GetTicketList;
using CleanArchitecture.Application.Ticket.Queries.Wearable.GetTicketTitleList;
using CleanArchitecture.Application.Ticket.Queries.Wearable.GetUnreadAndOpenNumber;
using CleanArchitecture.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CleanArchitecture.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ApiController
    {
        [Authorize(Roles = "Admin,Member")]
        [HttpGet]
        [Route("Create")]
        public async Task<ActionResult<CreateTicketDTO>> Create()
        {
            return await Mediator.Send(new CreateTicketQuery());
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        [Route("CreateCustomer")]
        public async Task<ActionResult<int>> CreateCustomer(CreateCustomerTicketCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Roles = "Admin,Member")]
        [HttpPost]
        [Route("CreateStaff")]
        public async Task<ActionResult<int>> CreateStaff(CreateStaffTicketCommand command)
        {
            return await Mediator.Send(command);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("CreateReplyTicketByMail")]
        public async Task<ActionResult<int>> CreateReplyTicketByMailCommand()
        {
            return await Mediator.Send(new CreateReplyTicketByMailCommand());
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetTicketDTO>> Get(int id)
        {
            return await Mediator.Send(new GetTicketQuery(id));
        }

        [Authorize(Roles = "Admin,Member,Customer")]
        [HttpPost]
        [Route("List")]
        public async Task<ActionResult<List<TicketListDTO>>> List(GetTicketListQuery query)
        {
            return await Mediator.Send(query);
        }

        [Authorize]
        [HttpPost]
        [Route("Reply")]
        public async Task<ActionResult<int>> Reply(ReplyTicketCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize]
        [HttpPost]
        [Route("Close")]
        public async Task<ActionResult<int>> Close(CloseTicketsCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Roles ="Member,Admin")]
        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult<int>> Update(UpdateTicketCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost]
        [Route("Image")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Image()
        {
            var file = Request.Form.Files[0];
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult<Unit>> Delete(DeleteTicketsCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Roles ="Customer")]
        [HttpPost]
        [Route("SetSatisfaction")]
        public async Task<ActionResult<int>> SetSatisfaction(SetSatisfactionCommand command)
        {
            return await Mediator.Send(command);
        }
        [Authorize]
        [HttpPost]
        [Route("Wearable")]
        public async Task<ActionResult<UnreadAndOpenDTO>> GetUnreadAndOpen(UnreadAndOpenQuery query)
        {
            return await Mediator.Send(query);
        }
        [Authorize]
        [HttpPost]
        [Route("WearableTicketTitleList")]
        public async Task<ActionResult<List<TicketTitleListDTO>>> GetUnreadAndOpen(TicketTitleListQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}

