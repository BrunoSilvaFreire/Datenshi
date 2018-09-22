using System.Collections;
using Shiroi.Cutscenes;
using UnityEngine;

namespace Datenshi.Scripts.Misc {
    public class BossFightController : MonoBehaviour {
        public Character.Character BossCharacter;

        public void Begin() {
            StartCoroutine(InitializeBossFight());
        }

        private IEnumerator InitializeBossFight() {
            var i = BossFightUIController.Instance;
            yield return i.InitializeUI(BossCharacter);
        }
    }
}