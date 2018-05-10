using Datenshi.Scripts.Util.Singleton;

namespace Datenshi.Scripts.Graphics {
    public class GraphicsSingleton : Singleton<GraphicsSingleton> {
        public BlackAndWhiteFX BlackAndWhite;
        public OverrideColorFX OverrideColor;
        public AnalogGlitch Glitch;
    }
}