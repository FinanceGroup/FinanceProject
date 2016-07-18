using Finance.DAL.DAOs;
using Finance.Framework.Hubs;
using Finance.Standlone.Hubs;

namespace Finance.Standlone.Controllers
{
    public class CurrencyController : ApiControllerWithHub<CurrencyGridHub>
    {
        private readonly IUserDAO _userDAO = null;
        public CurrencyController(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public string Get()
        {
            _userDAO.GetRecord("admin", "admin");

            return "success";
        }
    }
}
