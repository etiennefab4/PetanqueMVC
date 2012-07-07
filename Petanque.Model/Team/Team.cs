using Petanque.Model.Repository;

namespace Petanque.Model.Team
{
    public class Team : AbstractMongoEntity
    {
        public string Name { get;  set; }
        public int GamePlayed { get; set; }
        public int Number { get; set; }
        public bool IsTeamToReplace { get; set; }

        public bool CanSendToCryingCompetetion
        {
            get
            {
                return GamePlayed <= 2;
            }
        }

        public Team(string name, bool isTeamToReplace, int number):this()
        {
            Name = name;
            IsTeamToReplace = isTeamToReplace;
            Number = number;
        }

        public Team()
        {
            GamePlayed = 0;
        }
    }
}