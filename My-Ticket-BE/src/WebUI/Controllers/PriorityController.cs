using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.Priority.Queries.GetPriorityList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriorityController : ApiController
    {
        //api/Priority
        [Authorize(Roles = "Member,Admin")]
        [HttpGet]
        public async Task<ActionResult<List<GetPriorityListDTO>>> Get()
        {
            return await Mediator.Send(new GetPriorityListQuery());
        }
    }
}