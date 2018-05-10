using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Combat.Attacks.UI;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using UnityEngine;

namespace Datenshi.Scripts.Cinemachine {
    public class CinemachineCounterMiddlepoint : MonoBehaviour {
        private void Start() {
            CounterEvent.Instance.AddListener(OnDefend);
        }

        private void OnDefend(ICombatant t) {
            var e = PlayerController.Instance.CurrentEntity;
            var l = e as LivingEntity;
            Vector2 ePos;
            if (l != null) {
                ePos = l.Center;
            } else {
                ePos = e.transform.position;
            }

            transform.position = (ePos + t.Center) / 2;
        }
    }
}