using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW.ECS
{
    public struct Attack
    {
        public int attackerEntity;
        public int targetEntity;
        public SkillModel skillModel;

    }
}
