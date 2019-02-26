using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Input {
    [Serializable]
    public class ConsumableInput {
        [SerializeField, ReadOnly]
        private bool active;

        public bool Peek() {
            return active;
        }

        public bool Consume() {
            var value = active;
            active = false;
            return value;
        }

        public void Set() {
            active = true;
        }
    }

    /// <summary>
    /// Representa uma fonte de input. Seja um player, ou AI.
    /// </summary>
    public abstract class DatenshiInputProvider : MonoBehaviour {
        public void GetInputVectorInject(out float x, out float y, out Vector2 input) {
            x = GetHorizontal();
            y = GetVertical();
            input = new Vector2(x, y);
        }

        public Vector2 GetInputVector() {
            return new Vector2(GetHorizontal(), GetVertical());
        }

        public abstract float GetHorizontal();
        public abstract float GetVertical();
        public abstract ConsumableInput GetJump();
        public abstract ConsumableInput GetAttack();
        public abstract ConsumableInput GetDash();
        public abstract bool GetDashing();
        public abstract bool GetFocus();
        public abstract bool GetSubmit();
        public abstract bool GetJumping();

        public Vector2 GetLastValidInputVector(float defaultXDirection = 1) {
            var vec = GetInputVector();
            if (Mathf.Approximately(vec.x, 0)) {
                vec.x = defaultXDirection;
            }

            return vec;
        }
    }
}