using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW
{
    public class KeyInput : MonoBehaviour
    {
        void Awake()
        {
            ThisChar = GetComponent<Character>();
        }
        
        public Character ThisChar { get; private set; } = null;

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                ThisChar.Movement.Move(new Vector3(-1, 0, 0));
            } 
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                ThisChar.Movement.Move(new Vector3(1, 0, 0));
            }
            else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                ThisChar.Movement.Move(new Vector3(0, 0, 1));
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                ThisChar.Movement.Move(new Vector3(0, 0, -1));
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                SkillModel  skillModel      = ThisChar.Skill.ManualSkillModel;
                Character   lockedTarget    = ThisChar.Intellegent.LockedTarget;
                    
                if (skillModel != null)
                {
                    ThisChar.Attack(skillModel, lockedTarget);
                }
            }
        }
    }
}
