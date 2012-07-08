using System;
using System.Collections.Generic;
using System.Linq;
using Petanque.Model.Nodes;
using Petanque.Model.Repository;
using Petanque.Model.Results;
using Petanque.Model.Teams;

namespace Petanque.Model.Competitions
{
    public class Competition : AbstractMongoEntity
    {

        public IEnumerable<Team> TeamsNotInCompetition
        {
            get { return Results.Select(x => x.TeamLoose).Distinct(); }
        }

        public IEnumerable<Team> TeamsInCompetition
        {
            get { return InitialTeams.Except(TeamsNotInCompetition); }
        }

        public IEnumerable<Team> TeamsToRefund
        {
            get
            {
                return InitialTeams.Where(team => Results.Count(x => x.TeamWin == team) >= 2);
            }
        }

        public bool AllCompetitorHavePlayedTwoTime
        {
            get { return !TeamsInCompetition.Any(x => x.GamePlayed < 2); }
        }

        public int NbTeamToRefund
        {
            get
            {
                if(AllCompetitorHavePlayedTwoTime)
                {
                    return TeamsToRefund.Count();
                }
                throw new CannotDeterminateNowTheNbTeamToRefundException();
            }
        }

        public int NbTeamMainCompetition { get; set; }

        public string CryingCompetitionId { get; set; }

        public int Depth
        {
            get
            {
                var i = 0;

                while (true)
                {
                    var pow = (int)Math.Pow(2, i);
                    if (NumberOfTeam <= pow)
                    {
                        return i + 1;
                    }
                    i++;
                }
            }
        }

        public int NumberOfTeam
        {
            get
            {
                if (!InitialTeams.Any())
                {
                    int remain;
                    int nbTeamEliminateFirstGame = Math.DivRem(NbTeamMainCompetition, 2, out remain) + remain;
                    int nbTeamEliminateSecondGame = Math.DivRem(NbTeamMainCompetition - nbTeamEliminateFirstGame, 2, out remain) + remain;
                    return nbTeamEliminateFirstGame + nbTeamEliminateSecondGame;
                }
                return InitialTeams.Count;
            }
        }

        public List<Result> Results { get; set; }

        public bool IsLocked { get; set; }

        public bool IsCryingCompetion { get; set; }

        public double Price { get; set; }
        public double BetByTeam { get; set; }
        public double Pot { get; set; }
        public double PercentOfThePot { get; set; }
        public List<Team> Teams { get; set; }
        public List<Team> InitialTeams { get; set; }

        public Node EndNode;
       
        public string Name { get; set; }

        protected Competition()
        {
            Results = new List<Result> { };
            IsCryingCompetion = false;
            InitialTeams = new List<Team>();
            EndNode = new Node { ParentNode = null };
        }

        public Competition(string name, bool isCryingCompetition, double price, double betByTeam)
            : this()
        {
            IsCryingCompetion = isCryingCompetition;
            Price = price;
            BetByTeam = betByTeam;
            Name = name;
        }

        public void AddTeam(Team team)
        {
            InitialTeams.Add(team);
        }
    }
}
