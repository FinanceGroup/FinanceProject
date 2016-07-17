using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Finance.Framework.Hubs
{
    public abstract class ApiControllerWithHub<THub> : ApiController
    where THub : IHub
    {
        Lazy<IHubContext> _hub = new Lazy<IHubContext>(
            () => GlobalHost.ConnectionManager.GetHubContext<IHub>()
        );

        protected IHubContext Hub
        {
            get { return _hub.Value; }
        }
    }
}
