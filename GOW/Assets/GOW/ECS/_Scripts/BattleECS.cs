using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ECS;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOW.ECS
{
    public class BattleECS : Battle
    {
        [Serializable]
        public class CharAsset
        {
            public string       id      ;
            public GameObject   prefab  ;
        }
        
        [SerializeField] GameObject     _refEnemy   = null;
        [SerializeField] GameObject     _refAlly    = null;
        [SerializeField] List<CharAsset> _refEnemies = new List<CharAsset>();
        
        World _world;
        Configuration _config;
        
        public void SetWorld(World world)
        {
            _world = world;
        }

        public void SetConfig(Configuration config)
        {
            _config = config;
        }
        
        protected override void CreateAlly()
        {
            CharacterData data = _config.chars.FirstOrDefault(x => x.id == "black_knight");
            
            if(data == null)
                return;
            
            GameObject      go          = Instantiate(_refAlly, _tranCharacters, false);
            Transform       tran        = go.transform;
            
            int             entity      = _world.CreateEntity();
            Renderer        render      = tran.GetComponentInChildren<Renderer>(true);
            Graphic         graphic     = tran.GetComponentInChildren<Graphic>(true);
            CharacterECS    character   = tran.GetComponent<CharacterECS>();
            Skill           skill       = tran.GetComponent<Skill>();

            character.Entity    = entity;
            character.Team      = Team.Team1;
                
            _world.Add(entity, tran);
            _world.Add(entity, new Ally());
            _world.Add(entity, render);
            _world.Add(entity, graphic);
            _world.Add(entity, character);
            _world.Add(entity, skill);
            
            _world.Add(entity, new MaxHP() { value = data.hp });
            _world.Add(entity, new HP() { value = data.hp });
            _world.Add(entity, new Speed() { value = data.speed });
            _world.Add(entity, new Intellegent(){approachRange = data.approachRange});
            _world.Add(entity, new LockedTarget() { entity = 0 });
            
            _virtualCamera.Follow = tran;
            _virtualCamera.LookAt = tran;
        }

        public override void CreateEnemy(int amount)
        {
            Spawnpoint      sp      = GetRandomSpawnpoint();
            
            for (int i = 0; i < amount; ++i)
            {
                int             rand    = Random.Range(0, _refEnemies.Count);
                CharAsset       asset   = _refEnemies[rand];
                CharacterData   data    = _config.chars.FirstOrDefault(x => x.id == asset.id);
                Transform       tran    = CreateObject(asset.prefab, _tranCharacters, data);
                Vector3         pos     = GameUtility.RandomAround(sp.transform.position, sp.Range);

                tran.position = pos;
            }
            
            Transform CreateObject(GameObject prefab, Transform parent, CharacterData data)
            {
                GameObject      go          = Instantiate(prefab, parent, false);
                Transform       tran        = go.transform;
            
                int             entity      = _world.CreateEntity();
                Renderer        render      = tran.GetComponentInChildren<Renderer>(true);
                Graphic         graphic     = tran.GetComponentInChildren<Graphic>(true);
                CharacterECS    character   = tran.GetComponent<CharacterECS>();
                Skill           skill       = tran.GetComponent<Skill>();
                MovePattern     movePattern = tran.GetComponent<MovePattern>();
                
                character.Entity    = entity;
                character.Team      = Team.Team2;
                
                _world.Add(entity, tran);
                _world.Add(entity, new Enemy());
                _world.Add(entity, render);
                _world.Add(entity, graphic);
                _world.Add(entity, character);
                _world.Add(entity, skill);
                _world.Add(entity, movePattern);
                _world.Add(entity, new MaxHP() { value = data.hp });
                _world.Add(entity, new HP() { value = data.hp });
                _world.Add(entity, new Speed() { value = data.speed });
                _world.Add(entity, new Intellegent(){approachRange = data.approachRange});
                _world.Add(entity, new LockedTarget() { entity = 0 });

                return tran;
            }
        }
    }
}