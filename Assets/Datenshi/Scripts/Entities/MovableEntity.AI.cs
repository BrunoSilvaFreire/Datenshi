using Datenshi.Scripts.AI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public partial class MovableEntity {
        [SerializeField, BoxGroup(AIGroup)]
        private AINavigator aiNavigator;

        public AINavigator AINavigator => aiNavigator;

        [BoxGroup(AIGroup)]
        public bool AutoDrive;

        private void UpdateAI() {
            if (!AutoDrive || aiNavigator == null) {
//                Debug.Log("Auto drive @ " + AutoDrive + "/" + aiNavigator);
                return;
            }

            var p = InputProvider as DummyInputProvider;
            if (p == null) {
                //              Debug.Log("input provider");
                return;
            }

            aiNavigator.Execute(this, p);
        }
    }
}