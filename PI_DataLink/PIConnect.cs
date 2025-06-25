using OSIsoft.AF.PI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PI_DataLink
{
    public class PIConnect
    {
        public static PIServer pIServer(string piServerName)
        {
            PIServers piServers = new PIServers();
            PIServer piServer = piServers[piServerName];
            return piServer;
        }
    }
}
