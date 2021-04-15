using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.Project.Queries.GetProjectList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ApiController
    {
        //api/Project
        [Authorize(Roles = "Member,Admin")]
        [HttpGet]
        public async Task<ActionResult<List<GetProjectListDTO>>> Get()
        {
            return await Mediator.Send(new GetProjectListQuery());
        }
    }
}