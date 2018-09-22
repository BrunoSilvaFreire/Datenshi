using Cinemachine;
using Shiroi.FX.Effects;
using Shiroi.FX.Effects.Requirements;
using Shiroi.FX.Features;
using UnityEngine;

namespace Datenshi.Scripts.Cinemachine.Effects {
    [CreateAssetMenu(menuName = "Datenshi/Effects/CameraShake")]
    [RequiresFeature(typeof(PositionFeature))]
    public class CameraShakeEffect : Effect {
        public CinemachineImpulseDefinition Definition;

        public override void Play(EffectContext context) {
            var source = CinemachineSingletons.Instance.PlayerImpulseSource;
            source.m_ImpulseDefinition = Definition;
            var location = context.GetRequiredFeature<PositionFeature>().Position;
            source.GenerateImpulse(location);
        }
    }
}