namespace Datenshi.Scripts.Combat {
    public interface IDamageSource {
        uint GetDamage(IDamageable damageable);
    }
}