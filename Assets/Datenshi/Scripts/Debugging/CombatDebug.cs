using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Entities;

namespace Datenshi.Scripts.Debugging {
    public static class CombatDebug {
        public static void DrawCombatInfo(Entity entity, DebugInfo info) {
            info.AddInfo("Entity Target: " + entity.GetVariable(CombatVariables.EntityTarget));
            var l = entity as LivingEntity;
            if (l == null) {
                return;
            }

            info.AddInfo("Entity Strategy: " + l.DefaultAttackStrategy);
        }
    }
}