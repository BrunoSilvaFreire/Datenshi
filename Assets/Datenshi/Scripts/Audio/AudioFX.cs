using UnityEngine;

namespace Datenshi.Scripts.Audio {
    [CreateAssetMenu(menuName = "Datenshi/Audio/AudioFX")]
    public class AudioFX : ScriptableObject {
        public AudioCategory Category;
        public float Volume;
        public string FMODEventName;
    }
}