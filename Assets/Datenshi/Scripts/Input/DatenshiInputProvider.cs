using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Input {
    /// <summary>
    /// Representa uma fonte de input. Seja um player, ou AI.
    /// </summary>
    public abstract class DatenshiInputProvider : UPM.Input.InputProvider {
        public void GetInputVectorInject(out float x, out float y, out Vector2 input) {
            x = GetHorizontal();
            y = GetVertical();
            input = new Vector2(x, y);
        }

        public Vector2 GetInputVector() {
            return new Vector2(GetHorizontal(), GetVertical());
        }

        public abstract bool GetJump();

        public abstract bool GetJumpDown();

        public abstract bool GetAttack();

        public abstract bool GetDash();
        public abstract bool GetDefend();
        public abstract bool GetFocus();
        public abstract bool GetSubmit();
#if UNITY_EDITOR
        public const string DebugGroup = "Values";

        [ShowInInspector, FoldoutGroup(DebugGroup)]
        public float HorizontalValue => GetHorizontal();

        [ShowInInspector, FoldoutGroup(DebugGroup)]
        public float VerticalValue => GetVertical();

        [ShowInInspector, FoldoutGroup(DebugGroup)]
        public bool JumpValue => GetJump();
#endif
        public Vector2 GetLastValidInputVector(float defaultXDirection = 1) {
            var vec = GetInputVector();
            if (Mathf.Approximately(vec.x, 0)) {
                vec.x = defaultXDirection;
            }

            return vec;
        }
    }
}