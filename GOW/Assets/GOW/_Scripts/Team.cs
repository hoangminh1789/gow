using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW
{
    public enum Team
    {
        Team1,
        Team2,
    }

    public static class TeamExt
    {
        public static Team Opposite(this Team team)
        {
            if (team == Team.Team1)
                return Team.Team2;

            return Team.Team1;
        }
    }
}
