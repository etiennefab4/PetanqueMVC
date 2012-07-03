using System;
using Petanque.Model.Repository;

namespace Petanque.Model.Competition
{
    public class Result : AbstractMongoEntity
    {
        public Team.Team TeamWin { get; set; }
        public Team.Team TeamLoose { get; set; }
        public DateTime Date { get; set; }

        public Result()
        {
            Date = DateTime.Now;
        }
    }
}
