using System;
using System.Collections.Generic;
using Petanque.Model.Competitions;
using Petanque.Model.Teams;

namespace Petanque.Model.Tools.Extension
{
    public static class ToolsExtension
    {
        public static void Shuffle(this Competition competition)
        {
            Random rng = new Random();
            int n = competition.InitialTeams.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Team value = competition.InitialTeams[k];
                competition.InitialTeams[k] = competition.InitialTeams[n];
                competition.InitialTeams[n] = value;
            }
        }
    }
}
