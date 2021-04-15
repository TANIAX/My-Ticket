using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.SharedKernel.GlobalVar;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    public class CaptchaService : ICaptcha
    {
        HttpClient client;

        public Task<HttpResponseMessage> GetAsync(string token)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://www.google.com/recaptcha/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client.GetAsync("siteverify?secret=" + GlobalVar.PrivateGoogleCaptchaKey + "&response=" + token);
        }
    }
}
