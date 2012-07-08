using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Petanque.Model.Nodes;
using Petanque.Model.Prices;
using Petanque.Model.Repository;
using Petanque.Model.Teams;
using Petanque.Model.Tools.Extension;

namespace Petanque.Model.Competitions
{
    public class CompetitionService
    {
        private readonly MongoRepository<Competition> _competitionRepo;
        private readonly NodeService _nodeService;
        private readonly MongoRepository<Team> _teamRepo;
        private readonly PriceService _priceService;

        public CompetitionService(MongoRepository<Competition> competitionRepo, NodeService nodeService, MongoRepository<Team> teamRepo, PriceService priceService)
        {
            _competitionRepo = competitionRepo;
            _nodeService = nodeService;
            _teamRepo = teamRepo;
            _priceService = priceService;
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

        public void Lock(Competition competition)
        {
            competition.IsLocked = true;
            Save(competition); 
        }

        public void Randomize(Competition competition)
        {
            competition.Shuffle();
            Save(competition);
        }

        public bool IsTeamIsEliminated(Competition competition, Team team)
        {
            return competition.Results.Any(x => x.TeamLoose == team);
        }

        public void AddTeam(Competition competition, Team team)
        {
            if (competition.IsLocked)
            {
                throw new CannotAddTeamInLockedCompetition();
            }
            competition.AddTeam(team);
            _competitionRepo.Save(competition);
        }

        public void AddResult( Competition competition, Team team )
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

            if (team.WinInARow == 2)
            {
                _priceService.RefundTeam(competition, team);
            }

            if (team.WinInARow > 2)
            {

            }
            Save(competition);
            
        }

        void AddTeamInCryingCompetition(Competition competition, Team team)
        {
            var teamToReplace = competition.InitialTeams.FirstOrDefault(x => x.IsTeamToReplace);
            if (teamToReplace == null)
            {
                throw new Exception("pas possible normalement, learn to code noob");
            }
            competition.InitialTeams.Remove(teamToReplace);
            competition.AddTeam(team);
            team.WinInARow = 0;
            _teamRepo.Save(team);
            Save(competition);
        }

        public void CreateTeamInCompetion(Team team, Competition competition)
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
            var competition = new Competition("debug", false, 0, 0);
            for (int i = 0; i < nbTeam; i++)
            {
                competition.AddTeam(new Team("team-" + i, false, i + 1));
            }
            return competition;
        }

        public Competition CreateCompetition(string name)
        {
            var competition = new Competition(name, false, 0, 0);
            var cryingCompetition = new Competition(name, true, 0, 0);
            Save(cryingCompetition);
            competition.CryingCompetitionId = cryingCompetition.ObjectId.ToString();
            Save(competition);
            return competition;
        }

        public void StartCompetition(Competition competition)
        {
            var cryingCompetition = GetCryingCompetition(competition);
            Randomize(competition);
            PopulateCryingCompetition(cryingCompetition);
            AttributeBet(competition);
            AttributeBet(cryingCompetition);
            Lock(competition);

        }

        public Competition GetMainCompetition(Competition cryingCompetition)
        {
            return
                _competitionRepo.QueryAll().FirstOrDefault(x => x.CryingCompetitionId == cryingCompetition.Id);
        }


        public int GetNextNumber(Competition competition)
        {
            return competition.InitialTeams.Any() ? competition.InitialTeams.Max(x => x.Number) + 1 : 1;
        }

        #region private method

        private Competition GetCryingCompetition(Competition competition)
        {
            return Find(competition.CryingCompetitionId);
        }

        private void PopulateCryingCompetition(Competition competition)
        {
            if (competition.IsCryingCompetion && !competition.InitialTeams.Any())
            {
                var mainCompetition = GetMainCompetition(competition);
                competition.NbTeamMainCompetition = mainCompetition.NumberOfTeam;
                int nbTeamToAdd = competition.NumberOfTeam;
                for (int i = 0; i < nbTeamToAdd; i++)
                {
                    var team = new Team("A remplacer", true, 0);
                    competition.InitialTeams.Add(team);
                    _competitionRepo.Save(competition);
                }
            }
        }

        private void AttributeBet(Competition competition)
        {
            if(!competition.IsCryingCompetion)
            {
                competition.Pot = (competition.BetByTeam * competition.InitialTeams.Count) * competition.PercentOfThePot;
            }
            else
            {
                var mainCompetition = GetMainCompetition(competition);
                competition.Pot = (mainCompetition.BetByTeam * mainCompetition.InitialTeams.Count) * (1.0 - competition.PercentOfThePot);
            }

           Save(competition);
            
        }
        #endregion
    }
}