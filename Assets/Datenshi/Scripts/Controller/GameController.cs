using Datenshi.Scripts.Entities.Systems;
using UnityEngine;

namespace Datenshi.Scripts.Controller {
    public class GameController : MonoBehaviour {
        private MainSystem mainSystem;

        private void Awake() {
            mainSystem = new MainSystem(Contexts.sharedInstance);
            mainSystem.Initialize();
        }

        private void Update() {
            mainSystem.Execute();
        }

        private void OnDisable() {
            if (mainSystem != null) {
                mainSystem.TearDown();
            }
        }
    }
}