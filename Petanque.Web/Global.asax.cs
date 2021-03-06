﻿using System.Web.Mvc;
using System.Web.Routing;
using MongoDB.Bson.Serialization;
using Petanque.Model.Competitions;

namespace Petanque.Web
{
    // Remarque : pour obtenir des instructions sur l'activation du mode classique IIS6 ou IIS7, 
    // visitez http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Nom d'itinéraire
                "{controller}/{action}/{id}", // URL avec des paramètres
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Paramètres par défaut
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            BsonClassMap.RegisterClassMap<Competition>(
                cm =>
                    {
                        cm.MapProperty(c => c.Depth);
                        cm.MapProperty(c => c.InitialTeams);
                        cm.MapProperty(c => c.Name);
                        cm.MapProperty(c => c.Results);
                        cm.MapProperty(c => c.IsCryingCompetion);
                        cm.MapProperty(c => c.CryingCompetitionId);
                        cm.MapProperty(c => c.NbTeamMainCompetition);
                        cm.MapProperty(c => c.IsLocked);
                        cm.MapProperty(c => c.BetByTeam);
                        cm.MapProperty(c => c.Price);
                        cm.MapProperty(c => c.PercentOfThePot);
                        cm.MapProperty(c => c.Pot);
                    }
                );

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            StructureMapControllerFactory.Bootstrapper.Run();
        }
    }
}