using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ECS;
using UnityEngine;

namespace GOW.ECS
{
    public class InputSystem : ComponentSystem, IInitialize, IUpdate
    {

        public void Initialize()
        {
        }

        public void Update()
        {
            var state = _world.Get<BattleState>(_world.UniqueEntity);
            if (state != BattleState.Running)
                return;
            
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                var allyEntity = _world.All<Ally>().FirstOrDefault();
                if (allyEntity > 0)
                {
                    _world.CreateEntity(
                        new Move()
                        {
                            entity          = allyEntity,
                            speedModifier   = 1,
                            direction       = new Vector3(-1, 0, 0)
                        }, 
                    new DestroyEntity());
                }
            } 
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                var allyEntity = _world.All<Ally>().FirstOrDefault();
                if (allyEntity > 0)
                {
                    _world.CreateEntity(
                        new Move()
                        {
                            entity          = allyEntity,
                            speedModifier   = 1,
                            direction       = new Vector3(1, 0, 0)
                        },
                    new DestroyEntity());
                }
            }
            else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                var allyEntity = _world.All<Ally>().FirstOrDefault();
                if (allyEntity > 0)
                {
                    _world.CreateEntity(
                        new Move()
                        {
                            entity          = allyEntity,
                            speedModifier   = 1,
                            direction       = new Vector3(0, 0, 1)
                        },
                    new DestroyEntity());
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                var allyEntity = _world.All<Ally>().FirstOrDefault();
                if (allyEntity > 0)
                {
                    _world.CreateEntity(
                        new Move()
                        {
                            entity          = allyEntity,
                            speedModifier   = 1,
                            direction       = new Vector3(0, 0, -1)
                        },
                        new DestroyEntity()
                    );
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                var allyEntity      = _world.All<Ally>().FirstOrDefault();
                var lockedEntity    = 0;
                    
                if (allyEntity > 0)
                {
                    _world.Get(allyEntity, out Skill skill);

                    if (_world.Has<LockedTarget>(allyEntity))
                    {
                        lockedEntity = _world.Get<LockedTarget>(allyEntity).entity;
                    }
                    
                    SkillModel  skillModel      = skill.ManualSkillModel;
                        
                    if (skillModel != null && skillModel.IsReady)
                    {
                        _world.CreateEntity(
                            new Attack()
                            {
                                attackerEntity  = allyEntity,
                                targetEntity    = lockedEntity,
                                skillModel      = skillModel,
                            },
                            new DestroyEntity()
                        );
                    }
                }
            }
        }
    }
}
