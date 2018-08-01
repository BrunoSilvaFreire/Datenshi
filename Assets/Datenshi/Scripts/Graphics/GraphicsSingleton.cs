using Datenshi.Scripts.Util.Singleton;
using UnityEngine.Rendering.PostProcessing;

namespace Datenshi.Scripts.Graphics {
    public class GraphicsSingleton : Singleton<GraphicsSingleton> {
        public BlackAndWhiteFX BlackAndWhite;
        public OverrideColorFX OverrideColor;
        public AnalogGlitch Glitch;
        public PostProcessVolume PostProcessVolume;
    }
}