using System.Collections.Generic;

namespace Petanque.Web.Models
{
    public class TeamDto
    {
        public string CompetitionId { get; set; }
        public string Id { get; set; }
        public string Nom { get; set; }
        public int Number { get; set; }
    }

    public class CreateTeamDto
    {
        public TeamDto TeamDto { get; set; }
        public IEnumerable<TeamDto> TeamDtos { get; set; } 
    }
}