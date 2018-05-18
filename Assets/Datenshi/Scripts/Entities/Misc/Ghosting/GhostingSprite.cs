using System.Collections;
using Datenshi.Scripts.Util;
using DG.Tweening;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Misc.Ghosting {
    public class GhostingSprite : MonoBehaviour {
        public SpriteRenderer Renderer;
        public float Duration;

        public float TimeLeft {
            get;
            set;
        }

        private void Update() {
            if (!playing) {
                return;
            }

            var c = Renderer.color;
            c.a = TimeLeft / Duration;
            Renderer.color = c;
        }

        private bool playing;

        public void Setup(MovableEntity entity) {
            playing = true;
            TimeLeft = Duration;
            var r = entity.MiscController.MainSpriteRenderer;
            Renderer.sprite = r.sprite;
            Renderer.flipX = r.flipX;
            transform.position = entity.transform.position;
            transform.parent = null;
        }
    }
}