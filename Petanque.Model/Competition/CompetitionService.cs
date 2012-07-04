using MongoDB.Bson;
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

        public IEnumerable<Competition> GetMainCompetition()
        {
            return _competitionRepo.QueryAll().Where(x => !x.IsCryingCompetion).ToArray();
        }

        public void Save(Competition competition)
        {
            _competitionRepo.Save(competition);
        }

        public Competition Find(string id)
        {
            return _competitionRepo.Find(id);
        }

        public Competition Find(ObjectId id)
        {
            return _competitionRepo.Find(id);
        }

        public void Randomize(Competition competition)
        {
            competition.Shuffle();
            Save(competition);
        }

        public void AddTeam(Competition competition, Team.Team team)
        {
            if (competition.IsLocked)
            {
                throw new CannotAddTeamInLockedCompetition();
            }
            competition.AddTeam(team);
            _competitionRepo.Save(competition);
        }

        public void AddResult( Competition competition, Team.Team team )
        {
            var rootNode = _nodeService.GetTree(competition);
            var result = _nodeService.CreateResult(rootNode, team);

            if(!competition.IsCryingCompetion)
            {
                if(team.CanSendToCryingCompetetion)
                {
                    var cryingCompetition = GetCryingCompetition(competition);

                    if (result.TeamLoose != null)
                    {
                        AddTeamInCryingCompetition(cryingCompetition, result.TeamLoose);
                    }
                    
                    Save(cryingCompetition);
                }
            }

            competition.Results.Add(result);
            Save(competition);
            
        }

        void AddTeamInCryingCompetition(Competition competition, Team.Team team)
        {
            competition.InitialTeams.Where(x => x.IsTeamToReplace).ToList().RemoveAt(0);
            competition.AddTeam(team);
        }

        public void CreateTeamInCompetion(Team.Team team, Competition competition)
        {
            competition.AddTeam(team);
            Save(competition);
        }


        public void Delete(string  id)
        {
            _competitionRepo.Delete(id);
        }

        public Competition CreateCompetition(int nbTeam)
        {
            var competition = new Competition("debug", false);
            for (int i = 0; i < nbTeam; i++)
            {
                competition.AddTeam(new Team.Team("team-" + i, false));
            }
            return competition;
        }

        public Competition CreateCompetition(string name)
        {
            var competition = new Competition(name, false);
            var cryingCompetition = new Competition(name, true);
            Save(cryingCompetition);
            competition.CryingCompetitionId = cryingCompetition.ObjectId.ToString();
            Save(competition);
            return competition;
        }

        public Competition GetCryingCompetition(Competition competition)
        {
            return Find(competition.CryingCompetitionId);
        }

        public Competition GetMainCompetition(Competition cryingCompetition)
        {
            return
                _competitionRepo.QueryAll().FirstOrDefault(x => x.CryingCompetitionId == cryingCompetition.Id);
        }

        public void PopulateCryingCompetition(Competition competition)
        {
            if (competition.IsCryingCompetion && !competition.InitialTeams.Any())
            {
                var mainCompetition = GetMainCompetition(competition);
                competition.NbTeamMainCompetition = mainCompetition.NumberOfTeam;
                for (int i = 0; i < competition.NumberOfTeam; i++)
                {
                    var team = new Team.Team("A remplacer", true);
                    competition.InitialTeams.Add(team);
                    _competitionRepo.Save(competition);
                }
            }
        }
    }
}