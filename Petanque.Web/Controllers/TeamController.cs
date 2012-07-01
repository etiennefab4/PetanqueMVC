using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Petanque.Model.Repository;
using Petanque.Model.Team;
using Petanque.Web.Models;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace Petanque.Web.Controllers
{
    public class TeamController : Controller
    {
        private readonly TeamService _teamService ;

        public TeamController(TeamService teamService)
        {
            _teamService = teamService;
        }

        //
        // GET: /Team/

        public ActionResult Index()
        {
            var teams = _teamService.GetAllTeams();
            var teamDtos = teams.Select(x => new TeamDto() {Nom = x.Name, Id = x.Id});
            return View(teamDtos);
        }

        //
        // GET: /Team/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Team/Create

        public ActionResult Create()
        {
            var teamDto = new TeamDto(); 
            return View(teamDto);
        } 

        //
        // POST: /Team/Create

        [HttpPost]
        public ActionResult Create(TeamDto teamDto)
        {
            try
            {
                var team = new Team(teamDto.Nom);
                _teamService.Save(team);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Team/Edit/5
 
        public ActionResult Edit(string id)
        {
            var team = _teamService.Find(id);
            return View(new TeamDto(){Nom = team.Name});
        }

        //
        // POST: /Team/Edit/5
            
        [HttpPost]
        public ActionResult Edit(string id, TeamDto teamDto)
        {
            try
            {
                var team = _teamService.Find(id);
                team.Name = teamDto.Nom;
                _teamService.Save(team);
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Team/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Team/Delete/5

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
