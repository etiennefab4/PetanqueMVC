using Petanque.Model.Repository;

namespace Petanque.Model.Team
{
    public class Team : AbstractMongoEntity
    {
        public string Name { get;  set; }

        public bool IsFakeTeam { get; protected set; }

        public Team(string name)
        {
            Name = name;
            IsFakeTeam = false;
        }

        public Team()
        {
            IsFakeTeam = true;
        }
    }
}