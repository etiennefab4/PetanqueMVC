namespace Petanque.Model.Competition
{
    public class Node
    {
        public int DepthOfTheTree;
        public int Level;
        public Node TopNode;
        public Node BottomNode;
        public Team.Team Team;
        public Node ParentNode;
        public string CompetitionId { get; set; }
    }
}