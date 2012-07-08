using System.Collections.Generic;
using System.Linq;
using Petanque.Model.Repository;
using Petanque.Model.Results;

namespace Petanque.Model.Teams
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
                UpdatePlayedGame(result.TeamLoose, false);
            }
            UpdatePlayedGame(result.TeamWin, true);
        }

         public void UpdatePlayedGame(Team team, bool isWin)
         {
            if(isWin)
             {
                 team.WinInARow++;
             }
             team.GamePlayed++;
             
             Save(team);
         }
    }
}
