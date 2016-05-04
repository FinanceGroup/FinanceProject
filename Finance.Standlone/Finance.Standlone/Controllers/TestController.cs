using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Finance.DAL.DAOs;

namespace Finance.Standlone.Controllers
{
    public class TestController : ApiController
    {
        private readonly IUserDAO _userDAO = null;
        public TestController(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public string Get()
        {
            _userDAO.GetRecord("admin","admin");

            return "success";
        }
    }
}
