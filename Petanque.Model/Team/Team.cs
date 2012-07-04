using Petanque.Model.Repository;

namespace Petanque.Model.Team
{
    public class Team : AbstractMongoEntity
    {
        public string Name { get;  set; }
        public int GamePlayed { get; set; }
        public bool IsTeamToReplace { get; set; }

        public bool CanSendToCryingCompetetion
        {
            get
            {
                return GamePlayed <= 2;
            }
        }

        public Team(string name, bool isTeamToReplace):this()
        {
            Name = name;
            IsTeamToReplace = isTeamToReplace;
        }

        public Team()
        {
            GamePlayed = 0;
        }
    }
}