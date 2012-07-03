using System;
using System.Collections.Generic;
using Petanque.Model.Repository;

namespace Petanque.Model.Competition
{
    public class Competition : AbstractMongoEntity
    {
        public int Depth
        {
            get
            {
                var i = 0;

                while (true)
                {
                    var pow = (int) Math.Pow(2, i);
                    if (InitialTeams.Count <= pow)
                    {
                        return i + 1;
                    }
                    i++;
                }

            }
        }

        public bool IsCryingCompetion { get; set; }
        public Competition CryingCompetion { get; set; }

        public List<Team.Team> Teams { get; set; }
        public List<Team.Team> InitialTeams { get; set; }

        public Node EndNode;
        public string Name { get; set; }

        protected Competition()
        {
            IsCryingCompetion = false;
            InitialTeams = new List<Team.Team>();
            EndNode = new Node { ParentNode = null };
        }

        public Competition(string name, bool isCryingCompetition):this()
        {
            IsCryingCompetion = isCryingCompetition;
            Name = name;
            if(!IsCryingCompetion)
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
