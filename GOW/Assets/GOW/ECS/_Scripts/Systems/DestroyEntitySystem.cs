using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

namespace GOW.ECS
{
    public class DestroyEntitySystem : ComponentSystem, ILateUpdate
    {
        public void LateUpdate()
        {
            float dt = Time.deltaTime;
            _world.ForEachEntity((int entity, DestroyEntity destroyEntity) =>
            {
                destroyEntity.delay -= dt;

                if (destroyEntity.delay <= 0)
                {
                    if (_world.Has<Transform>(entity))
                    {
                        Transform t = _world.Get<Transform>(entity);
                        Object.Destroy(t.gameObject);
                    }
                    
                    _world.DestroyEntity(entity);
                }
            });
        }
    }
}
