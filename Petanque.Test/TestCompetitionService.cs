using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Petanque.Model.Competition;
using Petanque.Model.Repository;
using Petanque.Model.Team;

namespace Petanque.Test
{
    [TestFixture]
    public class TestCompetitionService
    {
        private Mock<MongoRepository<Competition>> _mockRepo;
        private CompetitionService _competitionService;
        private List<Team> _teams;
        private Competition _competition;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _teams = TeamsGenerator.GetTeams(30);

            _competition = new Competition(_teams);
            _mockRepo = MockRepository.GetMongoRepository(new List<Competition>(){_competition});
            _competitionService = new CompetitionService(_mockRepo.Object);
        }


        [Test]
        public void TestCreateCompetition()
        {
        
            var teams = TeamsGenerator.GetCollectionOfTeam();
            var competition = _competitionService.CreateCompetition(teams);
            Assert.NotNull(competition);
        }


        [Test]
        public void TestCreateTree()
        {
            _competition = _competitionService.CreateCompetition(_teams);
            Assert.AreEqual(0,_competition.Teams.Count);
        }
      
        [Test]
        public void TestSearchInNode()
        {
            var team = _teams[10];
            var competition = _competitionService.CreateCompetition(_teams);
            var node = _competitionService.SearchNodeOfPlayer(team, competition.EndNode);
            Assert.AreEqual(team.Name, node.Team.Name);
        }

       
    }

    public static class MockRepository
    {
        public static Mock<MongoRepository<T>> GetMongoRepository<T>(IEnumerable<T> entries) where T :AbstractMongoEntity
        {
            var mock = new Mock<MongoRepository<T>>(null);
            mock.Setup(x => x.Save(It.IsAny<T>()));
            mock.Setup(x => x.QueryAll()).Returns(entries.AsQueryable());
            return mock;
        }
    }

    public static class TeamsGenerator
    {
        public static List<Team> GetCollectionOfTeam()
        {
            return new List<Team> { new Team("Tom"), new Team("Olive") };
        }

        public static List<Team> GetTeams(int nb)
        {
            var teams = new List<Team>();
            for (int i = 0; i < nb; i++)
            {
                teams.Add(new Team(RandomString(15, true)));
            }
            return teams;
        }

        /// <summary>
        /// Generates a random string with the given length
        /// </summary>
        /// <param name="size">Size of the string</param>
        /// <param name="lowerCase">If true, generate lowercase string</param>
        /// <returns>Random string</returns>
        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }

    public static class CompetitionGenerator
    {
        public static Competition GetCompetitionWithTwoTeam()
        {
            return new Competition(TeamsGenerator.GetCollectionOfTeam());
        }
    }
}



