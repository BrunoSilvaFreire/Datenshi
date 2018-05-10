using System.Collections;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.World.Rooms.Doors {
    public abstract class Door : AbstractRoomMember {
        public SpriteRenderer[] Renderers;
        public Color OpenColor;
        public Color CloseColor;
        public AudioClip OpenClip;
        public AudioClip CloseClip;
        public AudioSource Source;
        public Collider2D Collider;
        private Coroutine openRoutine;
        private Coroutine closeRoutine;

        public void Open() {
            CoroutineUtil.ReplaceCoroutine(ref openRoutine, this, DoOpen());
        }

        public void Close() {
            CoroutineUtil.ReplaceCoroutine(ref closeRoutine, this, DoClose());
        }

        private IEnumerator DoOpen() {
            yield return new WaitForSeconds(1);
            foreach (var r in Renderers) {
                r.color = OpenColor;
            }

            Source.PlayOneShot(OpenClip);
            yield return new WaitForSeconds(1);
            foreach (var r in Renderers) {
                Destroy(r.gameObject);
            }

            Destroy(Collider.gameObject);
            openRoutine = null;
        }

        private IEnumerator DoClose() {
            yield return new WaitForSeconds(1);
            foreach (var r in Renderers) {
                r.color = CloseColor;
            }

            Source.PlayOneShot(CloseClip);
            yield return new WaitForSeconds(1);
            foreach (var r in Renderers) {
                r.gameObject.SetActive(false);
            }

            Collider.gameObject.SetActive(false);
            closeRoutine = null;
        }
    }
}