using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW
{
    public class Intellegent : MonoBehaviour
    {
        [SerializeField] float _approachRange = 1;
        [SerializeField] bool _showRangeDebug = false;
        
        Battle      _battle             = null;
        Character   _lockedTarget       = null;
        SkillModel  _currentSkillModel  = null;
        
        void Awake()
        {
            ThisChar = GetComponent<Character>();
        }
        
        void Start()
        {
            _battle = Battle.Instance;
        }

        public Character ThisChar       { get; private set; } = null;
        public Character LockedTarget   => _lockedTarget;
        
        void Update()
        {
            Skill       skill       = ThisChar.Skill;
            Team        myTeam      = ThisChar.Team;
            Team        opTeam      = myTeam.Opposite();
            SkillModel  skillModel  = skill.AutoSkillModel;
            float       attackRange = skillModel.AttackRange;
            float       sqrAtkRange = attackRange * attackRange;

            _currentSkillModel = skillModel;
            
            if (_lockedTarget != null)
            {
                if (_lockedTarget.Health.IsAlive == false)
                {
                    _lockedTarget = null;
                }
                else
                {
                    if (IsInRange(_lockedTarget, _approachRange) == false)
                    {
                        _lockedTarget = null;
                    }
                }
            }
            
            if (_lockedTarget == null)
            {
                _lockedTarget = FindClosetTarget(_approachRange > attackRange ? _approachRange : attackRange);
            }

            if (_lockedTarget != null)
            {
                //in range attack
                if (IsInRange(_lockedTarget, attackRange))
                {
                    if (ThisChar.Graphic.IsAttacking == false && skillModel.IsReady)
                    {
                        ThisChar.Attack(skillModel, _lockedTarget);
                        skill.UseSkill(skillModel);
                    }
                }
                else if(_approachRange > 0)
                {
                    Vector3 direction = _lockedTarget.Position - ThisChar.Position;
                    
                    ThisChar.Movement.Move(direction.normalized);
                }
            }
        }

        bool IsInRange(Character target, float range)
        {
            float sqrRange  = range * range;
            float sqr       = Vector3.SqrMagnitude(ThisChar.Position - target.Position);

            return sqr <= sqrRange;
        }
        
        Character FindClosetTarget(float range)
        {
            Team        myTeam      = ThisChar.Team;
            Team        opTeam      = myTeam.Opposite();
            var         chars       = _battle.GetChars(opTeam);
            float       sqrRange    = range * range;
            float       minSqrDis   = float.MaxValue;
            Character   target      = null;

            for (int i = 0, n = chars.Count; i < n; ++i)
            {
                Character   ch          = chars[i];
                Health      health      = ch.Health;
                Team        otherTeam   = ch.Team;

                if (health.IsAlive && myTeam != otherTeam)
                {
                    float sqr = Vector3.SqrMagnitude(ThisChar.Position - ch.Position);

                    if (sqr < sqrRange && sqr < minSqrDis)
                    {
                        minSqrDis   = sqr;
                        target      = ch;
                    }
                }
            }

            return target;
        }
        
        void FindNearestTarget()
        {
            Team        myTeam      = ThisChar.Team;
            Team        opTeam      = myTeam.Opposite();
            var         chars       = _battle.GetChars(opTeam);

            for (int i = 0; i < chars.Count; ++i)
            {
                
            }
        }

        void OnDrawGizmos()
        {
            if (_showRangeDebug)
            {
                EditorUtility.GizmosDrawCircle(transform.position, _approachRange, Color.yellow);

                if (_currentSkillModel != null)
                {
                    EditorUtility.GizmosDrawCircle(transform.position, _currentSkillModel.AttackRange, Color.red);
                }
            }
        }
    }
}
