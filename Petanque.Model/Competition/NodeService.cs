using System;
using System.Collections.Generic;
using System.Linq;
using Petanque.Model.Repository;
using Petanque.Model.Team;

namespace Petanque.Model.Competition
{
    public class NodeService
    {
        private readonly MongoRepository<Result> _resultRepo;
        private readonly MongoRepository<Competition> _competitionRepo;
        private readonly TeamService _teamService;

        public NodeService(MongoRepository<Result> resultRepo, TeamService teamService, MongoRepository<Competition> competitionRepo)
        {
            _resultRepo = resultRepo;
            _teamService = teamService;
            _competitionRepo = competitionRepo;
        }

        public Node ApplyResultOnCompetition(Competition competition, Node nodeTree)
        {
            return competition.Results.OrderBy(x => x.Date).Aggregate(nodeTree, (current, result) => ApplyResultOnCompetition(result, current));
        }

        public Result CreateResult(Node rootNode, Team.Team teamWin)
        {

            Node nodeOfTeamWin = SearchNodeOfTeam(teamWin, rootNode);

            if (nodeOfTeamWin.ParentNode == null)
            {
                throw new ResultImpossibleException();
            }

            Node nodeOfTeamLoose = nodeOfTeamWin.ParentNode.BottomNode == nodeOfTeamWin ? nodeOfTeamWin.ParentNode.TopNode : nodeOfTeamWin.ParentNode.BottomNode;

            var result = new Result { TeamWin = teamWin, TeamLoose = nodeOfTeamLoose.Team };

            _teamService.UpdatePlayedGame(result);

            _resultRepo.Save(result);
            return result;
        }

        public Node GetTree(Competition competition)
        {
            
            var tmpTeams = new List<Team.Team>();
            tmpTeams.AddRange(competition.InitialTeams);
            var nbTeamInFirstLevel = CalculateNbTeamInFirstLevel(competition.InitialTeams.Count(), competition.Depth);
            var listTeamInFirstLevel = new List<Team.Team>();

            for (int i = 0; i < nbTeamInFirstLevel; i++)
            {
                listTeamInFirstLevel.Add(PickATeam(tmpTeams));
            }

            var nodeTree = CreateNode(listTeamInFirstLevel, tmpTeams, competition, null, 0, competition.Depth - 1);
            AutoPlayForMissingTeam(nodeTree);
            return ApplyResultOnCompetition(competition, nodeTree);
        }

        #region private

        private void CheckTeamWithoutOpponent(Node node)
        {
            if (node == null) return;
            if (node.BottomNode == null || node.BottomNode.Team == null)
            {
                if (node.BottomNode != null && node.BottomNode.Team == null)
                {
                    CheckTeamWithoutOpponent(node.BottomNode);
                    return;
                }
                if (node.TopNode != null && node.TopNode.Team != null)
                {
                    node.Team = node.TopNode.Team;
                }
            }
            else if (node.TopNode == null || node.TopNode.Team == null)
            {
                if (node.TopNode != null && node.TopNode.Team == null)
                {
                    CheckTeamWithoutOpponent(node.TopNode);
                    return;
                }
                node.Team = node.BottomNode.Team;
            }
        }

        private void AutoPlayForMissingTeam(Node rootNode)
        {
            if (rootNode == null) return;

            AutoPlayForMissingTeam(rootNode.BottomNode);
            AutoPlayForMissingTeam(rootNode.TopNode);

            CheckTeamWithoutOpponent(rootNode.BottomNode);
            CheckTeamWithoutOpponent(rootNode.TopNode);
        }



        private Node SearchNodeOfTeam(Team.Team team, Node rootNode)
        {
            if (rootNode.Team != null && rootNode.Team.Id == team.Id)
            {
                return rootNode;
            }

            Node tmpNode;

            if (rootNode.BottomNode != null)
            {
                tmpNode = SearchNodeOfTeam(team, rootNode.BottomNode);
                if (tmpNode != null) return tmpNode;
            }

            if (rootNode.TopNode != null)
            {
                tmpNode = SearchNodeOfTeam(team, rootNode.TopNode);
                if (tmpNode != null) return tmpNode;
            }

            return null;
        }

        private int CalculateNbTeamInFirstLevel(int nbTeams, int depth)
        {
            var pow = (int)Math.Pow(2, depth - 1);
            if (pow == nbTeams)
            {
                return nbTeams;
            }
            if (pow > nbTeams)
            {
                var extras = nbTeams - (int)Math.Pow(2, depth - 1 - 1);
                return 2 * extras;
            }
            throw new NumberOfTeamIncoherentWithDepthException();
        }

        private Node ApplyResultOnCompetition(Result result, Node rootNode)
        {
            var node = SearchNodeOfTeam(result.TeamWin, rootNode);
            if (node.ParentNode.Team == null)
            {
                node.ParentNode.Team = result.TeamWin;
            }
            return rootNode;
        }

        private Team.Team PickATeam(List<Team.Team> teams)
        {
            if (!teams.Any()) return null;
            var teamPicked = teams.First();
            teams.Remove(teamPicked);
            return teamPicked;
        }

        private Node CreateNode(List<Team.Team> listTeamInFirstLevel, List<Team.Team> teams, Competition competition, Node parentNode, int levelTree, int depth)
        {
            if (levelTree > depth)
                return null;
            if (!listTeamInFirstLevel.Any() && !teams.Any() && parentNode != null)
                return null;

            var node = new Node { ParentNode = parentNode, DepthOfTheTree = depth, Level = levelTree, CompetitionId = competition.Id };

            if (!listTeamInFirstLevel.Any() && teams.Any() && (levelTree == depth - 1))
            {
                var team = PickATeam(teams);
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

            node.TopNode = CreateNode(listTeamInFirstLevel, teams, competition, node, levelTree + 1, depth);
            node.BottomNode = CreateNode(listTeamInFirstLevel, teams, competition, node, levelTree + 1, depth);
            return node;
        }
        #endregion
    }
}