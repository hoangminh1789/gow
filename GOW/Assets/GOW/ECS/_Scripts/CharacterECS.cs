using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW.ECS
{
    public class CharacterECS : MonoBehaviour, ICharacter
    {
        [SerializeField] Transform _centerAnchor;
        [SerializeField] Transform _weaponAnchor;
        
        void Start()
        {

        }
        
        public int          Entity          { get; set; }
        public Transform    Transform       => transform;
        public Vector3      Position        => transform.position;
        public Vector3      WeaponAnchor    => _weaponAnchor.position;
        public Vector3      CenterAnchor    => _centerAnchor.position;
        public Team         Team            { get; set; }

        public bool         IsAlive         { get; set; }
    }
}
