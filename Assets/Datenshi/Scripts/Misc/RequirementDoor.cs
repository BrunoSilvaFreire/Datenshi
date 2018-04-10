using System.Collections;
using System.Collections.Generic;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Misc {
    public class RequirementDoor : MonoBehaviour {
        public List<LivingEntity> DeadRequired;
        public SpriteRenderer[] Renderers;
        public Color CompletedColor;
        public AudioSource Source;
        public AudioClip Clip;
        public Collider2D Collider;
        private void Start() {
            foreach (var livingEntity in DeadRequired) {
                var entity = livingEntity;
                livingEntity.OnKilled.AddListener(() => {
                    DeadRequired.Remove(entity);
                    Check();
                });
            }
        }

        private void Check() {
            if (!DeadRequired.IsEmpty()) {
                return;
            }

            StartCoroutine(Open());
        }

        private IEnumerator Open() {
            yield return new WaitForSeconds(1);
            foreach (var renderer in Renderers) {
                renderer.color = CompletedColor;
            }
            Source.PlayOneShot(Clip);
            yield return new WaitForSeconds(1);
            foreach (var renderer in Renderers) {
                Destroy(renderer.gameObject);
            }
            Destroy(Collider.gameObject);
        }
    }
}