using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLib.Managers;

namespace PlugInLibraryB.Managers
{
    public class PlugInBHelloManager : IHelloManager
    {
        public string Say(string message)
        {
            return string.Format("PlugInB says {0}", message);
        }
    }
}
