﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Graphics {
    [ExecuteInEditMode]
    public class BlackAndWhiteFX : StandaloneVisualFX {
        private const string ShaderName = "Datenshi/BlackAndWhiteShader";
        private const string DarkenPropertyName = "_DarkenAmount";
        private const string PropertyName = "_Amount";

        [ShowInInspector]
        public float Amount {
            get {
                return Material != null ? Material.GetFloat(PropertyName) : 0;
            }
            set {
                if (Material != null) {
                    Material.SetFloat(PropertyName, value);
                }
            }
        }

        [ShowInInspector]
        public float DarkenAmount {
            get {
                return Material != null ? Material.GetFloat(DarkenPropertyName) : 0;
            }
            set {
                if (Material != null) {
                    Material.SetFloat(DarkenPropertyName, value);
                }
            }
        }

        protected override string GetShaderName() {
            return ShaderName;
        }
    }
}