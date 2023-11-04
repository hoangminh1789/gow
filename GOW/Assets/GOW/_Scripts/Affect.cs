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

        public Character Attacker   { get; set; }
        public Character Target     { get; set; }

    }
}
