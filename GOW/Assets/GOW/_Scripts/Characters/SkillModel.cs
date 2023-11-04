using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOW
{
    [Serializable]
    public class SkillModel
    {
        [SerializeField] string     _name           = "";
        [SerializeField] float      _interval       = 1;
        [SerializeField] float      _attackRange    = 1;
        [SerializeField] string     _animTrigger    = "attack1";
        [SerializeField] GameObject _projectile     = null;
        [SerializeField] GameObject _affect         = null;
        
        float _tick = 0;

        public bool         IsReady     => _tick >= _interval   ;
        public string       Name        => _name                ;
        public float        AttackRange => _attackRange         ;
        public string       AnimTrigger => _animTrigger         ;
        public GameObject   Projectile  => _projectile          ;
        public GameObject   Affect      => _affect              ;
        
        public void Tick(float dt)
        {
            _tick += dt;
        }

        public void DiffuseInterval()
        {
            _interval *= Random.Range(0.8f, 1.2f);
        }

        public void Use()
        {
            _tick = 0;
        }
    }
}
