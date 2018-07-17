using Cinemachine;
using UnityEngine;

namespace Datenshi.Scripts.Cinemachine {
    public class CinemachineProxy : MonoBehaviour {
        public CinemachineImpulseSource Source;

        public void DoScreenShake() {
            Debug.Log("Doing screen shake @ " + Source + " @ " + Source.transform.position);
            Source.GenerateImpulse();
        }
    }
}