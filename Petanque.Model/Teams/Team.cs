using Petanque.Model.Repository;

namespace Petanque.Model.Teams
{
    public class Team : AbstractMongoEntity
    {
        public double Gain { get; set; }
        public int WinInARow { get; set; }
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
            WinInARow = 0;
        }
    }
}