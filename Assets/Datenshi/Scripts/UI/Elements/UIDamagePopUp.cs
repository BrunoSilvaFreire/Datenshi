using System;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Elements {
    [Serializable]
    public class PopUpDisplayFinishedEvent : UnityEvent<UIDamagePopUp> { }

    public class UIDamagePopUp : MonoBehaviour {
        public AnimationCurve HeightCurve;
        public AnimationCurve ScaleCurve;
        public Transform MovableTransform;
        public Text Text;
        public float ScaleModifier = 0.01F;
        private float currentPosition;
        public float Duration;
        public PopUpDisplayFinishedEvent OnFinished;

        [ShowInInspector]
        public void Play() {
            currentPosition = 0;
        }

        public void Play(Vector3 position, uint damage, Color color, UnityAction<UIDamagePopUp> action) {
            Play();
            Text.text = damage.ToString();
            transform.position = position;
            Text.color = color;
            OnFinished.AddListener(action);
        }

        public void Stop() {
            currentPosition = Duration;
        }

        private void Update() {
            if (currentPosition >= Duration) {
                return;
            }

            if (currentPosition + Time.deltaTime >= Duration) {
                OnFinished.Invoke(this);
            }

            currentPosition += Time.deltaTime;
            UpdateTo(currentPosition / Duration);
        }

        public void UpdateTo(float time) {
            MovableTransform.localPosition = new Vector3(0, HeightCurve.Evaluate(time), 0);
            var scale = ScaleCurve.Evaluate(time) * ScaleModifier;
            MovableTransform.localScale = new Vector3(scale, scale, scale);
        }
    }
}