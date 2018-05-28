using Datenshi.Scripts.Data;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Combat.Attacks.Ranged {
    public class ProjectileShotEvent : UnityEvent<Projectile, ICombatant, ICombatant> {
        public static readonly ProjectileShotEvent Instance = new ProjectileShotEvent();
        private ProjectileShotEvent() { }
    }

    public class Projectile : Ownable<ICombatant>, IDefendable {
        [ShowInInspector, ReadOnly]
        private Vector2 velocity;

        public float Speed;
        public float DestroyDelay = 2;
        public GameObject SpawnOnHit;
        public AudioClip Clip;
        public AudioClip Defense;
        public AudioSource Source;
        public GameObject[] ToDecouple;
        public GameObject OnDefended;
        public float MaxAngle;

        [ShowInInspector, ReadOnly]
        private bool wasShot;

        [ShowInInspector, ReadOnly]
        private bool ownerDestroyed;

        [ShowInInspector, ReadOnly]
        public RangedAttack UsedAttack {
            get;
            private set;
        }

        public float DamageMultiplier {
            get;
            private set;
        } = 1;


        public void Shoot(RangedAttack attack, ICombatant shooter, ICombatant target) {
            Shoot(attack, shooter, target.Center - (Vector2) transform.position);
            ProjectileShotEvent.Instance.Invoke(this, shooter, target);
        }

        public void Shoot(RangedAttack attack, ICombatant shooter, Vector2 direction) {
            UsedAttack = attack;
            Owner = shooter;
            Owner.OnKilled.AddListener(OnOwnerKilled);
            wasShot = true;
            velocity = direction;
            velocity.Normalize();
            velocity *= Speed;

            if (Source != null && Clip != null) {
                Source.PlayOneShot(Clip);
            }
        }

        private void OnOwnerKilled() {
            ownerDestroyed = true;
            Owner.OnKilled.RemoveListener(OnOwnerKilled);
        }

        private void Update() {
            if (!wasShot) {
                return;
            }

            transform.position += (Vector3) velocity * Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            var col = other.collider;
            if (col.GetComponent<Projectile>() != null) {
                return;
            }

            var e = col.GetComponentInParent<ICombatant>();
            if (e != null) {
                if (e.Ignored) {
                    return;
                }

                var info = new DamageInfo(UsedAttack, DamageMultiplier, e, Owner);
                if (Owner.ShouldAttack(e)) {
                    e.Damage(Owner,ref info, this);
                    if (info.Canceled) {
                        return;
                    }
                } else {
                    return;
                }
            }

            Hit();
        }

        private void Hit() {
            foreach (var obj in ToDecouple) {
                obj.transform.parent = null;
                Destroy(obj, DestroyDelay);
            }

            if (SpawnOnHit != null) {
                SpawnOnHit.Clone(transform.position);
            }

            Destroy(gameObject);
        }

        private bool CanDefend(ICombatant entity) {
            return Owner != null && Owner.Relationship != entity.Relationship;
        }

        public bool CanAgressiveDefend(ICombatant combatant) {
            return CanDefend(combatant);
        }

        public float DoAgressiveDefend(ICombatant entity, ref DamageInfo info) {
            if (ownerDestroyed) {
                velocity = -velocity;
            } else {
                velocity = Owner.Center - (Vector2) transform.position;
            }

            Modify();
            Owner = entity;
            return UsedAttack.FocusConsumption;
        }

        public bool CanEvasiveDefend(ICombatant combatant) {
            return CanDefend(combatant);
        }

        public float DoEvasiveDefend(ICombatant combatant, ref DamageInfo info) {
            // TODO: Implement
            return UsedAttack.FocusConsumption;
        }


        public bool CanAutoDefend(ICombatant entity) {
            return CanDefend(entity);
        }

        public float DoAutoDefend(ICombatant entity, ref DamageInfo info) {
            // gg
            PlayDefendFX();
            info.Canceled = true;
            var angle = Random.value * MaxAngle - MaxAngle / 2 + Angle(velocity);
            velocity = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
            Owner = entity;
            Modify();
            //Destroy(gameObject);
            return UsedAttack.FocusConsumption;
        }

        private void Modify() {
            PlayDefendFX();
            velocity.Normalize();
            var g = GameResources.Instance;
            velocity *= g.DeflectSpeed;
            DamageMultiplier = g.DeflectDamageMultiply;
        }

        private void PlayDefendFX() {
            if (OnDefended != null) {
                OnDefended.Clone(transform.position);
            }

            if (Defense != null) {
                Source.PlayOneShot(Defense);
            }
        }

        public static float Angle(Vector2 vec) {
            if (vec.x < 0) {
                return 360 - Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg * -1;
            }

            return Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg;
        }
    }
}