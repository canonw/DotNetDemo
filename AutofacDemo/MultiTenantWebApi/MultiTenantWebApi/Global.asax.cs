using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Routing;
using Autofac;
using Autofac.Extras.Multitenant;
using Autofac.Integration.WebApi;
using MultiTenantWebApi.Managers;

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

            builder.RegisterType<HelloManager>().As<IHelloManager>().InstancePerDependency();

            // Standard API registration
            builder.RegisterWebApiFilterProvider(httpConfiguration);
            builder.RegisterApiControllers(assembly);

            var container = builder.Build();

            var tenantStrategy = new RequestParameterStrategy();

            var mtc = new MultitenantContainer(tenantStrategy, container);
            mtc.ConfigureTenant("A",
                b => b.RegisterType<HellloManagerA>().As<IHelloManager>()
                    .InstancePerDependency());
            mtc.ConfigureTenant("B",
                b => b.RegisterType<HellloManagerB>().As<IHelloManager>()
                    .InstancePerDependency());

            return new AutofacWebApiDependencyResolver(mtc);
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
