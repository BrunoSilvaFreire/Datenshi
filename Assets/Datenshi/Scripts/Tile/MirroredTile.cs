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
            Debug.Log("Right = " + right + " @ " + position + " vs " + pos);
            position.x -= 2;
            var left = tilemap.GetTile(position);
            Debug.Log("Left = " + left + " @ " + position + " vs " + pos);
            int x;
            if (left != null && right != null) {
                x = PrefersLeft ? 1 : -1;
            } else {
                var flipped = DefaultSpriteLeft ? -1 : 1;
                if (right) {
                    x = -flipped;
                } else if (left) {
                    x = flipped;
                } else {
                    x = 0;
                }
            }

            tileData.transform = Matrix4x4.Scale(new Vector3(x, 1, 1));
        }
#if UNITY_EDITOR

        private void OnValidate() {
            EditorUtility.SetDirty(this);
            SceneView.RepaintAll();
        }
#endif
    }
}