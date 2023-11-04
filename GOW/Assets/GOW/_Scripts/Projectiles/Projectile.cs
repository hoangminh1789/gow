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
        
        public Character    Attacker        { get; set; }
        public Character    Target          { get; set; }
        public Vector3      StartPosition   { get; set; }
        public Vector3      EndPosition     { get; set; }
        public Team         Team            { get; set; }

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
            transform.rotation  = Attacker.transform.rotation;
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

        void ExplodeOnTarget(Character target)
        {
            target.TakeDamage( _damage );
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
            Destroy(gameObject);
        }
        
        void OnTriggerEnter(Collider other)
        {
            //Debug.Log("OnTriggerEnter " + other.gameObject.name);
            Character character = other.gameObject.GetComponent<Character>();

            if (character != null && character.Team != this.Team)
            {
                this.ExplodeOnTarget(character);
                this.Complete();
            }
        }
    }
}
