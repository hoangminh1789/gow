using System;
using System.Collections;
using System.Collections.Generic;
using ECS;
using UnityEngine;

namespace GOW.ECS
{
    public class BattleController : MonoBehaviour
    {
        [SerializeField] BattleLayer    _layer  = null;
        [SerializeField] BattleECS      _battle = null;
        [SerializeField] Configuration  _config = null;
        [SerializeField] GOPool         _pool   = null;
        
        World _word;
        
        void Awake()
        {
            _word = new World();
            _word.AddSystem(
                new BattleStateSystem(),
                new IntellegentSystem(),
                new MovementSystem(),
                new WaveSystem(),
                new CullingSystem(),
                new InputSystem(),
                new AttackSystem(),
                new DamageSystem(),
                new DestroyEntitySystem(),
                new CheckResultSystem(),
                new UISystem()
                );
            
            _word.Inject(_config);
            _word.Inject(_battle);
            _word.Inject(_layer);
            _word.Inject(_pool);
            
            _battle.SetWorld(   _word   );
            _battle.SetConfig(  _config );
        }

        void Start()
        {
            _word.Initialize();
        }
        
        void Update()
        {
            _word.Update();
        }

        void LateUpdate()
        {
            _word.LateUpdate();
        }

        void OnDestroy()
        {
            _word.CleanUp();
        }
    }
}
