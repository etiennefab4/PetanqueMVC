using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Petanque.Model.Repository;

namespace Petanque.Model.Competition
{
    public class Result : AbstractMongoEntity
    {
        public Team.Team TeamWin { get; set; }
        public Team.Team TeamLoose { get; set; }
        public Competition Competition { get; set; }
        public DateTime Date { get; set; }
    }
}
