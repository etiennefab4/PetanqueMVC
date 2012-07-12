using System.Collections.Generic;
using Petanque.Model.Nodes;

namespace Petanque.Web.Models
{
    public class CompetitionDto
    {
        public bool IsCryingCompetion { get; set; }
        public string Id { get; set; }
        public string AffiliateCompetition { get; set; }
        public string Nom { get; set; }
        public IEnumerable<TeamDto> TeamDtos { get; set; }
        public Node Node { get; set; }
        public double Price { get; set; }
        public double BetByTeam { get; set; }
        public double PercentPotForMainCompetion { get; set; }
    }
}