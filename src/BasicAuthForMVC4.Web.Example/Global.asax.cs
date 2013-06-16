using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BasicAuthForMVC4.Common;
using BasicAuthForMVC4.Common.Configuration;
using BasicAuthForMVC4.Web.Common;
using BasicAuthForMVC4.Web.Common.Security;
using BasicAuthForMVC4.Web.Example;
using Ninject;
using log4net;

[assembly: PreApplicationStartMethod(typeof(Bootstrapper), "Init")]

namespace BasicAuthForMVC4.Web.Example
{
    public static class Bootstrapper
    {
        public static void Init()
        {
            var container = new StandardKernel();
            ContainerManager.Container = container;

            ConfigureContainer(container);
        }

        private static void ConfigureContainer(IKernel container)
        {
            AddBindings(container);

            var ninjectControllerFactory = new NinjectControllerFactory(container);
            ControllerBuilder.Current.SetControllerFactory(ninjectControllerFactory);
        }

        private static void AddBindings(IKernel container)
        {
            log4net.Config.XmlConfigurator.Configure();
            var loggerForWebSite = LogManager.GetLogger("BasicAuthForMVC4ExampleSite");
            container.Bind<ILog>().ToConstant(loggerForWebSite);

            container.Bind<ICurrentDateTime>().To<CurrentDateTime>();
            container.Bind<IEnvironment>().To<EnvironmentWrapper>();
            container.Bind<IConfigurationManager>().To<ConfigurationManagerAdapter>();
            container.Bind<IAuthenticationProvider>().To<BasicAuthenticationProvider>();
        }
    }

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}