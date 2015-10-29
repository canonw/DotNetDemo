using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using SharedLib.Managers;

namespace SharedLib
{
    public class SharedLibModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HelloManager>().As<IHelloManager>().InstancePerRequest();
        }
    }
}
