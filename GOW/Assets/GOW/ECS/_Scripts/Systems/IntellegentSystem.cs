using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

namespace GOW.ECS
{
    public class IntellegentSystem : ComponentSystem, IInitialize, IUpdate
    {
        public void Initialize()
        {

        }

        public void Update()
        {
            var allies = _world.All<Ally>();
            var enemies = _world.All<Enemy>();
            
            foreach (var ally in allies)
            {
                this.Update(ally, enemies);
            }
            
            foreach (var enemy in enemies)
            {
                this.Update(enemy, allies);
            }
        }

        void Update(int entity, IEntityCollection targetEntities)
        {
            _world.Get(entity, out CharacterECS attackerChar, out Skill attackerSkill, out Graphic attackerGraphic);
            _world.Get(entity, out Intellegent intellegent);

            MovePattern movePattern     = null;
            SkillModel  skillModel      = attackerSkill.AutoSkillModel;
            float       attackRange     = skillModel.AttackRange;
            float       approachRange   = intellegent.approachRange;
            int         lockedEntity    = _world.Get<LockedTarget>(entity).entity;
            ICharacter  lockedChar      = null;
            int         oldLockedEntity = lockedEntity;
            
            if (_world.Has<MovePattern>(entity))
            {
                movePattern = _world.Get<MovePattern>(entity);
            }

            if (lockedEntity > 0)
            {
                if (_world.Has<HP>(lockedEntity))
                {
                    if (_world.Get<HP>(lockedEntity).value <= 0)
                    {
                        lockedEntity = 0;
                    }
                    else
                    {
                        lockedChar = _world.Get<CharacterECS>(lockedEntity);
                        if (GameUtility.IsInRange(attackerChar, lockedChar, approachRange) == false)
                        {
                            lockedEntity = 0;
                        }
                    }
                }
                else
                {
                    lockedEntity = 0;
                }
            }

            if (lockedEntity == 0)
            {
                var closetTarget = FindClosetTarget(entity, targetEntities, attackRange);
                if (closetTarget != null)
                {
                    lockedEntity = closetTarget.Entity;
                    
                    var lockedTarget = new LockedTarget() { entity = lockedEntity };
                    
                    if (_world.Has<LockedTarget>(entity))
                    {
                        _world.Replace(entity, lockedTarget);
                    }
                    else
                    {
                        _world.Add(entity, lockedTarget);
                    }
                }
            }

            if (oldLockedEntity != lockedEntity)
            {
                _world.Replace(entity, new LockedTarget() { entity = lockedEntity });
            }
            
            if (attackerGraphic.IsAttacking == false)
            {
                if (CanAttack(attackerChar, attackerGraphic, attackerSkill, attackRange, skillModel, lockedChar))
                {
                    _world.CreateEntity(
                        new Attack()
                        {
                            attackerEntity  = entity,
                            targetEntity    = lockedEntity,
                            skillModel      = skillModel,
                        },
                        new DestroyEntity()
                    );

                    attackerSkill.UseSkill(skillModel);
                }
                else if (GameUtility.CanApproach(lockedChar, approachRange))
                {
                    if (GameUtility.ShouldApproach(attackerChar, lockedChar, attackRange))
                    {
                        Vector3 direction = lockedChar.Position - attackerChar.Position;

                        _world.CreateEntity(
                            new Move()
                            {
                                entity          = entity,
                                speedModifier   = 1,
                                direction       = direction.normalized
                            },
                            new DestroyEntity()
                        );
                    }
                } 
                else if (CanMoveFollowPattern(attackerChar, movePattern))
                {
                    Vector3 direction = movePattern.TargetPosition - attackerChar.Position;
                    
                    _world.CreateEntity(
                        new Move()
                        {
                            entity          = entity,
                            speedModifier   = 0.4f,
                            direction       = direction.normalized
                        },
                        new DestroyEntity()
                    );
                }
            }
        }

        bool CanMoveFollowPattern(ICharacter attackerChar, MovePattern movePattern)
        {
            if (movePattern != null && movePattern.IsReady)
            {
                float sqrRange = Vector3.SqrMagnitude(attackerChar.Position - movePattern.TargetPosition);

                if (sqrRange > 0.5f)
                {
                    return true;
                }
                else
                {
                    movePattern.MoveNext();
                    return false;
                }
            }

            return false;
        }
        
        bool CanAttack(ICharacter attackerChar, Graphic attackerGraphic, Skill attackerSkill, float attackRange, SkillModel model, ICharacter lockedTarget)
        {
            if (lockedTarget != null)
            {
                if (GameUtility.IsInRange(attackerChar, lockedTarget, attackRange))
                {
                    if (attackerGraphic.IsAttacking == false && attackerSkill.CanUseSkill && model.IsReady)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        CharacterECS FindClosetTarget(int myEntity, IEntityCollection otherEntities, float range)
        {
            Transform       myTran      = _world.Get<Transform>(myEntity);
            float           sqrRange    = range * range;
            float           minSqrDis   = float.MaxValue;
            CharacterECS    target      = null;

            foreach (var e in otherEntities)
            {
                _world.Get(e, out CharacterECS ch, out Transform t, out HP hp);

                if (hp.value > 0)
                {
                    float sqr = Vector3.SqrMagnitude(myTran.position - t.position);
                    
                    if (sqr < sqrRange && sqr < minSqrDis)
                    {
                        minSqrDis   = sqr;
                        target      = ch;
                    }
                }
            }
            
            return target;
        }
    }
}
