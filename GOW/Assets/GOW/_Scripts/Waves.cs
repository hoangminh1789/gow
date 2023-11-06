using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW
{

    public class Waves : MonoBehaviour
    {
        [SerializeField] List<Wave> _waves = new List<Wave>();

        void Start()
        {
            _waves.Sort((a, b) => a.time.CompareTo(b.time));
        }

        public List<Wave> AllWaves { get => _waves; }
    }
}
