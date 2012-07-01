using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Petanque.Model.Repository;

namespace Petanque.Model.Competition
{
    public class Competition : AbstractMongoEntity
    {
        public int Depth { get; set; }

        public List<Team.Team> Teams { get; set; }
        public List<Team.Team> InitialTeams { get; set; }

        public Node EndNode;

        public Competition(List<Team.Team> teams)
        {
            Teams = teams;
            InitialTeams = new List<Team.Team>();
            InitialTeams.AddRange(teams);
            EndNode = new Node {ParentNode = null};
        }

        public void AddTeam(Team.Team team)
        {
            Teams.Add(team);
        }
    }
}
