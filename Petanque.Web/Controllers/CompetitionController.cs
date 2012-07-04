using System;
using System.Linq;
using System.Web.Mvc;
using Petanque.Model.Competition;
using Petanque.Model.Team;
using Petanque.Web.Models;

namespace Petanque.Web.Controllers
{
    public class CompetitionController : Controller
    {
        private readonly CompetitionService _competitionService;
        private readonly TeamService _teamService;
        private readonly NodeService _nodeService;

        public CompetitionController(CompetitionService competitionService, TeamService teamService, NodeService nodeService)
        {
            _competitionService = competitionService;
            _teamService = teamService;
            _nodeService = nodeService;
        }

        //
        // GET: /Competition/

        public ActionResult Index()
        {
            var competitions = _competitionService.GetMainCompetition();
            var competitionDtos = competitions.Select(x => new CompetitionDto { Id = x.Id, Nom = x.Name ?? "no name" });
            return View(competitionDtos);
        }

        //
        // GET: /Competition/Details/5

        public ActionResult Details(string id)
        {
            var competition = _competitionService.Find(id);
            return View(new CompetitionDto
                            {
                                Id = competition.Id,
                                Nom = competition.Name,
                                TeamDtos = competition.InitialTeams.Select(x => new TeamDto
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

        public ActionResult Randomize(string id)
        {
            var competition = _competitionService.Find(id);
            _competitionService.Randomize(competition);
            return RedirectToAction("Edit", "Competition", new { id });
        }

        public ActionResult SetWinner(string id, string teamId)
        {
            var team = _teamService.Find(teamId);
            var competition = _competitionService.Find(id);

            try
            {
                _competitionService.AddResult(competition, team);
            }
            catch (Exception)
            {

            }

            return RedirectToAction("GetTree", "Competition", new { id });
        }

        public ActionResult GetTree(string id)
        {
            var competition = _competitionService.Find(id);

            if (competition.IsCryingCompetion && competition.InitialTeams.Count == 0)
            {
                _competitionService.PopulateCryingCompetition(competition);
            }

            var node = _nodeService.GetTree(competition);

            var competitionDto = new CompetitionDto
                                     {
                                         Id = competition.Id,
                                         Nom = competition.Name,
                                         Node = node,
                                         CryingCompetitionId = competition.CryingCompetitionId
                                     };
            return View("Tree", competitionDto);
        }


        public ActionResult GetTreeDebug(int nbTeam)
        {
            var competition = _competitionService.CreateCompetition(nbTeam);
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
                var competition = _competitionService.CreateCompetition(competitionDto.Nom);

                return RedirectToAction("AddTeamInCompetition", "Team", new { competitionId = competition.Id });
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
            var competition = _competitionService.Find(id);
            return View(new CompetitionDto()
                             {
                                 Id = competition.Id,
                                 Nom = competition.Name,
                                 TeamDtos = competition.InitialTeams.Select(x => new TeamDto
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
                var competition = _competitionService.Find(id);
                competition.Name = competitionDto.Nom;
                _competitionService.Save(competition);
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
            _competitionService.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
