using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace GOW
{
    public class Battle : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera _virtualCamera;
        [SerializeField] GameObject _refAlly;
        [SerializeField] GameObject _refEnemy;
        [SerializeField] List<GameObject> _refEnemies = new List<GameObject>();
        [SerializeField] Vector2 _range = Vector2.one;
        [SerializeField] [Range(0, 9)] int _count0 = 1;
        [SerializeField] [Range(0, 9)] int _count1 = 0;
        [SerializeField] [Range(0, 9)] int _count2 = 0;
        [SerializeField] Transform _tranCharacters = null;

        Camera          _mainCamera = null;
        Waves           _waves      = null;
        
        List<Spawnpoint>                    _spawnpoints    = new List<Spawnpoint>();
        Dictionary<Team, List<Character>>   _charByTeam     = new Dictionary<Team, List<Character>>();
        List<Character>                     _characters     = new List<Character>();
        
        int     _count          = 1;
        float   _time           = 0;
        int     _roundedTime    = 0;
        
        public static Battle Instance { get; private set; } = null;
        
        void Awake()
        {
            Instance = this;
            
            //_refAlly.SetActive(false);
            //_refEnemy.SetActive(false);

            _waves = GetComponent<Waves>();
            var array = GetComponentsInChildren<Spawnpoint>();
            _spawnpoints.AddRange(array);
        }
        
        void Start()
        {
            int         rand        = Random.Range(0, _refEnemies.Count);
            GameObject  refEnemy    = _refEnemies[rand];
            Character   ally        = CreateObj(_refAlly, Team.Team1);
            Character   enemy       = CreateObj(refEnemy, Team.Team2);

            _virtualCamera.Follow = ally.transform;
            _virtualCamera.LookAt = ally.transform;

            ally.gameObject.AddComponent<KeyInput>();
            
            this.AddChar(ally);
            this.AddChar(enemy);
            
            _mainCamera = Camera.main;
        }

        public UnityEvent OnCharacterChanged { get; } = new UnityEvent();
        public UnityEvent<int> OnTimeChanged { get; } = new UnityEvent<int>();
        public List<Character> AllCharacters => _characters;

        void Update()
        {
            _time += Time.deltaTime;

            if (_roundedTime != (int)_time)
            {
                _roundedTime = (int)_time;
                
                this.OnTimeChanged.Invoke( _roundedTime );
            }
            
            Waves.Wave wave = _waves.GetNextWave(_time);

            if (wave != null)
            {
                Spawnpoint  sp      = GetRandomSpawnpoint();
                int         amount  = wave.amount;
                
                for (int i = 0; i < amount; ++i)
                {
                    int         rand        = Random.Range(0, _refEnemies.Count);
                    GameObject  refEnemy    = _refEnemies[rand];
                    Character   enemy       = CreateObj(refEnemy, Team.Team2);
                    Vector3     pos         = this.RandomAround(sp.transform.position, sp.Range);
                    
                    enemy.transform.position = pos;
                    
                    this.AddChar(enemy);
                }
            }
            
            this.Culling();
        }

        Spawnpoint GetRandomSpawnpoint()
        {
            int rand = Random.Range(0, _spawnpoints.Count);
            return _spawnpoints[rand];
        }

        Vector3 RandomAround(Vector3 position, float range)
        {
            Vector2 circle = Random.insideUnitCircle;
            return new Vector3(position.x + circle.x * range, position.y, position.z + circle.y * range);
        }
        
        public List<Character> GetChars(Team team)
        {
            return _charByTeam[team];
        }
        
        void AddChar(Character character)
        {
            Team team = character.Team;
            List<Character> list = null;
            
            if (_charByTeam.TryGetValue(team, out list) == false)
            {
                list = new List<Character>();
                _charByTeam[team] = list;
            }
            
            list.Add(character);
            _characters.Add(character);
            
            this.OnCharacterChanged.Invoke();
        }

        void RemoveChar(Character character)
        {
            _characters.Remove(character);
            _charByTeam[character.Team].Remove(character);
            
            this.OnCharacterChanged.Invoke();
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

        void OnCharacter_Dead(Character character)
        {
            this.RemoveChar(character);
            character.DestroyObject();
        }

        void Culling()
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_mainCamera);

            for (int i = 0; i < _characters.Count; ++i)
            {
                Character   ch          = _characters[i];
                Renderer    renderer    = ch.Renderer;

                if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
                {
                    ch.Visible(true);
                }
                else
                {
                    ch.Visible(false);
                }
            }
        }
        
    }
}
