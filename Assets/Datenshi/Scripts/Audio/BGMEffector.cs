using UnityEngine;

namespace Datenshi.Scripts.Audio {
    public class BGMEffector : MonoBehaviour {
        public string Parameter;
        public float Value;

        public void Effect() {
            AudioManager.Instance.BGMSource.EventInstance.setParameterValue(Parameter, Value);
        }
    }
}