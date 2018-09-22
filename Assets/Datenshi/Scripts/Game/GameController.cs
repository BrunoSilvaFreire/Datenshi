using System.Collections;
using Datenshi.Scripts.Audio;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game.Restart;
using Datenshi.Scripts.Graphics;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Singleton;
using Datenshi.Scripts.World;
using Shiroi.FX.Utilities;
using UnityEngine;

namespace Datenshi.Scripts.Game {
    public class GameController : Singleton<GameController> {
        public Checkpoint LastCheckpoint;
        public float DeathFXDuration = 2;
        public ushort EffectPriority = 200;
        public AnimationCurve DeathFXColorDriftIntensity;
        public AnimationCurve DeathFXScanLineIntensity;
        public AnimationCurve DeathFXVerticalJumpIntensity;
        public AnimationCurve DeathFXHorizontalShakeIntensity;
        public AnimationCurve DeathFXDesaturate;
        public AnimationCurve DeathFXDarken;
        public float DeathFXRestartDuration = 2;
        public float DeathFXWaitDuration = 2;
        public AnimationCurve ResaturationCurve;
        public AnimationCurve BrightenCurve;
        public AudioFX DeathFX;

        private void Start() {
            CheckpointCollidedEvent.Instance.AddListener(OnCheckpointReached);
        }

        public void AttempSetCheckpoint(Checkpoint cp) {
            if (LastCheckpoint != null && cp.Priority < LastCheckpoint.Priority) {
                return;
            }

            LastCheckpoint = cp;
        }

        private void OnCheckpointReached(Checkpoint arg0, Collision2D collision2D) {
            var e = collision2D.collider.GetComponentInParent<Entity>();
            var ce = PlayerController.Instance.CurrentEntity;
            if (ce == null || ce != e) {
                return;
            }

            AttempSetCheckpoint(arg0);
        }

        private Coroutine restartRoutine;


        public void RestartGame() {
            CoroutineUtil.ReplaceCoroutine(ref restartRoutine, this, RestartFX());
        }

        private IEnumerator RestartFX() {
            var audio = AudioManager.Instance;
            audio.StopBGM();
            audio.PlayFX(DeathFX);
            var f = audio.ReverbFilter;
            if (f != null) {
                f.enabled = true;
            }

            var runtime = RuntimeResources.Instance;
            runtime.AllowPlayerInput = false;
            runtime.AllowAIInput = false;

            var graphics = GraphicsSingleton.Instance;
            var glitch = graphics.Glitch;
            var meta = new GlitchMeta(
                DeathFXScanLineIntensity,
                DeathFXVerticalJumpIntensity,
                DeathFXHorizontalShakeIntensity,
                DeathFXColorDriftIntensity
            );
            glitch.RegisterTimedService(DeathFXDuration, meta, priority: EffectPriority);
            var bnw = graphics.BlackAndWhite;
            bnw.RegisterTimedService(DeathFXDuration, new BlackAndWhiteMeta(DeathFXDesaturate, DeathFXDarken),
                priority: 200);
            float defaultDesaturate = bnw.DefaultDesaturationAmount, defaultDarken = bnw.DefaultDarkenAmount;
            yield return new WaitForSeconds(DeathFXDuration);
            bnw.DefaultDarkenAmount = 1;
            bnw.DefaultDesaturationAmount = 1;
            yield return new WaitForSeconds(DeathFXWaitDuration);
            if (f) {
                f.enabled = false;
            }

            audio.RestartBGM();
            RestartAll();
            runtime.AllowPlayerInput = true;
            runtime.AllowAIInput = true;
            bnw.RegisterTimedService(DeathFXRestartDuration, new BlackAndWhiteMeta(ResaturationCurve, BrightenCurve),
                priority: EffectPriority);
            bnw.DefaultDarkenAmount = defaultDarken;
            bnw.DefaultDesaturationAmount = defaultDesaturate;
        }

        private void RestartAll() {
            foreach (var restartable in ObjectUtil.FindAll<IRestartable>()) {
                restartable.Restart();
            }
        }
    }
}