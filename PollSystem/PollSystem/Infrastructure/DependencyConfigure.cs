using Autofac;
using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace PollSystem.Infrastructure
{
    internal class DependencyConfigure
    {
        public static void Initialize()
        {
            var builder = new ContainerBuilder();
            DependencyResolver.SetResolver(new Dependency(RegisterServices(builder)));
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly).PropertiesAutowired();
            builder.RegisterType<PollSystemEntities>().As<IPollSystemEntities>().InstancePerDependency();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<UserRoleService>().As<IUserRoleService>();
            builder.RegisterType<PollService>().As<IPollService>();
            builder.RegisterType<PollQuestionService>().As<IPollQuestionService>();
            builder.RegisterType<PollQuesAnswersService>().As<IPollQuesAnswersService>();
            builder.RegisterType<UserPollService>().As<IUserPollService>();
            builder.RegisterType<UserAnswerService>().As<IUserAnswerService>();
            return builder.Build();
        }
    }
}