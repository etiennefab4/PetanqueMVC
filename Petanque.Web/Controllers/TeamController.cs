using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Petanque.Model.Competition;
using Petanque.Model.Team;
using Petanque.Web.Models;

namespace Petanque.Web.Controllers
{
    public class TeamController : Controller
    {
        private readonly TeamService _teamService;
        private readonly CompetitionService _competitionService;

        public TeamController(TeamService teamService, CompetitionService competitionService)
        {
            _teamService = teamService;
            _competitionService = competitionService;
        }

        //
        // GET: /Team/

        public ActionResult Index()
        {
            var teams = _teamService.GetAllTeams();
            var teamDtos = teams.Select(x => new TeamDto() { Nom = x.Name, Id = x.Id });
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
                var team = new Team(teamDto.Nom, false);
                _teamService.Save(team);
                if (!string.IsNullOrEmpty(teamDto.CompetitionId))
                {
                    var competition = _competitionService.Find(teamDto.CompetitionId);
                    _competitionService.CreateTeamInCompetion(team, competition);
                    return RedirectToAction("AddTeamInCompetition", "Team", new { competitionId = teamDto.CompetitionId });
                }
                

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
            return View(new TeamDto() { Nom = team.Name });
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

        public ActionResult AddTeamInCompetition(string competitionId)
        {
            var teamDto = new TeamDto()
                              {
                                  CompetitionId = competitionId
                              };
            return View("Create", teamDto);
        }

        public ActionResult AddTeamInCompetitionDebug(string competitionId, int nbTeam)
        {
            var competition = _competitionService.Find(competitionId);

            for (int i = 0; i < nbTeam; i++)
            {
                var team = new Team(string.Format("{0}-Team{1}",competition.Name, i), false);
                _teamService.Save(team);
                _competitionService.AddTeam(competition, team);
            }

            return RedirectToAction("Edit", "Competition", new {id = competitionId});
        }

        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }

  
}
