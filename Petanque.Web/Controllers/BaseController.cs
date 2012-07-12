using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Petanque.Model.Competitions;

namespace Petanque.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly CompetitionService CompetitionService;

        public BaseController(CompetitionService competitionService)
        {
            CompetitionService = competitionService;
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            ViewBag.MainCompetitionId = MainCompetition!= null ? MainCompetition.Id : string.Empty;
            ViewBag.CryingCompetitionId = CryingCompetition != null ? CryingCompetition.Id :string.Empty;
        }


        private string CompetitionId
        {
            get { return Session["CompetitionId"] != null ? Session["CompetitionId"].ToString() : string.Empty; }
            set { Session["CompetitionId"] = value; }
        }

        protected Competition MainCompetition
        {
            get { return CompetitionService.Find(CompetitionId); }
            set
            {
                CompetitionId = value != null ? value.Id : string.Empty;
            }
        }

        protected Competition CryingCompetition
        {
            get { return MainCompetition != null ?
                CompetitionService.GetCompetition(MainCompetition.CryingCompetitionId) :
                null; }
        }

        protected Competition CurrentCompetion
        {
            get { return WorkOnCrying ? CryingCompetition : MainCompetition; }
        }

        protected bool WorkOnCrying { get; set; }

        protected Competition GetCurrentCompetition(bool isCrying)
        {
            var competition = !isCrying ? MainCompetition : CryingCompetition;
            return competition;
        }

    }
}
