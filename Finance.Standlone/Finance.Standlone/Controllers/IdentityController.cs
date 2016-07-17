using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace Finance.Standlone.Controllers
{
    //[Authorize]
    public class IdentityController : ApiController
    {
        public string Get()
        {
            //var manager = new LdapManager();
            //IAppUserManager userManager = new AppUserManager(manager);
            //AppUser appUser = userManager.Extract(RequestContext);
            //return JsonConvert.SerializeObject(appUser);

            return string.Empty;
        }

        public string Post()
        {
            Dictionary<string, bool> result = new Dictionary<string, bool>();

            result.Add(RoleConstant.ROLE_CURRENCY, true);
            //result.Add(RoleConstant.ROLE_PAYUPGRID_WEB, User.IsInRole(RoleConstant.ROLE_PAYUPGRID));
            //result.Add(RoleConstant.ROLE_TOP10_WEB, User.IsInRole(RoleConstant.ROLE_TOP10));

            return JsonConvert.SerializeObject(result);
        }
    }
}
