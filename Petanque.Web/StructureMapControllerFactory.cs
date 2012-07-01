using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using Petanque.Model.Competition;
using Petanque.Model.Repository;
using Petanque.Model.Team;
using Petanque.Web.Controllers;
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
                ControllerBuilder.Current
                    .SetControllerFactory(new StructureMapControllerFactory());
                    var server = MongoServer.Create("mongodb://localhost/?safe=true");
                    var db = server.GetDatabase("petanque");


                ObjectFactory.Initialize(x =>
                                             {
                                                 x.For<MongoRepository<Team>>().LifecycleIs(new HybridLifecycle()).Use(
                                                     new MongoRepository<Team>(db));
                                                 x.For<MongoRepository<Competition>>().LifecycleIs(new HybridLifecycle()).Use(
                                                     new MongoRepository<Competition>(db));
                                             });
            }
        }
    }
}