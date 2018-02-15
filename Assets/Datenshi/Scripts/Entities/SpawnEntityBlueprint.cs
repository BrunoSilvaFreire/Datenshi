using System;
using Datenshi.Scripts.Entities.Blueprints;
using Datenshi.Scripts.Util;
using DesperateDevs.Utils;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public class SpawnEntityBlueprint : MonoBehaviour {
        public EntityBlueprint Blueprint;

        private void Start() {
            var entity = Contexts.sharedInstance.game.CreateEntity();
            entity.ApplyBlueprint(Blueprint);
        }

#if UNITY_EDITOR
        private const byte PreviewAlpha = 127;

        [ShowInInspector, ReadOnly]
        private Texture2D preview;

        private Texture2D Preview {
            get {
                if (preview == null) {
                    return preview = FindPreview();
                }
                return preview;
            }
        }

        private Texture2D FindPreview() {
            if (Blueprint == null) {
                return null;
            }
            foreach (var component in Blueprint.Components) {
                var typeName = component.FullTypeName;
                var type = Type.GetType(typeName);
                if (type == null) {
                    continue;
                }
                if (!type.ImplementsInterface<ISpawnPreview>()) {
                    continue;
                }
                var entity = Contexts.sharedInstance.game.CreateEntity();
                var temp = ((ISpawnPreview) component.CreateComponent(entity)).GetPreviewTexture();

                entity.Destroy();
                var pixels = temp.GetPixels();
                for (var i = 0; i < pixels.Length; i++) {
                    var pixel = pixels[i];
                    var color = pixel;
                    color.a = PreviewAlpha;
                    pixels[i] = color;
                }
                var texture = new Texture2D(temp.width, temp.height);
                texture.SetPixels(pixels);
                texture.Apply(true);
                return texture;
            }
            return null;
        }

        private void OnDrawGizmos() {
            if (Preview == null || Blueprint == null) {
                return;
            }
            var texture = Preview;
            var position = Camera.current.WorldToScreenPoint(transform.position);
            var texelSize = texture.texelSize;
            var textureSize = new Vector2(texture.width, texture.height);
            var rect = new Rect(position, textureSize);
            Graphics.DrawTexture(rect, texture);
            GizmosUtil.DrawBox2DWire(transform.position, textureSize.Multiply(texelSize), Color.white);
        }
    }
#endif

    public interface ISpawnPreview {
        Texture2D GetPreviewTexture();
    }
}