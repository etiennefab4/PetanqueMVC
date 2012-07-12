using System;
using Petanque.Model.Repository;
using Petanque.Model.Teams;

namespace Petanque.Model.Results
{
    public class Result : AbstractMongoEntity
    {
        public Team TeamWin { get; set; }
        public Team TeamLoose { get; set; }
        public DateTime Date { get; set; }
        public int DepthOfTheGame { get; set; }
        public bool GainProcessed { get; set; }

        public Result()
        {
            Date = DateTime.Now;
            GainProcessed = false;
        }
    }
}
