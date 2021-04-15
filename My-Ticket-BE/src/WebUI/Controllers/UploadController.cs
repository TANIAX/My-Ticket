using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.Upload;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<object>> Upload()
        {
            var file = Request.Form.Files[0];
            return await Mediator.Send(new UploadCommand(file.FileName, file.OpenReadStream()));
        }
    }
}