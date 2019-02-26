using System.Collections;
using Datenshi.Scripts.Entities;
using UnityEngine;
using BehaviorDesigner.Runtime;

namespace Datenshi.Scripts.Misc {
    public class BossFightController : MonoBehaviour {
        public MovableEntity Boss;
        public Behavior BehaviourTree;
        public string BossFightValidFight;
        public void Begin() {
            StartCoroutine(InitializeBossFight());
        }

        private IEnumerator InitializeBossFight() {
            var i = BossFightUIController.Instance;
            yield return i.InitializeUI(Boss.Character);
            BehaviourTree.SetVariableValue(BossFightValidFight, true);
        }
    }
}