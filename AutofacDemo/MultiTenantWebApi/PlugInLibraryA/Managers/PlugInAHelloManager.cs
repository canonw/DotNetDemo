using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLib.Managers;

namespace PlugInLibraryA.Managers
{
    public class PlugInAHelloManager : IHelloManager
    {
        public string Say(string message)
        {
            return string.Format("PlugInA says {0}", message);
        }
    }
}
