using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

namespace GOW.ECS
{
    public class BattleStateSystem : ComponentSystem, IInitialize
    {
        BattleECS _battle = null;
        
        public void Initialize()
        {
            _world.Add(_world.UniqueEntity, BattleState.Initialize);
            
            _battle.Initialize();
            
            _world.Replace(_world.UniqueEntity, BattleState.Running);
        }
    }
}