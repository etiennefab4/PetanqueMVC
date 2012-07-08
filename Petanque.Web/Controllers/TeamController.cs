using System.Linq;
using System.Web.Mvc;
using Petanque.Model.Competitions;
using Petanque.Model.Teams;
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


        public ActionResult IndexPartial(string id)
        {
            var competition = _competitionService.Find(id);
            var teamDtos = competition.InitialTeams.Select(x => new TeamDto() { Nom = x.Name, Id = x.Id });
            return PartialView("IndexTeam", teamDtos);
        }

        //
        // GET: /Team/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }



        [HttpPost]
        public ActionResult PartialCreate(TeamDto teamDto)
        {
            try
            {
                var team = new Team(teamDto.Nom, false, teamDto.Number);
                _teamService.Save(team);
                if (!string.IsNullOrEmpty(teamDto.CompetitionId))
                {
                    var competition = _competitionService.Find(teamDto.CompetitionId);
                    _competitionService.CreateTeamInCompetion(team, competition);
                    
                    return RedirectToAction("AddTeamInCompetitionPartial", "Team", new { competitionId = teamDto.CompetitionId });
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
            return View(new TeamDto()
                            {
                                Number = team.Number,
                                Nom = team.Name
                            });
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
            var dto = CreateTeamDto(competitionId);
            return View("Create", dto);
        }

        private CreateTeamDto CreateTeamDto(string competitionId)
        {
            var competition = _competitionService.Find(competitionId);
            var number = _competitionService.GetNextNumber(competition);

            var teamDto = new TeamDto()
                              {
                                  Number = number,
                                  CompetitionId = competitionId,
                              };
            var dto = new CreateTeamDto()
                          {
                              TeamDto = teamDto,
                              TeamDtos = competition.InitialTeams.Select(x => new TeamDto()
                                                                                  {
                                                                                      CompetitionId = competitionId,
                                                                                      Nom = x.Name,
                                                                                      Number = x.Number,
                                                                                      Id = x.Id
                                                                                  })
                          };
            return dto;
        }

        public ActionResult AddTeamInCompetitionPartial(string competitionId)
        {
            var dto = CreateTeamDto(competitionId);
            return PartialView("PartialCreate", dto);
        }

        public ActionResult AddTeamInCompetitionDebug(string competitionId, int nbTeam)
        {
            var competition = _competitionService.Find(competitionId);

            for (int i = 0; i < nbTeam; i++)
            {
                var team = new Team(string.Format("{0}-Team{1}", competition.Name, i), false, i + 1);
                _teamService.Save(team);
                _competitionService.AddTeam(competition, team);
            }

            return RedirectToAction("Edit", "Competition", new { id = competitionId });
        }
    }


}
