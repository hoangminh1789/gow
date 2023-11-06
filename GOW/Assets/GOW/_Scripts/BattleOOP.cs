using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOW
{
    public class BattleOOP : Battle
    {
        [SerializeField] GameObject         _refAlly        = null;
        [SerializeField] List<GameObject>   _refEnemies     = new List<GameObject>();
        [SerializeField] bool               _culling        = true;
        [SerializeField] Vector2            _range          = Vector2.one;
        
        Waves       _waves      = null;
        Team        _allyTeam   = Team.Team1;
        
        protected override void Awake()
        {
            base.Awake();
            
            _waves = GetComponent<Waves>();
        }

        protected override void Start()
        {
            base.Start();

            StartCoroutine(eUpdate10());
        }

        protected override void Update()
        {
            base.Update();
            if (_state == BattleState.Initialize)
            {
                this.Initialize();
                _state = BattleState.Running;
            }
            else
            {
                this.CheckNextWave();
                if (this.IsLose())
                {
                    this.OnLose.Invoke();
                    _state = BattleState.Finish;
                }
            }
        }

        IEnumerator eUpdate10()
        {
            var wait = new WaitForSeconds(1.0f / 10);
            while (gameObject.activeInHierarchy)
            {
                this.Update10();
                yield return wait;
            }
        }

        void Update10()
        {
            if (_state != BattleState.Initialize)
            {
                if (_culling)
                {
                    this.Culling();
                }
            }
        }

        protected override void CreateAlly()
        {
            Character   ally        = CreateObj(_refAlly, Team.Team1);
         
            _virtualCamera.Follow   = ally.transform;
            _virtualCamera.LookAt   = ally.transform;
            _allyTeam               = Team.Team1;
            
            ally.gameObject.AddComponent<KeyInput>();
            ally.Health.OnHpChanged.AddListener( OnAlly_HPChanged );
            
            this.AddChar(ally);
        }

        public override void CreateEnemy(int amount)
        {
            Spawnpoint  sp      = GetRandomSpawnpoint();
                
            for (int i = 0; i < amount; ++i)
            {
                int         rand        = Random.Range(0, _refEnemies.Count);
                GameObject  refEnemy    = _refEnemies[rand];
                Character   enemy       = CreateObj(refEnemy, Team.Team2);
                Vector3     pos         = GameUtility.RandomAround(sp.transform.position, sp.Range);

                enemy.transform.position = pos;
                    
                this.AddChar(enemy);
            }
        }
        
        Character CreateObj(GameObject prefab, Team team)
        {
            GameObject  go      = Instantiate(prefab, _tranCharacters, false);
            Character   ch      = go.GetComponent<Character>();
            Vector2     rand    = Random.insideUnitCircle;
            
            go.transform.position = new Vector3(_range.x * rand.x, 0, _range.y * rand.y);
            
            go.SetActive(true);


            ch.Team = team;
            
            ch.OnDead.AddListener( OnCharacter_Dead );
            
            return ch;
        }

        void OnAlly_HPChanged(int damage, int hp, int maxHp)
        {
            this.OnPlayerHPChanged.Invoke(hp, maxHp);
        }

        void CheckNextWave()
        {
            Wave wave = GameUtility.GetNextWave(_time, _waves.AllWaves);

            if (wave != null)
            {
                this.CreateEnemy(wave.amount);
            }
        }
        
        void Culling()
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_mainCamera);

            for (int i = 0; i < _characters.Count; ++i)
            {
                Character   ch          = _characters[i];
                Renderer    render    = ch.Renderer;

                bool isInCamera = GeometryUtility.TestPlanesAABB(planes, render.bounds);
                
                ch.SetVisible(isInCamera);
            }
        }

        bool IsLose()
        {
            var chars = GetChars(_allyTeam);
            return chars == null || chars.Count == 0;
        }
    }
}