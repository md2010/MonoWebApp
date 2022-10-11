using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using DoctorAndPatients.Model;
using DoctorAndPatients.Model.Common;
using DoctorAndPatients.Repository;
using DoctorAndPatients.RepositoryCommon;
using DoctorAndPatients.Service;
using DoctorAndPatients.Service.Common;
using DoctorAndPatients.WebAPI.Controllers;
using DoctorAndPatients.WebAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DoctorAndPatients.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());            

            //dependecies

            //models
            builder.RegisterType<Doctor>().As<IDoctor>().InstancePerRequest();
            builder.RegisterType<Patient>().As<IPatient>().InstancePerRequest();

            //repositories
            builder.RegisterType<DoctorRepository>().As<IDoctorRepository>().InstancePerRequest();
            builder.RegisterType<PatientRepository>().As<IPatientRepository>().InstancePerRequest();

            //services
            builder.RegisterType<PatientService>().As<IPatientService>().InstancePerRequest();
            builder.RegisterType<DoctorService>().As<IDoctorService>().InstancePerRequest();
         
            //mapper           
            builder.Register(context => new MapperConfiguration(cfg =>
            {
                //Register Mapper Profile
                cfg.AddProfile<AutoMapperProfile>();
            }
            )).AsSelf().SingleInstance();

            builder.Register(c =>
            {
                //This resolves a new context that can be used later.
                var context = c.Resolve<IComponentContext>();
                var configuration = context.Resolve<MapperConfiguration>();
                return configuration.CreateMapper(context.Resolve);
            })
            .As<IMapper>()
            .InstancePerLifetimeScope();            

           
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);


            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
