using Petanque.Model.Tools.Extension;
using System.Linq;
using System.Collections.Generic;
using Petanque.Model.Repository;

namespace Petanque.Model.Competition
{
    public class CompetitionService
    {
        private readonly MongoRepository<Competition> _competitionRepo;
        private readonly NodeService _nodeService;

        public CompetitionService(MongoRepository<Competition> competitionRepo, NodeService nodeService)
        {
            _competitionRepo = competitionRepo;
            _nodeService = nodeService;
        }
        
        public Competition GetCompetition(string id)
        {
            var competition = _competitionRepo.Find(id);
            _nodeService.GetTree(competition);
            return competition;
        }

        public IEnumerable<Competition> GetAll()
        {
            return _competitionRepo.QueryAll().ToArray();
        }

        public void Save(Competition competition)
        {
            _competitionRepo.Save(competition);
        }

        public Competition Find(string id)
        {
            return _competitionRepo.Find(id);
        }

        public void Randomize(Competition competition)
        {
            competition.Shuffle();
            Save(competition);
        } dd

        public void AddTeam(Competition competition, Team.Team team)
        {
            competition.AddTeam(team);
            _competitionRepo.Save(competition);
        }

        public void SendToCryingCompetition(Competition competition, Team.Team team)
        {
            competition.AddTeam(team);
        }
    }
}