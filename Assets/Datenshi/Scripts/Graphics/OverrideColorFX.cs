using DG.Tweening;
using DG.Tweening.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Graphics {
    [ExecuteInEditMode]
    public class OverrideColorFX : StandaloneVisualFX {
        public const string ShaderName = "Datenshi/OverrideColorShader";
        public const string ColorKey = "_Color";
        public const string AmountKey = "_Amount";


        [ShowInInspector]
        public Color Color {
            get {
                if (Material == null) {
                    return Color.black;
                }

                return Material.GetColor(ColorKey);
            }
            set {
                if (Material == null) {
                    return;
                }

                Material.SetColor(ColorKey, value);
            }
        }

        [ShowInInspector]
        public float Amount {
            get {
                if (Material == null) {
                    return 0;
                }

                return Material.GetFloat(AmountKey);
            }
            set {
                if (Material == null) {
                    return;
                }

                Material.SetFloat(AmountKey, value);
            }
        }

        public void DOAmount(float value, float duration) {
            this.DOKill();
            DOTween.To(() => Amount, v => Amount = v, value, duration);
        }

        protected override string GetShaderName() {
            return ShaderName;
        }
    }
}