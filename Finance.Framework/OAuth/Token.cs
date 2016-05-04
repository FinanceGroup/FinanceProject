using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Finance.Framework.OAuth
{
    public class Token
    {
        private readonly JObject _jObject;
        public Token(string jsonToken)
        {
            _jObject = JObject.Parse(jsonToken);
        }

        public string AccessToken
        {
            get { return _jObject["AccessToken"].ToString(); }
        }

        public string TokenType
        {
            get { return _jObject["token_type"].ToString(); }
        }

        public string ExpiresIn
        {
            get { return _jObject["expires_in"].ToString(); }
        }
    }
}
