using Datenshi.Scripts.Game.Restart;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.World.Rooms.Game {
    public class EventRestarter : MonoBehaviour, IRestartable {
        public UnityEvent OnRestart;

        public void Restart() {
            OnRestart.Invoke();
        }
    }
}