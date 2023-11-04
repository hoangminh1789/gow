using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW
{
    public class ProjectileMovement : MonoBehaviour
    {
        [SerializeField] float _speed = 1;
        
        Vector3     _direction      = Vector3.zero;
        Projectile  _projectile     = null;
        
        void Awake()
        {
            _projectile = GetComponent<Projectile>();
            
            _projectile.InitializedEvent.AddListener(OnInit);
        }

        void OnInit()
        {
            _direction          = _projectile.EndPosition - _projectile.StartPosition;
            
            _direction.Normalize();
        }
        
        void Update()
        {
            if (_projectile.CurrentState == Projectile.State.Runing)
            {
                transform.position += _speed * Time.deltaTime * _direction;

                if (Vector3.SqrMagnitude(_projectile.EndPosition - transform.position) < 0.1f)
                {
                    _projectile.Complete();
                }
            }
        }
    }
}
