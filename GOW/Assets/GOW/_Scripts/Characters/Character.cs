using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GOW
{
    public class Character : MonoBehaviour, ICharacter
    {
        [SerializeField] Team _team = Team.Team1;
        [SerializeField] Transform _centerAnchor;
        [SerializeField] Transform _weaponAnchor;
        
        void Awake()
        {
            Graphic     = GetComponentInChildren<Graphic>(true);
            Renderer    = GetComponentInChildren<Renderer>(true);
            Movement    = GetComponent<Movement>();
            Skill       = GetComponent<Skill>();
            Intellegent = GetComponent<Intellegent>();
            Health      = GetComponent<Health>();
            
            Health.OnHpChanged.AddListener( OnHpChanged );
        }

        void Start()
        {

        }

        public Transform    Transform   => transform;
        public Graphic      Graphic     { get; private set; } = null;
        public Renderer     Renderer    { get; private set; } = null;
        public Movement     Movement    { get; private set; } = null;
        public Skill        Skill       { get; private set; } = null;
        public Intellegent  Intellegent { get; private set; } = null;
        public Health       Health      { get; private set; } = null;
        
        public Team Team
        {
            get => _team;
            set => _team = value;
        }
        
        public  bool IsAlive { get=> this.Health.IsAlive; set{} }

        public Vector3 Position         => transform.position;
        public Vector3 WeaponAnchor     => _weaponAnchor.position;
        public Vector3 CenterAnchor     => _centerAnchor.position;

        public UnityEvent<Character> OnDead { get; } = new UnityEvent<Character>();

        public void SetVisible(bool visible)
        {
            Graphic.SetActive( visible );
        }
        
        public void Attack(SkillModel skillModel, Character target)
        {
            Vector3 targetPosition = target != null ? target.Position : transform.forward * 3;
            Vector3 targetCenter = target != null ? target.CenterAnchor : transform.forward * 3;
            
            GameUtility.RotateToDirection(transform, targetPosition - this.Position);

            if (skillModel.AnimTrigger.IsValid())
            {
                this.Graphic.Attack(skillModel.AnimTrigger , () =>
                {
                    CreateProjectile(skillModel, targetCenter, target);
                });
            }
            else
            {
                CreateProjectile(skillModel, targetCenter, target);
            }

            void CreateProjectile(SkillModel model, Vector3 endPosition, Character target)
            {
                GameObject          prefab      = model.Projectile;
                GameObject          go          = Instantiate(prefab);
                Projectile          projectile  = go.GetComponent<Projectile>();
                //ProjectileMovement  movement    = go.GetComponent<ProjectileMovement>();

                projectile.Attacker         = this;
                projectile.Target           = target;
                projectile.StartPosition    = this.WeaponAnchor;
                projectile.EndPosition      = endPosition;
                projectile.Team             = this.Team;

                if (model.Affect != null)
                {
                    GameObject prefabAffect = model.Affect;
                    GameObject goAffect = Instantiate(prefabAffect);
                    Affect affect = goAffect.GetComponent<Affect>();

                    affect.Attacker = this;
                    affect.Target   = target;
                }
                
                projectile.ExplodeEvent.AddListener((ch, damage) =>
                {
                    if (ch != null)
                    {
                        ((Character)ch).TakeDamage( damage );
                    }
                });
            }
        }

        public void TakeDamage(int damage)
        {
            this.Health.TakeDamage(damage);
            this.Graphic.Hit();
        }

        public void DestroyObject()
        {
            Destroy(gameObject);
        }
        
        void OnHpChanged(int damage, int hp, int maxHp)
        {
            if (hp <= 0)
            {
                this.OnDead.Invoke( this );
            }
        }
    }
}
