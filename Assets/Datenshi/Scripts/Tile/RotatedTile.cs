using UnityEngine;
using UnityEngine.Tilemaps;

namespace Datenshi.Scripts.Tile {
    [CreateAssetMenu(menuName = "RotatedTile")]
    public class RotatedTile : TileBase {
        public Sprite Sprite;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            var t = (int) (Random.value * 4);
            var quaternion = Quaternion.Euler(0, 0, t * 90);
            tileData.transform = Matrix4x4.Rotate(quaternion);
            tileData.sprite = Sprite;
        }
    }
}