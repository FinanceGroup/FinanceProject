using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Finance.Standlone.Hubs
{
    [HubName("PayUpGrid")]
    public class PayUpGridHub : Hub { }

    [HubName("GinniePayupGrid")]
    public class GinniePayupGridHub : Hub { }
}
