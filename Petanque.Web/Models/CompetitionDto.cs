using System.Collections.Generic;
using Petanque.Model.Competition;

namespace Petanque.Web.Models
{
    public class CompetitionDto
    {
        public string Id { get; set; }
        public string CryingCompetitionId { get; set; }
        public string Nom { get; set; }
        public IEnumerable<TeamDto> TeamDtos { get; set; }
        public Node Node { get; set; }
    }
}