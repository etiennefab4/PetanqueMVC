using System;
using Petanque.Model.Competitions;
using Petanque.Model.Repository;
using Petanque.Model.Results;
using Petanque.Model.Teams;

namespace Petanque.Model.Prices
{
    public class PriceService
    {
        private readonly TeamService _teamService;
        private readonly MongoRepository<Competition> _competitionRepo;
        private readonly MongoRepository<Result> _resultRepo;

        public PriceService(TeamService teamService, MongoRepository<Competition> competitionRepo, MongoRepository<Result> resultRepo)
        {
            _teamService = teamService;
            _competitionRepo = competitionRepo;
            _resultRepo = resultRepo;
        }

        public void AttributPot(Competition competition)
        {
            competition.Pot = (competition.BetByTeam * competition.InitialTeams.Count) * competition.PercentOfThePot;
            _competitionRepo.Save(competition);
        }

        public void RefundTeam(Competition competition, Result result)
        {
            competition.Pot -= competition.BetByTeam;
            if (competition.Pot < 0)
            {
                throw new PotCannotBeNegatifException();
            }

            result.TeamWin.Gain += competition.BetByTeam;
            result.GainProcessed = true;
            _teamService.Save(result.TeamWin);
            _resultRepo.Save(result);
            _competitionRepo.Save(competition);
        }

        /// <summary>
        /// Avalaible only for team refund
        /// </summary>
        public void RewardWinnerTeam(Competition competition, Result result)
        {
            if (competition.DynamicPot <= 0.01 && result.DepthOfTheGame < competition.Depth - 2) return;

            competition.Pot -= competition.PriceForEachGame;
            if (competition.Pot < 0)
            {
                throw new PotCannotBeNegatifException();
            }

            result.TeamWin.Gain += competition.PriceForEachGame;
            result.GainProcessed = true;
            _teamService.Save(result.TeamWin);
            _resultRepo.Save(result);
            _competitionRepo.Save(competition);
        }

        public void AttributeGain(Competition competition, Result result)
        {

            if (result.TeamWin.WinInARow == 2)
            {
                RefundTeam(competition, result);
            }

            if (competition.AllCompetitorHavePlayedTwoTime && Math.Abs(competition.DynamicPot - 0) < 0.01)
            {
                competition.DynamicPot = competition.Pot;
                _competitionRepo.Save(competition);
            }

            RewardWinnerTeam(competition, result);
        }
    }
}

