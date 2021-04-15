using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.Satisfaction.Queries.GetSatisfactionList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SatisfactionController : ApiController
    {
        //api/Priority
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<GetSatisfactionListDTO>>> Get()
        {
            return await Mediator.Send(new GetSatisfactionListQuery());
        }
    }
}