using System;
using Datenshi.Scripts.Audio;
using Datenshi.Scripts.Game.Restart;
using UnityEngine;

namespace Datenshi.Scripts.FMOD {
    public class FMODRestarter : MonoBehaviour, IRestartable {
        public FMODParameterValue[] Values;

        public void Restart() {
            var instance = AudioManager.Instance.BGMSource.EventInstance;
            foreach (var parameterValue in Values) {
                instance.setParameterValue(parameterValue.Parameter, parameterValue.Value);
            }
        }
    }

    [Serializable]
    public struct FMODParameterValue {
        public string Parameter;
        public float Value;
    }
}