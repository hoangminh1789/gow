using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GOW
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int _maxHp = 1;
        [SerializeField] int _hp    = 1;
        
        void Start()
        {

        }

        void Update()
        {

        }

        public UnityEvent<int, int, int> OnHpChanged { get; } = new UnityEvent<int, int, int>();
        public bool IsAlive => _hp > 0;
        
        public void TakeDamage(int damage)
        {
            _hp -= damage;

            if (_hp < 0)
            {
                _hp = 0;
            }
            
            this.OnHpChanged.Invoke(damage, _hp, _maxHp);
        }
    }
}
