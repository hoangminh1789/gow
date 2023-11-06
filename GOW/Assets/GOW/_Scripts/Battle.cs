using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace GOW
{
    public class Battle : MonoBehaviour
    {
        [SerializeField] protected CinemachineVirtualCamera _virtualCamera  = null;
        [SerializeField] protected Transform                _tranCharacters = null;
        
        
        protected List<Spawnpoint>                  _spawnpoints    = new List<Spawnpoint>();
        protected Dictionary<Team, List<Character>> _charByTeam     = new Dictionary<Team, List<Character>>();
        protected List<Character>                   _characters     = new List<Character>();

        protected BattleState   _state          = BattleState.Initialize;
        protected Camera        _mainCamera     = null;
        protected int           _count          = 1;
        protected float         _time           = 0;
        protected int           _roundedTime    = 0;
        
        public static Battle Instance { get; private set; } = null;
        
        protected virtual void Awake()
        {
            Instance = this;

            var array = GetComponentsInChildren<Spawnpoint>();
            _spawnpoints.AddRange(array);
        }
        
        protected virtual void Start()
        {
        }

        public UnityEvent           OnCharacterChanged      { get; } = new UnityEvent();
        public UnityEvent<int, int> OnPlayerHPChanged       { get; } = new UnityEvent<int, int>();
        public UnityEvent<int>      OnTimeChanged           { get; } = new UnityEvent<int>();
        public UnityEvent           OnLose                  { get; } = new UnityEvent();
        public List<Character>      AllCharacters           => _characters;
        public Camera               MainCamera              => _mainCamera;
        
        protected virtual void Update()
        {
            _time += Time.deltaTime;

            if (_roundedTime != (int)_time)
            {
                _roundedTime = (int)_time;
                
                this.OnTimeChanged.Invoke( _roundedTime );
            }
        }

        protected Spawnpoint GetRandomSpawnpoint()
        {
            int rand = Random.Range(0, _spawnpoints.Count);
            return _spawnpoints[rand];
        }

        public void Initialize()
        {
            this.CreateAlly();
            
            _mainCamera = Camera.main;
        }

        protected virtual void CreateAlly()
        {
        }
        
        public virtual void CreateEnemy(int amount)
        {

        }
        
        public List<Character> GetChars(Team team)
        {
            if (_charByTeam.ContainsKey(team))
            {
                return _charByTeam[team];
            }
            else
            {
                List<Character> list = new List<Character>();
                _charByTeam[team] = list;

                return list;
            }
        }
        
        protected void AddChar(Character character)
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

        protected void OnCharacter_Dead(Character character)
        {
            this.RemoveChar(character);
            character.DestroyObject();
        }
    }
}
