using Datenshi.Scripts.Combat;
using Shiroi.Cutscenes.Triggers;

namespace Datenshi.Scripts.Cutscenes.Triggers {
    public class DefenseCutsceneTrigger : CutsceneTrigger {
        private void OnEnable() {
            GlobalDefenseEvent.Instance.AddListener(OnDefended);
        }


        private void OnDisable() {
            GlobalDefenseEvent.Instance.RemoveListener(OnDefended);
        }

        private void OnDefended(ICombatant combatant, DamageInfo damageInfo) {
            Trigger();
        }
    }
}