using Finance.DAL.DAOs;

namespace Finance.Framework.OAuth
{
    public class SsoAuthenticate : IAuthenticate
    {
        private readonly IUserDAO _userDAO = null;
        public SsoAuthenticate(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        //private readonly IAuthenticationService _service = new AuthenticationService();
        public Response Authenticate(string userName, string password)
        {
            var result = new Response();
            var record = _userDAO.GetRecord(userName, password);

            if (record != null)
            {
                result.IsValid = true;
                result.Output = "Hello, SSo OAuth";
            }
            else
            {
                result.IsValid = false;
                result.Output = "SSo OAuth falied.";
            }

            return result;
        }
    }
}
