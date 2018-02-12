using Datenshi.Scripts.Entities.Systems;
using UnityEngine;

namespace Datenshi.Scripts.Controller {
    public class GameController : MonoBehaviour {
        private MainSystem mainSystem;

        private void Awake() {
            mainSystem = new MainSystem(Contexts.sharedInstance);
        }

        private void Update() {
            mainSystem.Execute();
        }
    }
}