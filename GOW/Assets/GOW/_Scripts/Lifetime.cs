using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GOW
{
    public class Lifetime : MonoBehaviour
    {
        [SerializeField] float _time = 1;
        [SerializeField] UnityEvent OnCompleted = new UnityEvent();
        
        float _tick = 0;
        
        void Update()
        {
            if (_tick >= 0)
            {
                _tick += Time.deltaTime;

                if (_tick > _time)
                {
                    _tick = -1;
                    
                    this.OnCompleted.Invoke();
                }
            }
        }
    }
}
