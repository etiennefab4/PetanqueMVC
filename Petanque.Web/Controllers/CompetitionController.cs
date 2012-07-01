using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Petanque.Model.Competition;
using Petanque.Model.Team;

namespace Petanque.Web.Controllers
{
    public class CompetitionController : Controller
    {
        private readonly CompetitionService _competitionService;
        private readonly TeamService _teamService;

        public CompetitionController(CompetitionService competitionService, TeamService teamService)
        {
            _competitionService = competitionService;
            _teamService = teamService;
        }

        //
        // GET: /Competition/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Competition/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Competition/Create

        public ActionResult Create()
        {
            var teams = _teamService.GetAllTeams();
            var competition =_competitionService.CreateCompetition(teams.ToList());

            return View(competition);
        } 

        

        //
        // POST: /Competition/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Competition/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Competition/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Competition/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Competition/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
