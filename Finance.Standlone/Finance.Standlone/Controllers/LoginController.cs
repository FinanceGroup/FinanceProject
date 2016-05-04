using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Finance.Framework.Configs;

namespace Finance.Standlone.Controllers
{
    public class LoginController : ApiController
    {
        private readonly IFinanceConfigManager _financeConfigManager;
        private const string _authUrl = "http://localhost:{0}/api/token";

        public LoginController(IFinanceConfigManager financeConfigManager)
        {
            _financeConfigManager = financeConfigManager;
        }

        //GET api/<controller>/5
        public string Get()
        {
            return "value";
        }

        public HttpResponseMessage Post([FromBody] object formData)
        {
            string param = JsonConvert.SerializeObject(formData);
            var client = new HttpClient();
            var myform = JsonConvert.DeserializeObject<Dictionary<string, string>>(param);
            myform["grant_type"] = "password";
            //myform["Username"] = param["Username"];
            //myform["Password"] = param["password"];
            var form = new FormUrlEncodedContent(myform);
            string url = string.Format(_authUrl, _financeConfigManager.Port);
            var response = client.PostAsync(url, form).Result;
            if (response.StatusCode.ToString().Equals("OK"))
            {
                response.EnsureSuccessStatusCode();
                string tokenResponse = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenResponse);
                string dd = tokenResponse.Substring(0, tokenResponse.Length - 1);
                //+ ",\"url\":\"" + _financeConfigManager.SiteUrl + "/views/index.html\"}";
                return Request.CreateResponse(HttpStatusCode.OK, dd);
            }
            return Request.CreateResponse(response.StatusCode, "Please check your Username/Password and try again");
        }

        //PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {

        }

        //DELETE api/<controller>/5
        public void Delete(int id)
        {

        }

    }
}
