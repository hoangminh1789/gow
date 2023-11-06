using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW.ECS
{
    public struct Move
    {
        public int entity;
        public Vector3 direction;
        public float speedModifier;
    }
}
