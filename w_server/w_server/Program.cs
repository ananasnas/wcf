using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using WcfService_;


using MySql.Data.MySqlClient;
namespace w_server
{
    class Program
    {
        static void Main(string[] args)
        {          
            using (ServiceHost serviceHost =
                   new ServiceHost(typeof(WcfService_.OperationChanges)))
            {              
                serviceHost.Open();
                Console.WriteLine("The service is ready.");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.WriteLine();
                Console.ReadLine();
            }
        }
    }
}
