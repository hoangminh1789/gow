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
        MovePattern _movePattern        = null;
        
        void Awake()
        {
            ThisChar = GetComponent<Character>();
        }
        
        void Start()
        {
            _battle         = Battle.Instance;
            _movePattern    = GetComponent<MovePattern>();
        }

        public Character ThisChar       { get; private set; } = null;
        public Character LockedTarget   => _lockedTarget;
        
        void Update()
        {
            Skill       skill       = ThisChar.Skill;
            SkillModel  skillModel  = skill.AutoSkillModel;
            float       attackRange = skillModel.AttackRange;

            _currentSkillModel = skillModel;
            
            if (_lockedTarget != null)
            {
                if (_lockedTarget.Health.IsAlive == false)
                {
                    _lockedTarget = null;
                }
                else
                {
                    if (GameUtility.IsInRange(ThisChar, _lockedTarget, _approachRange) == false)
                    {
                        _lockedTarget = null;
                    }
                }
            }
            
            if (_lockedTarget == null)
            {
                _lockedTarget = FindClosetTarget(_approachRange > attackRange ? _approachRange : attackRange);
            }

            if (ThisChar.Graphic.IsAttacking == false)
            {
                if (CanAttack(attackRange, skillModel))
                {
                    ThisChar.Attack(skillModel, _lockedTarget);
                    skill.UseSkill(skillModel);
                }
                else if (GameUtility.CanApproach(_lockedTarget, _approachRange))
                {
                    if (GameUtility.ShouldApproach(ThisChar, _lockedTarget, attackRange))
                    {
                        Vector3 direction = _lockedTarget.Position - ThisChar.Position;

                        ThisChar.Movement.Move(direction.normalized);
                    }
                } 
                else if (CanMoveFollowPattern())
                {
                    Vector3 direction = _movePattern.TargetPosition - ThisChar.Position;
                    
                    ThisChar.Movement.Move(direction.normalized, 0.4f);
                }
            }
        }

        bool CanAttack(float attackRange, SkillModel model)
        {
            if (_lockedTarget != null)
            {
                if (GameUtility.IsInRange(ThisChar, _lockedTarget, attackRange))
                {
                    if (ThisChar.Graphic.IsAttacking == false && ThisChar.Skill.CanUseSkill && model.IsReady)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        bool CanMoveFollowPattern()
        {
            if (_movePattern != null && _movePattern.IsReady)
            {
                float sqrRange = Vector3.SqrMagnitude(ThisChar.Position - _movePattern.TargetPosition);

                if (sqrRange > 0.5f)
                {
                    return true;
                }
                else
                {
                    _movePattern.MoveNext();
                    return false;
                }
            }

            return false;
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
