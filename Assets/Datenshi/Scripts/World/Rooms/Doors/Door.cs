using System.Collections;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.World.Rooms.Doors {
    public class Door : AbstractDoor {
        public MeshRenderer[] Renderers;
        public Material OpenMaterial;
        public Material CloseMaterial;
        public AudioClip OpenClip;
        public AudioClip CloseClip;
        public AudioSource Source;
        public Collider2D Collider;
        private Coroutine openRoutine;
        private Coroutine closeRoutine;
        public bool DoDoorDelay = true;

        [ShowIf(nameof(DoDoorDelay))]
        public float DoorDelay = 1;

        public override void Open(bool silent = false) {
            CoroutineUtil.ReplaceCoroutine(ref openRoutine, this, DoOpen(silent));
        }

        public override void Close(bool silent = false) {
            CoroutineUtil.ReplaceCoroutine(ref closeRoutine, this, DoClose(silent));
        }

        private IEnumerator DoOpen(bool silent) {
            if (!silent && DoDoorDelay) {
                yield return new WaitForSeconds(DoorDelay);
            }

            SetMat(OpenMaterial);

            if (!silent) {
                Source.PlayOneShot(OpenClip);
                yield return new WaitForSeconds(1);
            }

            foreach (var r in Renderers) {
                r.gameObject.SetActive(false);
            }

            Collider.gameObject.SetActive(false);
            openRoutine = null;
        }

        private void SetMat(Material mat) {
            if (mat == null) {
                return;
            }

            foreach (var r in Renderers) {
                r.material = mat;
            }
        }

        private IEnumerator DoClose(bool silent) {
            if (!silent && DoDoorDelay) {
                yield return new WaitForSeconds(DoorDelay);
            }

            SetMat(CloseMaterial);

            if (!silent) {
                Source.PlayOneShot(CloseClip);
            }

            foreach (var r in Renderers) {
                r.gameObject.SetActive(true);
            }

            Collider.gameObject.SetActive(true);
            closeRoutine = null;
        }
    }
}