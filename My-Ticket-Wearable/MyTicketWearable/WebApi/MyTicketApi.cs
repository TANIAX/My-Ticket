using Nancy.Json;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace MyTicketWearable.WebApi
{
    public class MyTicketApi
    {
        public const string UnreadAndOpenEndPoint = "api/Ticket/Wearable";
        public const string LoginEndPoint = "api/Auth/Login";
        public const string TestTokenEndPoint = "api/Auth/TestToken";
        public const string TicketListEndPoint = "api/Ticket/WearableTicketTitleList";
        public const string TicketEndPoint = "api//";
        public string Token { get; set; }
        public string BaseUrl { get; set; }

        public MyTicketApi()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            GetToken();
        }
        private void GetToken()
        {
            bool lContinue = true;

            if (!Application.Current.Properties.ContainsKey("Email")
                || !Application.Current.Properties.ContainsKey("Password")
                    || !Application.Current.Properties.ContainsKey("ApiUrl"))
                lContinue = false;

            if (lContinue)
            {
                if (Application.Current.Properties.ContainsKey("Token"))
                {
                    //Make a request to test the validity of the token
                    var httpRequest = (HttpWebRequest)WebRequest.Create(Application.Current.Properties["ApiUrl"].ToString() + TestTokenEndPoint);

                    httpRequest.Accept = "application/json";
                    var x = Application.Current.Properties["Token"];
                    httpRequest.Headers.Add("Authorization", "Bearer " + Application.Current.Properties["Token"].ToString());

                    var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                    if(httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        this.Token = Application.Current.Properties["Token"].ToString();
                    }
                    else
                    {
                        this.Token = CreateToken();
                        if (this.Token != "")
                        {
                            Application.Current.Properties["Token"] = this.Token;
                            Application.Current.SavePropertiesAsync();
                        }
                    }
                }
                else
                {
                    //Create the token
                   this.Token = CreateToken();
                    if(this.Token != "")
                    {
                        Application.Current.Properties["Token"] = this.Token;
                        Application.Current.SavePropertiesAsync();
                    }
                }
            }        
        }

        private string CreateToken()
        {
            string returnValue = "";

            try
            {
                //Header of the request
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(Application.Current.Properties["ApiUrl"].ToString() + LoginEndPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                //Content of the request
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        UserName = Application.Current.Properties["Email"].ToString(),
                        Password = Application.Current.Properties["Password"].ToString()
                    });

                    streamWriter.Write(json);
                }
                //Post & response
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var token = JsonConvert.DeserializeObject<Token>(streamReader.ReadToEnd());
                    returnValue = token.UserToken;
                }
            }
            catch(Exception e)
            {

            }
            return returnValue;
        }

        public UnreadAndOpen GetUnreadAndOpen()
        {
            UnreadAndOpen unreadAndOpen = new UnreadAndOpen();

            try
            {
                var client = new RestClient(Application.Current.Properties["ApiUrl"] + UnreadAndOpenEndPoint);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Bearer "+ this.Token);
                request.AddHeader("Content-Type", "application/json");

                string json = new JavaScriptSerializer().Serialize(new
                {
                    Email = Application.Current.Properties["Email"].ToString()
                });
                request.AddParameter("application/json", json, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                    unreadAndOpen = JsonConvert.DeserializeObject<UnreadAndOpen>(response.Content);
            }
            catch (Exception e)
            {

            }
            return unreadAndOpen;
        }

        public List<TicketHeader> GetTicketList(int type)
        {
            List<TicketHeader> tl = new List<TicketHeader>();

            try
            {
                var client = new RestClient(Application.Current.Properties["ApiUrl"] + TicketListEndPoint);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Bearer " + this.Token);
                request.AddHeader("Content-Type", "application/json");

                string json = new JavaScriptSerializer().Serialize(new
                {
                    Type = type
                });
                request.AddParameter("application/json", json, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                    tl = JsonConvert.DeserializeObject<List<TicketHeader>>(response.Content);
            }
            catch (Exception e)
            {

            }
            return tl;
        }
    }
}
