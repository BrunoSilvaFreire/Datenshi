using System;
using System.Collections.Generic;
using Lunari.Tsuki;
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
            Debug.Log($"Looking for link with id {id}");
            foreach (var objectLink in objects) {
                Debug.Log(objectLink.ID);
                Debug.Log(id);
                var v = objectLink.ID == id;
                Debug.Log(v);
                Debug.Log("finish");
                if (v) {
                    Debug.Log("gg");
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
            Debug.Log($"id: {id}");
            var found = FindLink(id);
            Debug.Log($"Found @ '{id}'");
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
                if (sceneObj.AttemptRetrievePosition(out pos)) {
                    GizmosUtility.DrawWireCircle2D(pos, 1, Color.magenta);
                }
            }
        }

        private void Awake() {
            foreach (var sceneLinkerAcessor in acessors) {
                sceneLinkerAcessor.OnLoaded(this);
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