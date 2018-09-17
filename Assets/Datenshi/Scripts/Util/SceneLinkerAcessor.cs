using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Util {
    public class SceneLinkerAcessor : MonoBehaviour {
        [ShowInInspector, ReadOnly]
        private readonly List<SceneLinker> loadedLinkers = new List<SceneLinker>();

        public void OnLoaded(SceneLinker linker) {
            loadedLinkers.Add(linker);
        }

        public Object this[string key] {
            get {
                foreach (var loadedLinker in loadedLinkers) {
                    bool valid;
                    var obj = loadedLinker.GetReferenceValue(key, out valid);
                    if (valid) {
                        return obj;
                    }
                }

                return null;
            }
        }
    }
}