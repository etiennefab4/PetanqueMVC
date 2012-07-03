using System.Collections.Generic;
using System.Linq;
using Petanque.Model.Competition;
using Petanque.Model.Repository;

namespace Petanque.Model.Team
{
    public class TeamService
    {
        private readonly MongoRepository<Team> _teamRepo;
        private readonly Competition.CompetitionService _competitionService;

        public TeamService(MongoRepository<Team> teamRepo, CompetitionService competitionService)
        {
            _teamRepo = teamRepo;
            _competitionService = competitionService;
        }

        public IEnumerable<Team> GetAllTeams()
        {
            return _teamRepo.QueryAll().ToArray();
        }

        public void Save(Team team)
        {
            _teamRepo.Save(team);
        }

        public Team Find(string id)
        {
            return _teamRepo.Find(id);
        }

        public void CreateTeamInCompetion(Team team, Competition.Competition competition)
        {
            competition.AddTeam(team);
            _competitionService.Save(competition);
            
        }
    }
}
