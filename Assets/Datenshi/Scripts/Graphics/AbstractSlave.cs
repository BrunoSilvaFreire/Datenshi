using UnityEngine;

namespace Datenshi.Scripts.Graphics {
    public abstract class AbstractSlave<T> : MonoBehaviour {
        public T Renderer;

        public virtual void Initialize(T renderer) {
            Renderer = renderer;
        }
    }
}