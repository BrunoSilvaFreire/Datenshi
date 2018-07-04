using Datenshi.Scripts.Entities.Animation;
using Datenshi.Scripts.Graphics;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public partial class MovableEntity {
#if UNITY_EDITOR

        [ShowInInspector]
        public void Setup() {
            SetupBody();
        }

        private void SetupBody() {
            var objName = $"{name}_Body";
            var obj = GameObject.Find(objName);
            if (obj == null) {
                obj = new GameObject(objName);
            }

            ColorizableRenderer = obj.GetOrAddComponent<ColorizableRenderer>();
            obj.GetOrAddComponent<SpriteRenderer>();
            AnimatorUpdater.Animator = obj.GetOrAddComponent<Animator>();
            AnimatorUpdater = obj.GetOrAddComponent<MovableEntityUpdater>();
            obj.GetOrAddComponent<MovableEntityProxy>();
        }
#endif
    }
}