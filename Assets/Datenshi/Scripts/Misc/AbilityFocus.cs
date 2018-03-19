using Datenshi.Scripts.Util.Singleton;
using DG.Tweening;
using UnityEngine;

namespace Datenshi.Scripts.Misc {
    public class AbilityFocus : Singleton<AbilityFocus> {
        public Material BlackAndWhite;
        private bool showing;
        public float Duration = 0.1F;

        public bool Showing {
            get {
                return showing;
            }
            set {
                showing = value;
                BlackAndWhite.DOKill();
                BlackAndWhite.DOFloat(showing ? 1 : 0, "_Amount", Duration);
            }
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest) {
            Graphics.Blit(src, dest, BlackAndWhite);
        }
    }
}