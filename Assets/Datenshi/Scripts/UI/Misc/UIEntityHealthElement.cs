using Datenshi.Scripts.Entities;

namespace Datenshi.Scripts.UI.Misc {
    public class UIEntityHealthElement : UIHealthElement {
        public LivingEntity Entity;

        protected override LivingEntity GetEntity() {
            return Entity;
        }

        protected override void Start() {
            Entity.OnHealthChanged.AddListener(OnChange);
            base.Start();
        }

        private void OnChange() {
            UpdateBar();
        }
    }
}