using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    [SerializeField] float _range = 2;
    
    void Start()
    {
        
    }

    public float Range => _range;
}
