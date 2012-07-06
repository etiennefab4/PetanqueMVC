using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Bson;
using Petanque.Model.Repository;

namespace Petanque.Model.Competition
{
    public class Competition : AbstractMongoEntity
    {

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

        public Competition(string name, bool isCryingCompetition)
            : this()
        {
            IsCryingCompetion = isCryingCompetition;
            Name = name;
        }

        public void AddTeam(Team.Team team)
        {
            InitialTeams.Add(team);
        }
    }
}
