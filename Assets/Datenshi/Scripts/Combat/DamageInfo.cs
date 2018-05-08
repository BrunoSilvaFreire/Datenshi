namespace Datenshi.Scripts.Combat {
    public class DamageInfo {
        public DamageInfo(ICombatant entity, uint damage) {
            User = entity;
            Damage = damage;
            Canceled = false;
            Hit = null;
        }

        public ICombatant Hit {
            get;
            set;
        }

        public ICombatant User {
            get;
            set;
        }

        public uint Damage {
            get;
            set;
        }

        public bool Canceled {
            get;
            set;
        }
    }
}