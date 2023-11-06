using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

namespace GOW.ECS
{
    public class CheckResultSystem : ComponentSystem, IUpdate
    {
        BattleLayer _layer;
        
        public void Update()
        {
            var state = _world.Get<BattleState>(_world.UniqueEntity);

            if (state == BattleState.Running)
            {
                var allies = _world.All<Ally>();

                if (allies.Count == 0)
                {
                    _world.Replace(_world.UniqueEntity, BattleState.Finish);
                    _layer.OnLose();
                }
            }
        }
    }
}