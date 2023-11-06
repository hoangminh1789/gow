using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

namespace GOW.ECS
{
    public class WaveSystem : ComponentSystem, IInitialize, IUpdate
    {
        BattleECS       _battle = null;
        Configuration   _config = null;
        float           _time   = 0;
        List<Wave> _waves = new List<Wave>();

        public void Initialize()
        {
            _waves .AddRange( _config.waves );
        }

        public void Update()
        {
            _world.Get(_world.UniqueEntity, out BattleState state);
            
            if(state != BattleState.Running)
                return;

            _time += Time.deltaTime;
            
            Wave wave = GameUtility.GetNextWave(_time, _waves);

            if (wave != null)
            {
                _battle.CreateEnemy(wave.amount);
            }
        }
    }
}
