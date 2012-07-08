using Petanque.Model.Competitions;
using Petanque.Model.Repository;
using Petanque.Model.Teams;

namespace Petanque.Model.Prices
{
    public class PriceService
    {
        private readonly TeamService _teamService;
        private readonly MongoRepository<Competition> _competitionRepo; 

        public PriceService(TeamService teamService, MongoRepository<Competition> competitionRepo)
        {
            _teamService = teamService;
            _competitionRepo = competitionRepo;
        }

        public void RefundTeam(Competition competition, Team team)
        {
            competition.Pot -= competition.BetByTeam;
            if(competition.Pot < 0)
            {
                throw new PotCannotBeNegatifException();
            }
            team.Gain += competition.BetByTeam;
            _teamService.Save(team);
            _competitionRepo.Save(competition);
        }
    }
}
