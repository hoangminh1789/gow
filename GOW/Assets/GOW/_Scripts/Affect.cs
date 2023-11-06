using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW
{
    public class Affect : MonoBehaviour
    {
        protected virtual void Start()
        {
        }

        public ICharacter Attacker   { get; set; }
        public ICharacter Target     { get; set; }

    }
}
