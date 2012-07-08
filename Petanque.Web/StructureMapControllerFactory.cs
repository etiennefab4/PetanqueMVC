using System;
using System.Web.Mvc;
using MongoDB.Driver;
using Petanque.Model.Competitions;
using Petanque.Model.Repository;
using Petanque.Model.Results;
using Petanque.Model.Teams;
using StructureMap;
using StructureMap.Pipeline;

namespace Petanque.Web
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            try
            {
                if ((requestContext == null) || (controllerType == null))
                    return null;

                return (Controller)ObjectFactory.GetInstance(controllerType);
            }
            catch (StructureMapException)
            {
                System.Diagnostics.Debug.WriteLine(ObjectFactory.WhatDoIHave());
                throw new Exception(ObjectFactory.WhatDoIHave());
            }
        }

        public static class Bootstrapper
        {
            public static void Run()
            {
                ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());

                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;

                var databaseName = MongoUrl.Create(connectionString).DatabaseName;
                var server = MongoServer.Create(connectionString);
                var db = server.GetDatabase(databaseName);


                ObjectFactory.Initialize(x =>
                                             {
                                                 x.For<MongoRepository<Team>>().LifecycleIs(new HybridLifecycle()).Use(
                                                     new MongoRepository<Team>(db));
                                                 x.For<MongoRepository<Competition>>().LifecycleIs(new HybridLifecycle()).Use(
                                                     new MongoRepository<Competition>(db));
                                                 x.For<MongoRepository<Result>>().LifecycleIs(new HybridLifecycle()).Use(
                                                    new MongoRepository<Result>(db));
                                             });
            }
        }
    }
}