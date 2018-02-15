using Entitas;
using Entitas.Unity;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Components {
    [Game]
    public class VelocityComponent : IComponent {
        public Vector2 Velocity;
    }

    [Game]
    public class AnimatedComponent : IComponent, ISpawnPreview {
        public Animator AnimatorPrefab;

        public Texture2D GetPreviewTexture() {
            if (AnimatorPrefab == null) {
                return null;
            }
            var renderer = AnimatorPrefab.GetComponent<SpriteRenderer>();
            return renderer == null ? null : renderer.sprite.texture;
        }
    }

    [Game]
    public class ViewComponent : IComponent {
        public EntityLink View;
    }

    [Game]
    public class CharacterComponent : IComponent {
        public Character.Character Character;
    }
}