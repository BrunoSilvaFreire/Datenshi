using UnityEngine;

namespace Datenshi.Scripts.Graphics {
    [ExecuteInEditMode]
    public class BlitFX : VisualFX {
        public Material Material;

        public override Material GetMaterial() {
            return Material;
        }
    }
}