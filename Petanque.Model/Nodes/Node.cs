using Petanque.Model.Teams;

namespace Petanque.Model.Nodes
{
    public class Node
    {
        public int DepthOfTheTree;
        public int Level;
        public Node TopNode;
        public Node BottomNode;
        public Team Team;
        public Node ParentNode;
        public string CompetitionId;
    }
}