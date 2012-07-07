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

            _competition = new Competition("name", false);
            _mockRepo = MockRepository.GetMongoRepository(new List<Competition>(){_competition});
            _competitionService = new CompetitionService(_mockRepo.Object, null);
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
            return new List<Team> { new Team("Tom", false, 1), new Team("Olive", false, 2) };
        }

        public static List<Team> GetTeams(int nb)
        {
            var teams = new List<Team>();
            for (int i = 0; i < nb; i++)
            {
                teams.Add(new Team(RandomString(15, true), false, i + 1));
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
            var builder = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < size; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }

    public static class CompetitionGenerator
    {
        public static Competition GetCompetitionWithTwoTeam()
        {
            return null;
            //return new Competition(TeamsGenerator.GetCollectionOfTeam());
        }
    }
}



