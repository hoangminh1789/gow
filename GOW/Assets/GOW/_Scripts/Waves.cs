using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW
{
    
public class Waves : MonoBehaviour
{
    [Serializable]
    public class Wave
    {
        public float time;
        public int amount;
    }

    [SerializeField] List<Wave> _waves = new List<Wave>();
    
    void Start()
    {
        _waves.Sort((a, b) => a.time.CompareTo(b.time));
    }

    public Wave GetNextWave(float time)
    {
        for (int i = 0; i < _waves.Count; ++i)
        {
            if (_waves[i].time <= time)
            {
                Wave wave = _waves[i];

                _waves.RemoveAt(i);
                
                return wave;
            }
        }

        return null;
    }
    
    void Update()
    {
        
    }
}
}
