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

        public void Move(Vector3 direction)
        {
            if (ThisChar.Graphic.IsAttacking == false)
            {
                transform.position += _speed * Time.deltaTime * direction;
                
                this.RotateToDir(direction);
                ThisChar.Graphic.Run();
            }
        }

        public void RotateToDir(Vector3 direction)
        {
            float angleY = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            transform.rotation = Quaternion.Euler(0, angleY, 0);
        }
    }
}