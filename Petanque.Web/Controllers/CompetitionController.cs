﻿using System;
using System.Linq;
using System.Web.Mvc;
using Petanque.Model.Competitions;
using Petanque.Model.Nodes;
using Petanque.Model.Teams;
using Petanque.Web.Models;

namespace Petanque.Web.Controllers
{
    public class CompetitionController : BaseController
    {
        private readonly TeamService _teamService;
        private readonly NodeService _nodeService;

        public CompetitionController(TeamService teamService, NodeService nodeService, CompetitionService competitionService) : base(competitionService)
        {
            _teamService = teamService;
            _nodeService = nodeService;
        }

        //
        // GET: /Competition/

        public ActionResult Index()
        {
            MainCompetition = null;
            var competitions = CompetitionService.GetMainCompetition();
            var competitionDtos = competitions.Select(x => new CompetitionDto { Id = x.Id, Nom = x.Name ?? "no name" });
            return View(competitionDtos);
        }

        public ActionResult Select(string id)
        {
            MainCompetition = CompetitionService.Find(id);
            var competitions = CompetitionService.GetMainCompetition();
            var competitionDtos = competitions.Select(x => new CompetitionDto { Id = x.Id, Nom = x.Name ?? "no name" });
            return View("Index", competitionDtos );
        }

        //
        // GET: /Competition/Create

        public ActionResult Create()
        {
            var competitionDto = new CompetitionDto();
            return View(competitionDto);
        }

        public ActionResult SetWinnerAjax(bool isCrying, string teamId)
        {
            ViewBag.IsCrying = isCrying;
            var team = _teamService.Find(teamId);
            var competition = GetCurrentCompetition(isCrying);
            if (!competition.IsLocked && !competition.IsCryingCompetion)
            {
                return RedirectToAction("GetTree", "Competition", new { isCrying });
            }

            try
            {
                CompetitionService.AddResult(competition, team);
            }
            //TODO fix ça un jour
            catch (Exception)
            {

            }
            return RedirectToAction("GetTreePartial", "Competition", new { isCrying });
        }

        public ActionResult StartCompetition()
        {
            ViewBag.IsCrying = false;
            CompetitionService.StartCompetition(MainCompetition);
            return RedirectToAction("GetTree", new { MainCompetition.Id });
        }

        public ActionResult GetTree(bool isCrying)
        {
            ViewBag.IsCrying = isCrying;
            var competition = GetCurrentCompetition(isCrying);
            ViewBag.Page =  Page.TreeConsolante;
            var node = _nodeService.GetTree(competition);

            var competitionDto = new CompetitionDto
                                     {
                                         Id = competition.Id,
                                         Nom = competition.Name,
                                         Node = node,
                                     };
            return View("Tree", competitionDto);
        }

       
        public ActionResult GetTreePartial(bool isCrying)
        {
            ViewBag.IsCrying = isCrying;
            var competition = GetCurrentCompetition(isCrying);

            var node = _nodeService.GetTree(competition);

            var competitionDto = new CompetitionDto
            {
                Id = competition.Id,
                Nom = competition.Name,
                Node = node,
            };
            return PartialView("PartialNode", competitionDto.Node);
        }

        public ActionResult GetTreeDebug(int nbTeam)
        {
            var competition = CompetitionService.CreateCompetition(nbTeam);
            var node = _nodeService.GetTree(competition);
            var competitionDto = new CompetitionDto
            {
                Id = competition.Id,
                Nom = competition.Name,
                Node = node
            };
            return View("Tree", competitionDto);
        }


        //
        // POST: /Competition/Create

        [HttpPost]
        public ActionResult Create(CompetitionDto competitionDto)
        {
            try
            {
                ViewBag.IsCrying = false;
                MainCompetition = CompetitionService.CreateCompetition(competitionDto.Nom, competitionDto.Price, competitionDto.BetByTeam, competitionDto.PercentPotForMainCompetion / 100.0);

                return RedirectToAction("AddTeamInCompetition", "Team", new { competitionId = MainCompetition.Id });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Competition/Edit/5

        public ActionResult Edit(string id)
        {
            ViewBag.IsCrying = false;
            MainCompetition = CompetitionService.Find(id);
            return View(new CompetitionDto()
                             {
                                 Id = MainCompetition.Id,
                                 Nom = MainCompetition.Name,
                                 TeamDtos = MainCompetition.InitialTeams.Select(x => new TeamDto
                                                                                    {
                                                                                        Id = x.Id,
                                                                                        Nom = x.Name
                                                                                    })
                             });
        }

     

        //
        // POST: /Competition/Edit/5

        [HttpPost]
        public ActionResult Edit(string id, CompetitionDto competitionDto)
        {
            try
            {
                ViewBag.IsCrying = false;
                var competition = CompetitionService.Find(id);
                competition.Name = competitionDto.Nom;
                CompetitionService.Save(competition);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Competition/Delete/5

        public ActionResult Delete(string id)
        {
            CompetitionService.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
