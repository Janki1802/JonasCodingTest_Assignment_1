using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Services;
using DataAccessLayer.Database;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System.Web.Http;


namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Set up Web API routing
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{code}",
                defaults: new { code = RouteParameter.Optional }
            );

            // Create a Simple Injector container
            var container = new Container();

            // Register Web API controllers
            container.RegisterWebApiControllers(config);

            // Register application services
            container.Register<ICompanyService, CompanyService>();
            container.Register<ICompanyRepository, CompanyRepository>();
            container.Register<IEmployeeService, EmployeeService>();
            container.Register<IEmployeeRepository, EmployeeRepository>();

            // Register in-memory database wrapper
            container.Register(typeof(IDbWrapper<>), typeof(InMemoryDatabase<>), Lifestyle.Singleton);

            // Configure AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AppServicesProfile>(); // Register AutoMapper profiles
            });
            IMapper mapper = mapperConfig.CreateMapper();
            container.RegisterInstance(mapper);

            // Register logging services
            container.RegisterSingleton<ILoggerFactory>(() => new LoggerFactory());
            container.Register(typeof(ILogger<>), typeof(Logger<>), Lifestyle.Singleton);

            // Verify the container setup
            container.Verify();

            // Set the dependency resolver to use Simple Injector
            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}
