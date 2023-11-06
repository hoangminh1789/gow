using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW.ECS
{
    [Serializable]
    public class CharacterData
    {
        public string   id              = "";
        public int      hp              = 1;
        public int      speed           = 0;
        public float    approachRange   = 0;
    }
}
