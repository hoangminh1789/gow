using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ECS;
using UnityEngine;

namespace GOW.ECS
{
    public class UISystem : ComponentSystem, IUpdate
    {
        BattleLayer _layer;
        
        public void Update()
        {
            var state = _world.Get<BattleState>(_world.UniqueEntity);

            if (state == BattleState.Running)
            {
                var chars = _world.All<CharacterECS>();
                var ally = _world.All<Ally>().FirstOrDefault();
                
                if (ally > 0)
                {
                    _world.Get(ally, out HP hp, out MaxHP maxHp);
                    _layer.SetPlayerHp(hp.value, maxHp.value);
                }
                _layer.SetCharCount(chars.Count);
            }
        }
    }
}