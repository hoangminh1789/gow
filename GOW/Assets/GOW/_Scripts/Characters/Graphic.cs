using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOW
{
    public class Graphic : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        float _animSpeed = 0;
        bool _attacking = false;
        
        void Start()
        {
            StartCoroutine(Idle());
        }

        public bool IsAttacking => _attacking;

        void Update()
        {
            if (_animSpeed > 0)
            {
                _animSpeed -= Time.deltaTime * 10;
                
                if (_animSpeed < 0)
                {
                    _animSpeed = 0;
                }
                
                _animator.SetFloat("speed", _animSpeed);
            }

            if (_attackCountdown > 0)
            {
                _attackCountdown -= Time.deltaTime;
                if (_attackCountdown <= 0)
                {
                    _attackCountdown    = 0;
                    _attacking          = false;
                    
                    _animator.SetBool("attacking", _attacking);
                }
            }
        }

        IEnumerator Idle()
        {
            float time = Random.Range(0.0f, 0.7f);
            yield return new WaitForSeconds(time);
            _animator.SetTrigger("idle");
        }

        Action _fire1Callback = null;
        float _attackCountdown = 0;
        
        public void Attack(string animTrigger, Action fireCallback)
        {
            _fire1Callback      = fireCallback;
            _attacking          = true;
            _attackCountdown    = 1.2f;
            _animator.SetTrigger(animTrigger);
            _animator.SetBool("attacking", _attacking);
        }

        public void Run()
        {
            _animSpeed += 0.3f;
            _animSpeed = _animSpeed > 1.0f ? 1.0f : _animSpeed;
        }

        public void Hit()
        {
            _attacking = false;
            _animator.SetBool("attacking", _attacking);
            _animator.SetTrigger("hit");
        }
        
        public void OnAnim_Attack()
        {
            _fire1Callback?.Invoke();
            _fire1Callback = null;
        }
        
        public void OnAnim_Attack_Finished()
        {
            _attacking = false;
            _animator.SetBool("attacking", _attacking);
        }
    }
}
