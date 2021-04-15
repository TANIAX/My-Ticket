using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Interfaces
{
    public interface ICaptcha
    {
        Task<HttpResponseMessage> GetAsync(string token);
    }
}
