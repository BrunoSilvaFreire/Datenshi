using Datenshi.Scripts.Util.Singleton;
using UnityEngine;

namespace Datenshi.Scripts.Game {
    public class Singletons : Singleton<Singletons> {
        public AudioLowPassFilter LowPassFilter;
    }
}