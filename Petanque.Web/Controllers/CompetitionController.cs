using System;
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
            var competitions = CompetitionService.GetMainCompetition();
            var competitionDtos = competitions.Select(x => new CompetitionDto { Id = x.Id, Nom = x.Name ?? "no name" });
            return View(competitionDtos);
        }

        //
        // GET: /Competition/Details/5

        public ActionResult Details(string id)
        {
            
            MainCompetition = CompetitionService.Find(id);
            return View(new CompetitionDto
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
        // GET: /Competition/Create

        public ActionResult Create()
        {
            var competitionDto = new CompetitionDto();
            return View(competitionDto);
        }

        public ActionResult SetWinnerAjax(bool isCrying, string teamId)
        {
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
            CompetitionService.StartCompetition(MainCompetition);
            return RedirectToAction("GetTree", new { MainCompetition.Id });
        }

        public ActionResult GetMainTree(string id)
        {
            MainCompetition = CompetitionService.Find(id);
            return RedirectToAction("GetTree", "Competition", new {isCrying = false});
            }

        public ActionResult GetTree(bool isCrying)
        {

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
                var competition = _competitionService.CreateCompetition(competitionDto.Nom, competitionDto.Price, competitionDto.BetByTeam, competitionDto.PercentPotForMainCompetion / 100.0);

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
