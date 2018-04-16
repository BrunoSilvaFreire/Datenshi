using Datenshi.Scripts.Misc;
using Datenshi.Scripts.Util.Singleton;
using Shiroi.Cutscenes;
using UnityEngine;

namespace Datenshi.Scripts.Game {
    public class Singletons : Singleton<Singletons> {
        public AudioLowPassFilter LowPassFilter;
    }
}