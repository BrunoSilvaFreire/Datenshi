using System.Collections;
using Datenshi.Scripts.Audio;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game.Restart;
using Datenshi.Scripts.Graphics;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Singleton;
using Datenshi.Scripts.World;
using UnityEngine;

namespace Datenshi.Scripts.Game {
    public class GameController : Singleton<GameController> {
        public Checkpoint LastCheckpoint;
        public float DeathFXDuration = 2;
        public float DeathFXColorIntensity = 1;
        public float DeathFXJumpIntensity = 1;
        public float DeathFXRestartDuration = 2;
        public float DeathFXWaitDuration = 2;
        public AnimationCurve DesaturationCurve;
        public AnimationCurve DarkenCurve;
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
            var f = audio.ReverbFilter;
            f.enabled = true;
            var graphics = GraphicsSingleton.Instance;
            var glitch = graphics.Glitch;
            glitch.ColorDrift = DeathFXColorIntensity;
            glitch.ScanLineJitter = DeathFXJumpIntensity;
            var runtime = RuntimeResources.Instance;
            runtime.AllowPlayerInput = false;
            runtime.AllowAIInput = false;
            var bnw = graphics.BlackAndWhite;
            bnw.RequestService(DeathFXRestartDuration, DesaturationCurve, DarkenCurve, 2);
            float defaultDesaturate = bnw.DefaultDesaturationAmount, defaultDarken = bnw.DefaultDarkenAmount;
            bnw.DefaultDarkenAmount = 1;
            bnw.DefaultDesaturationAmount = 1;
            yield return new WaitForSeconds(DeathFXDuration + DeathFXWaitDuration);
            f.enabled = false;
            audio.RestartBGM();
            RestartAll();
            runtime.AllowPlayerInput = true;
            runtime.AllowAIInput = true;
            glitch.ColorDrift = 0;
            glitch.VerticalJump = 0;
            bnw.RequestService(DeathFXRestartDuration, ResaturationCurve, BrightenCurve, 2);
            bnw.DefaultDarkenAmount = defaultDarken;
            bnw.DefaultDesaturationAmount = defaultDesaturate;
        }

        private static void RestartAll() {
            foreach (var restartable in ObjectUtil.FindAll<IRestartable>()) {
                restartable.Restart();
            }
        }
    }
}