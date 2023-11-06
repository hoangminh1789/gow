using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

namespace GOW.ECS
{
    public class DamageSystem : ComponentSystem, IInitialize
    {
        public void Initialize()
        {
            _world.All<Damage>().OnAdded.Bind(OnTakeDamage);
        }

        void OnTakeDamage(int entity)
        {
            _world.Get(entity, out Damage damage);
            _world.Get(damage.targetEntity, out HP hp, out CharacterECS ch, out Graphic graphic);

            int newHp = hp.value - damage.value;

            ch.IsAlive = newHp > 0;
            graphic.Hit();
            
            _world.Replace(damage.targetEntity, new HP() { value = newHp });

            if (newHp <= 0)
            {
                if (_world.Has<DestroyEntity>(damage.targetEntity) == false)
                {
                    _world.Add(damage.targetEntity, new DestroyEntity());
                }
            }
        }
    }
}
