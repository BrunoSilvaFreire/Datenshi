using Cinemachine;
using Datenshi.Scripts.FX;
using UnityEngine;

namespace Datenshi.Scripts.Cinemachine.Effects {
    [CreateAssetMenu(menuName = "Datenshi/Effects/CameraShake")]
    public class CameraShakeEffect : Effect {
        public CinemachineImpulseDefinition Definition;

        public override void Execute(Vector3 location) {
            var source = CinemachineSingletons.Instance.PlayerImpulseSource;
            source.m_ImpulseDefinition = Definition;
            source.GenerateImpulse(location);
        }
    }
}