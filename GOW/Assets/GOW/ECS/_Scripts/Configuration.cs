using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW.ECS
{
    [Serializable]
    public class Configuration
    {
        public bool                 culling     = true;
        public List<Wave>           waves       = new List<Wave>();
        public List<CharacterData>  chars       = new List<CharacterData>();
    }
}
