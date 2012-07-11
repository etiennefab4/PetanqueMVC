namespace Petanque.Web.Models
{
    public class ResultDto
    {
        public string Id { get; set; }
        public string NameTeamWin { get; set; }
        public string NameTeamLoose { get; set; }
        public bool CanDelete { get; set; }
    }
}