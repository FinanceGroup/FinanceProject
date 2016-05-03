using Finance.Standlone.Webapp;
using System;
using System.ServiceProcess;

namespace Finance.Standlone
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                WebAppServiceHost.Instance.TryStartApplication(args);
                Console.ReadLine();
                WebAppServiceHost.Instance.StopApplication();
            }
            else
            {
                ServiceBase.Run(WebAppServiceHost.Instance);
            }
        }
    }
}
