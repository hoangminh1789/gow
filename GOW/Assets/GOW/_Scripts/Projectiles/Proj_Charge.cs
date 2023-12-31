using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW
{
    public class Proj_Charge : Projectile
    {
        Animator _animator = null;
        
        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        protected override void OnInit()
        {
            if (Attacker != null && Attacker.IsAlive)
            {
                transform.SetParent(Attacker.Transform, false);
                _graphic.SetActive(true);
            
                _animator.SetTrigger("play");
            }
        }
    }
}
