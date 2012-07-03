using System.Collections.Generic;
using MongoDB.Driver;
using NUnit.Framework;
using Petanque.Model.Repository;
using Petanque.Model.Team;
using System.Linq;

namespace Petanque.Test
{
    [TestFixture]
    public class TestMongoDB
    {
        private MongoDatabase _db;
        private MongoServer _server;

        [SetUp]
        public void SetUpFixture()
        {
            _db.DropCollection(typeof(Team).FullName);
        }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _server = MongoServer.Create("mongodb://localhost/?safe=true");
            _db = _server.GetDatabase("petanque");
            
        }

        [Test]
        public void TestInsert()
        {
            var t = new Team {Name = "Test"};
            var repo = new MongoRepository<Team>(_db);
            repo.Save(t);
            Assert.NotNull(t.Id);
        }

        [Test]
        public void TestGetById()
        {
            var repo = new MongoRepository<Team>(_db);
            var team = new Team()
            {
                Name = "Tom"
            };
            repo.Save(team);
            var id = team.Id;


            var teamRefresh = repo.Find(id);

            Assert.AreEqual(teamRefresh.Name, "Tom");
        }


        [Test]
        public void TestRepository()
        {
            var teams = new List<Team>{new Team("Tom"), new Team("Olive")};
            var repo = new MongoRepository<Team>(_db);
            foreach (var t in teams)
            {
                repo.Save(t);
            }

            var teamsRefresh = repo.QueryAll();

            Assert.AreEqual(2 , teamsRefresh.Count());
        }
    }
}
