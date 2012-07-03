using System.Collections.Generic;
using System.Linq;
using Petanque.Model.Competition;
using Petanque.Model.Repository;

namespace Petanque.Model.Team
{
    public class TeamService
    {
        private readonly MongoRepository<Team> _teamRepo;

        public TeamService(MongoRepository<Team> teamRepo)
        {
            _teamRepo = teamRepo;
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

        public void UpdatePlayedGame(Result result)
        {
            if (result.TeamLoose != null)
            {
                UpdatePlayedGame(result.TeamLoose);
            }
            UpdatePlayedGame(result.TeamWin);
        }

         public void UpdatePlayedGame(Team team)
         {
             team.GamePlayed++;
             Save(team);
         }
    }
}
