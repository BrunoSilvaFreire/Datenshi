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
        public abstract bool GetSubmit();

    }
}