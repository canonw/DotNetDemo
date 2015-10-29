using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Routing;
using Autofac;
using Autofac.Extras.Multitenant;
using Autofac.Extras.Multitenant.Web;
using Autofac.Integration.WebApi;
using SharedLib;
using SharedLib.Managers;

namespace MultiTenantWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            GlobalConfiguration.Configuration.DependencyResolver = RegisterBuilderAutofacMultitenant(GlobalConfiguration.Configuration);
        }

        private IDependencyResolver RegisterBuilderAutofacMultitenant(HttpConfiguration httpConfiguration)
        {
            // From http://docs.autofac.org/en/latest/advanced/multitenant.html#what-is-multitenancy

            var builder = new ContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterModule(new SharedLibModule());
            //builder.RegisterType<HelloManager>().As<IHelloManager>().InstancePerRequest();

            // TODO: Make configurable
            builder.RegisterType<RequestParameterStrategy>().As<ITenantIdentificationStrategy>();

            // Standard API registration
            builder.RegisterWebApiFilterProvider(httpConfiguration);
            builder.RegisterApiControllers(assembly);

            var container = builder.Build();

            //if (container.IsRegistered<ITenantIdentificationStrategy>())

            var tenantStrategy = container.Resolve<ITenantIdentificationStrategy>();

            var mtc = new MultitenantContainer(tenantStrategy, container);

            //mtc.ConfigureTenant("A",
            //    b => b.RegisterType<HellloManagerA>().As<IHelloManager>()
            //        .InstancePerDependency());

            // Read assembly custom attribute
            var groupedAssemblies = LoadAssemblies().Select(ass => new
            {
                Assembly = ass,
                TenantAttribute = ass.GetCustomAttribute<TenantAttributeAttribute>()
            })
            .Where(o => o.TenantAttribute != null)
            .GroupBy(o => o.TenantAttribute.TenantId, o => o.Assembly);

            foreach (var group in groupedAssemblies)
            {
                // TODO: Log tenant being loaded at info level to give a way to debug issue.
                var grp = group;
                mtc.ConfigureTenant(grp.Key, cb =>
                {
                    cb.RegisterAssemblyModules(grp.ToArray());
                });
            }

            return new AutofacWebApiDependencyResolver(mtc);
        }

        public IEnumerable<Assembly> LoadAssemblies()
        {
            // TODO: Fix path loading
            // TODO: If plugin modified by not webapp.  plugin should not locked
            var plugInPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "bin");
            var directory = new DirectoryInfo(plugInPath);
            var files = directory.GetFiles("*.dll", SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                var assemblyName = AssemblyName.GetAssemblyName(file.FullName);
                var assembly = AppDomain.CurrentDomain.Load(assemblyName);
                yield return assembly;
            }

            yield break;
        }

        public static IDependencyResolver RegisterBuilderAutofac(HttpConfiguration httpConfiguration)
        {
            var builder = new ContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterType<HelloManager>().As<IHelloManager>();

            // Standard API registration
            builder.RegisterWebApiFilterProvider(httpConfiguration);
            builder.RegisterApiControllers(assembly);

            var container = builder.Build();
            return new AutofacWebApiDependencyResolver(container);
        }
    }

    internal class RequestParameterStrategy : ITenantIdentificationStrategy
    {
        public bool TryIdentifyTenant(out object tenantId)
        {
            // This is an EXAMPLE ONLY and is NOT RECOMMENDED.
            tenantId = null;
            try
            {
                var context = HttpContext.Current;
                if (context != null && context.Request != null)
                {
                    tenantId = context.Request.Params["tenant"];
                }
            }
            catch (HttpException)
            {
                // Happens at app startup in IIS 7.0
            }
            return tenantId != null;
        }
    }
}
