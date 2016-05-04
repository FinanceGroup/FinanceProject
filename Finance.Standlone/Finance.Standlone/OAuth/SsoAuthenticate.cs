using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Framework.OAuth
{
    public class SsoAuthenticate : IAuthenticate
    {
        //private readonly IAuthenticationService _service = new AuthenticationService();
        public Response Authenticate(string userName, string password)
        {
            //AuthenticationResponse response = _service.Authenticate(userName,password);
            var result = new Response();
            result.IsValid = true;
            result.Output = "Hello, SSo OAuth";
            return result;
        }
    }
}
