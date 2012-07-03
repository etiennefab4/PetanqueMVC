using System;
using System.Linq;
using System.Collections.Generic;
using Petanque.Model.Repository;

namespace Petanque.Model.Competition
{
    public class Competition : AbstractMongoEntity
    {
        private readonly int _nbTeamMainCompetition;

        public int Depth
        {
            get
            {
                if (!IsCryingCompetion)
                {
                    var i = 0;

                    while (true)
                    {
                        var pow = (int)Math.Pow(2, i);
                        if (InitialTeams.Count <= pow)
                        {
                            return i + 1;
                        }
                        i++;
                    }
                }
                int remain;
                int nbTeamEliminateFirstGame = Math.DivRem(_nbTeamMainCompetition, 2, out remain) + remain;
                int nbTeamEliminateSecondGame = Math.DivRem(nbTeamEliminateFirstGame, 2, out remain) + remain;
                return nbTeamEliminateFirstGame + nbTeamEliminateSecondGame;
            }
        }

        public List<Result> Results { get; set; }

        public bool IsLocked { get { return Results.Any(); } }

        public bool IsCryingCompetion { get; set; }
        public Competition CryingCompetion { get; set; }

        public List<Team.Team> Teams { get; set; }
        public List<Team.Team> InitialTeams { get; set; }

        public Node EndNode;
        public string Name { get; set; }

        protected Competition()
        {
            Results = new List<Result> { };
            IsCryingCompetion = false;
            InitialTeams = new List<Team.Team>();
            EndNode = new Node { ParentNode = null };
        }

        public Competition(string name, bool isCryingCompetition, int nbTeamMainCompetition = 0)
            : this()
        {
            _nbTeamMainCompetition = nbTeamMainCompetition;

            IsCryingCompetion = isCryingCompetition;
            Name = name;
            if (!IsCryingCompetion)
            {
                CryingCompetion = new Competition(name, true);
            }
        }

        public void AddTeam(Team.Team team)
        {
            InitialTeams.Add(team);
        }
    }
}
