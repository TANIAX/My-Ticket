using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.Type.Queries.GetTypeList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ApiController
    {
        //api/Type
        [Authorize(Roles = "Member,Admin")]
        [HttpGet]
        public async Task<ActionResult<List<GetTypeListDTO>>> Get()
        {
            return await Mediator.Send(new GetTypeListQuery());
        }
    }
}