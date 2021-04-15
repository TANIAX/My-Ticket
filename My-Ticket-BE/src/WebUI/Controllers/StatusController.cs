using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.Status.Queries.GetStatusList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ApiController
    {
        //api/Status
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<GetStatusListDTO>>> Get()
        {
            return await Mediator.Send(new GetStatusListQuery());
        }
    }
}