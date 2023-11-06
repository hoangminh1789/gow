using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] float _speed = 1;

        void Awake()
        {
            ThisChar = GetComponent<Character>();
        }
        
        void Start()
        {

        }

        public Character ThisChar { get; private set; } = null;

        public void Move(Vector3 direction, float speedModifier = 1.0f)
        {
            if (ThisChar.Graphic.IsAttacking == false)
            {
                transform.position += (speedModifier * _speed * Time.deltaTime) * direction;
                
                GameUtility.RotateToDirection(transform, direction);
                ThisChar.Graphic.Run();
            }
        }
    }
}
