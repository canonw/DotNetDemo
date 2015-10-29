using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using PlugInLibraryB.Managers;
using SharedLib.Managers;

namespace PlugInLibraryB
{
    public class PlugInBModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PlugInBHelloManager>().As<IHelloManager>().InstancePerRequest();
        }
    }
}