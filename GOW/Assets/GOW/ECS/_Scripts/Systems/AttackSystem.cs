using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

namespace GOW.ECS
{
    public class AttackSystem : ComponentSystem, IInitialize
    {
        GOPool _pool;
        
        public void Initialize()
        {
            _world.All<Attack>().OnAdded.Bind(OnAttack);
        }

        void OnAttack(int entity)
        {
            _world.Get(entity, out Attack attack);

            _world.Get(attack.attackerEntity, out CharacterECS attacker, out Graphic graphic);

            this.Attack(attack.attackerEntity, attacker, graphic, attack.skillModel, attack.targetEntity);
        }
        
        public void Attack(int attackerEntity, ICharacter attacker, Graphic attackerGraphic, SkillModel skillModel, int targetEntity)
        {
            Vector3         targetPosition  =  attacker.Transform.forward * 3;
            Vector3         targetCenter    =  attacker.Transform.forward * 3;
            CharacterECS    target          = null;
            
            if (targetEntity > 0)
            {
                _world.Get(targetEntity, out target);
                
                targetPosition  = target.Position;
                targetCenter    = target.CenterAnchor;
            }
            
            GameUtility.RotateToDirection(attacker.Transform, targetPosition - attacker.Position);

            if (skillModel.AnimTrigger.IsValid())
            {
                attackerGraphic.Attack(skillModel.AnimTrigger , () =>
                {
                    CreateProjectile(targetCenter, targetEntity);
                });
            }
            else
            {
                CreateProjectile(targetCenter, targetEntity);
            }
            
            void CreateProjectile(Vector3 endPosition, int targetEntity)
            {
                GameObject          prefab      = skillModel.Projectile;
                //Debug.Log("" + prefab.name);
                Projectile          projectile  = _pool.Get<Projectile>(prefab.name);
                GameObject          go          = prefab.gameObject; //GameObject.Instantiate(prefab);
                Transform           tran        = go.transform;
                //Projectile          projectile  = go.GetComponent<Projectile>();

                int entity = _world.CreateEntity();
            
                _world.Add(entity, tran);
                _world.Add(entity, projectile);
            
                //ProjectileMovement  movement    = go.GetComponent<ProjectileMovement>();

                projectile.Reset();
                
                projectile.Attacker         = attacker;
                projectile.StartPosition    = attacker.WeaponAnchor;
                projectile.EndPosition      = endPosition;
                projectile.Team             = attacker.Team;
                projectile.UsePool          = true;
                
                if (skillModel.Affect != null)
                {
                    GameObject prefabAffect = skillModel.Affect;
                    GameObject goAffect     = GameObject.Instantiate(prefabAffect);
                    Affect affect = goAffect.GetComponent<Affect>();

                    affect.Attacker = attacker;
                    affect.Target   = target;
                }
                
                projectile.ExplodeEvent.AddListener((ch, damage) =>
                {
                    if (ch != null)
                    {
                        _world.CreateEntity
                        (
                            new Damage()
                            {
                                targetEntity    = ((CharacterECS)ch).Entity,
                                value           = damage
                            },
                            new DestroyEntity()
                        );
                    }
                });
                
                projectile.DeadEvent.AddListener((proj) =>
                {
                    _pool.Return(proj);
                });
            }
        }
    }
}
