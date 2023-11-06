using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GOW
{
    public class Projectile : MonoBehaviour
    {
        public enum State
        {
            Init, Runing, Completed, Dead
        }

        [SerializeField] protected Transform  _graphic        = null;
        [SerializeField] protected int        _damage         = 1;
        [SerializeField] protected float      _delayDestroy   = 0;
        
        protected State   _state      = State.Init;
        
        protected virtual void Awake()
        {
            _graphic.SetActive(false);
        }
        
        protected virtual void Start()
        {

        }

        public State CurrentState => _state;
        public UnityEvent InitializedEvent { get; } = new UnityEvent();
        public UnityEvent<ICharacter, int>  ExplodeEvent    { get; } = new UnityEvent<ICharacter, int>();
        public UnityEvent<Projectile>       DeadEvent       { get; } = new UnityEvent<Projectile>();
        
        public ICharacter    Attacker       { get; set; }
        public ICharacter    Target         { get; set; }
        public Vector3      StartPosition   { get; set; }
        public Vector3      EndPosition     { get; set; }
        public Team         Team            { get; set; }
        public bool         UsePool         { get; set; } = false;

        public void Reset()
        {
            _state = State.Init;
        }
        
        void Update()
        {
            if (_state == State.Init)
            {
                this.OnInit();
                this.InitializedEvent.Invoke();
                
                _state              = State.Runing;
            }
            else if (_state == State.Completed)
            {
                _state = State.Dead;
                
                this.DestroyObject();
            }
        }

        protected virtual void OnInit()
        {
            if (Attacker != null && Attacker.IsAlive)
            {
                transform.rotation  = Attacker.Transform.rotation;
            }
            transform.position  = StartPosition;
            
            _graphic.SetActive(true);
        }
        
        public void Complete()
        {
            _state = State.Completed;
        }

        public void Explode()
        {
            _state = State.Completed;
        }

        public void DestroyObject()
        {
            StartCoroutine(DelayDestroy());
        }

        IEnumerator DelayDestroy()
        {
            if (_delayDestroy > 0)
            {
                yield return new WaitForSeconds(_delayDestroy);
            }
            
            this.DeadEvent.Invoke(this);

            if (this.UsePool == false)
            {
                Destroy(gameObject);
            }
        }
        
        void OnTriggerEnter(Collider other)
        {
            //Debug.Log("OnTriggerEnter " + other.gameObject.name);
            ICharacter character = other.gameObject.GetComponent<ICharacter>();
            
            if (character != null && character.Team != this.Team)
            {
                this.ExplodeEvent.Invoke(character, _damage);
                this.Complete();
            }
        }
    }
}
