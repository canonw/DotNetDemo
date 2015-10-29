using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using PlugInLibraryA.Managers; 
using SharedLib.Managers;

namespace PlugInLibraryA
{
    public class PlugInAModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PlugInAHelloManager>().As<IHelloManager>().InstancePerRequest();
        }
    }
}