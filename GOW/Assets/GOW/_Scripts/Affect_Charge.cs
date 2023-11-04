using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW
{
    public class Affect_Charge : Affect
    {
        [SerializeField] float _range = 1;

        Vector3 _targetPosition;
        Vector3 _attackerPosition;
        
        protected override void Start()
        {
            _targetPosition     = Target.Position;
            _attackerPosition   = Attacker.Position;
            
            StartCoroutine(IUpdate());
        }
        
        IEnumerator IUpdate()
        {
            yield return new WaitForSeconds(0.07f);

            if (Target.Health.IsAlive)
            {
                Vector3 direction   = (_targetPosition - _attackerPosition).normalized;

                for (int i = 1; i <= 10; ++i)
                {
                    Target.transform.position = _targetPosition + direction * _range / 10 * i;

                    yield return new WaitForSeconds(0.03f);
                }
            }

            Destroy(gameObject);
        }
    }
}
