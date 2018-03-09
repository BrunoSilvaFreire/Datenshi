using UnityEngine;

namespace Datenshi.Scripts.Entities.Input {
    /// <summary>
    /// Representa uma fonte de input. Seja um player, ou AI.
    /// </summary>
    public abstract class InputProvider : MonoBehaviour {
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

        public abstract bool GetJump();

        public abstract bool GetAttack();
        
        public abstract bool GetWalk();

        public abstract bool GetDash();

    }
}