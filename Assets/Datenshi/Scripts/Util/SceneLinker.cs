using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Datenshi.Scripts.Util {
    [Serializable]
    public class SceneLinker : MonoBehaviour, IExposedPropertyTable {
        [SerializeField]
        private List<SceneLinkerAcessor> acessors;

        [SerializeField]
        private List<ObjectLink> objects = new List<ObjectLink>();

        public void SetReferenceValue(PropertyName id, Object value) {
            var link = FindOrCreateNewLink(id);
            link.Obj = value;
        }

        private ObjectLink? FindLink(PropertyName id) {
            foreach (var objectLink in objects) {
                if (objectLink.ID == id) {
                    return objectLink;
                }
            }

            return null;
        }

        private ObjectLink FindOrCreateNewLink(PropertyName id) {
            var found = FindLink(id);
            if (found != null) {
                return found.Value;
            }

            var link = new ObjectLink(id, null);
            objects.Add(link);
            return link;
        }

        public Object GetReferenceValue(PropertyName id, out bool idValid) {
            var found = FindLink(id);
            if (found != null) {
                idValid = true;
                return found.Value.Obj;
            }

            idValid = false;
            return null;
        }

        public void ClearReferenceValue(PropertyName id) {
            objects.RemoveAll(link => link.ID == id);
        }

        private void OnDrawGizmos() {
            foreach (var obj in objects) {
                Vector2 pos;
                var sceneObj = obj.Obj;
                if (sceneObj.AttempRetrievePosition(out pos)) {
                    GizmosUtil.DrawWireCircle2D(pos, 1, Color.magenta);
                }
            }
        }
    }

    [Serializable]
    public struct ObjectLink {
        [SerializeField]
        private PropertyName id;

        [SerializeField]
        private Object obj;

        public ObjectLink(PropertyName id, Object obj) {
            this.id = id;
            this.obj = obj;
        }

        public PropertyName ID => id;

        public Object Obj {
            get {
                return obj;
            }
            set {
                obj = value;
            }
        }
    }
}