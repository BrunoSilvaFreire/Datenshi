using System.Collections;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.World.Rooms;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Tokens {
    public class SpawnEntityToken : Token {
        public Entity Entity;
        public Vector2 Position;

        [NullSupported]
        public ExposedReference<Room> Room;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var e = Instantiate(Entity, Position, Quaternion.identity) as IRoomMember;
            var room = Room.Resolve(player);
            if (e != null && room != null) {
                room.AddMember(e);
            }

            yield break;
        }
    }
}