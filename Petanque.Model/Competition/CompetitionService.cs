using System;
using System.Linq;
using System.Collections.Generic;
using Petanque.Model.Repository;

namespace Petanque.Model.Competition
{
    public class CompetitionService
    {
        private readonly MongoRepository<Competition> _competitionRepo;

        public CompetitionService(MongoRepository<Competition> competitionRepo)
        {
            _competitionRepo = competitionRepo;
        }

        public int CalculateDepth(int nbTeam)
        {
            var i = 0;

            while (true)
            {
                var pow = (int)Math.Pow(2, i);
                if (nbTeam <= pow)
                {
                    return i + 1;
                }
                i++;
            }
        }

        public void CreateTree(Competition competition)
        {
            var nbTeamInFirstLevel = CalculateNbTeamInFirstLevel(competition.InitialTeams.Count(), competition.Depth);
            var listTeamInFirstLevel = new List<Team.Team>();
            
            for (int i = 0; i < nbTeamInFirstLevel; i++)
            {
                listTeamInFirstLevel.Add(PickATeam(competition.Teams));
            }

            competition.EndNode = CreateNode(listTeamInFirstLevel, competition, null, 0, competition.Depth -1);
        }

        public Node CreateNode(List<Team.Team> listTeamInFirstLevel, Competition competition, Node parentNode, int levelTree, int depth)
        {
            if (levelTree > depth)
                return null;
            if (!listTeamInFirstLevel.Any() && !competition.Teams.Any() && parentNode != null)
                return null;

            var node = new Node { ParentNode = parentNode, DepthOfTheTree = depth, Level = levelTree};

            if (!listTeamInFirstLevel.Any() && competition.Teams.Any() && (levelTree == depth - 1))
            {
                var team = PickATeam(competition.Teams);
                node.TopNode = null;
                node.BottomNode = null;
                node.Team = team;
                
            }

            if (listTeamInFirstLevel.Any() && levelTree == depth)
            {  
                var team = PickATeam(listTeamInFirstLevel);
                node.TopNode = null;
                node.BottomNode = null;
                node.Team = team;
            }

            node.TopNode = CreateNode(listTeamInFirstLevel, competition, node, levelTree + 1, depth);
            node.BottomNode = CreateNode(listTeamInFirstLevel, competition, node, levelTree + 1, depth);
            return node;
        }

        public int CalculateNbTeamInFirstLevel(int nbTeams, int depth)
        {
            var pow = (int)Math.Pow(2, depth - 1);
            if (pow == nbTeams)
            {
                return nbTeams;
            }
            if (pow > nbTeams)
            {
                var extras = nbTeams - (int)Math.Pow(2, depth - 1 - 1) ;
                return 2 * extras;
            }
            throw new NumberOfTeamIncoherentWithDepthException();
        }

        public Competition CreateCompetition(List<Team.Team> teams)
        {
            var competition = new Competition(teams) { Depth = CalculateDepth(teams.Count()) };
            CreateTree(competition);
            return competition;
        }

        public Node SearchNodeOfPlayer(Team.Team team, Node rootNode)
        {
            if(rootNode.Team != null && rootNode.Team.Name == team.Name)
            {
                return rootNode;
            }
            
            if(rootNode.BottomNode != null)
            {
                return SearchNodeOfPlayer(team, rootNode.BottomNode);
            }

            if(rootNode.TopNode != null)
            {
                return SearchNodeOfPlayer(team, rootNode.TopNode);
            }

            return null;
        }

        public Team.Team PickATeam(List<Team.Team> teams)
        {
            if (!teams.Any()) return null;
            var random = new Random().Next(0, teams.Count() - 1);
            var teamPicked = teams[random];
            teams.Remove(teamPicked);
            return teamPicked;
        }

        public void ApplyResultOnCompetition(Result result)
        {
            var node = SearchNodeOfPlayer(result.TeamWin, result.Competition.EndNode);
            node.ParentNode.Team = result.TeamWin;
        }
    }
}