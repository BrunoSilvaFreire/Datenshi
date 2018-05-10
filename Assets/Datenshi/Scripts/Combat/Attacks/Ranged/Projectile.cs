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
        public uint Damage;
        public GameObject SpawnOnHit;
        public AudioClip Clip;
        public AudioClip Defense;
        public AudioSource Source;
        public GameObject[] ToDecouple;
        public GameObject OnDefended;
        private bool wasShot;

        public void Shoot(ICombatant shooter, ICombatant target) {
            Shoot(shooter, target.Center - (Vector2) transform.position);
            ProjectileShotEvent.Instance.Invoke(this, shooter, target);
        }

        public void Shoot(ICombatant shooter, Vector2 direction) {
            Owner = shooter;
            wasShot = true;
            velocity = direction;
            velocity.Normalize();
            velocity *= Speed;

            if (Source != null && Clip != null) {
                Source.PlayOneShot(Clip);
            }
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

                if (Owner.ShouldAttack(e)) {
                    e.Damage(e, Damage);
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

        public bool CanDefend(ICombatant entity) {
            return Owner != null && Owner.Relationship != entity.Relationship;
        }

        public void Defend(ICombatant entity, ref DamageInfo info) {
            velocity = Owner.Center - (Vector2) transform.position;
            Modify();
            Owner = entity;
            PlayDefendFX();
        }

        private void Modify() {
            velocity.Normalize();
            var g = GameResources.Instance;
            velocity *= g.DeflectSpeed;
            Damage = (uint) (Damage * g.DeflectDamageMultiply);
        }

        public bool CanPoorlyDefend(ICombatant entity) {
            return true;
        }

        public float MaxAngle;

        public void PoorlyDefend(ICombatant entity, ref DamageInfo info) {
            var angle = Random.value * MaxAngle - MaxAngle / 2 + Angle(velocity);
            velocity = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
            Modify();
            Owner = entity;
            PlayDefendFX();
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

        public DefenseType GetDefenseType() {
            return DefenseType.Deflect;
        }
    }
}