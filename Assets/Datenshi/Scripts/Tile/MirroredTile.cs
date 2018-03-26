#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Datenshi.Scripts.Tile {
    [CreateAssetMenu]
    public class MirroredTile : TileBase {
        public Sprite Sprite;
        public bool DefaultSpriteLeft = true;
        public bool PrefersLeft = true;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            tileData.sprite = Sprite;
            var pos = position;
            position.x += 1;
            var right = tilemap.GetTile(position);
            position.x -= 2;
            var left = tilemap.GetTile(position);
            bool flipped;
            if (left != null && right != null) {
                flipped = PrefersLeft;
            } else {
                var flippedX = DefaultSpriteLeft ? -1 : 1;
                if (right) {
                    flipped = DefaultSpriteLeft;
                } else if (left) {
                    flipped = !DefaultSpriteLeft;
                } else {
                    flipped = false;
                }
            }

            tileData.transform = Matrix4x4.TRS(
                Vector3.zero,
                Quaternion.Euler(0, flipped ? 180 : 0, 0),
                Vector3.one);
        }
#if UNITY_EDITOR

        private void OnValidate() {
            EditorUtility.SetDirty(this);
            SceneView.RepaintAll();
        }
#endif
    }
}