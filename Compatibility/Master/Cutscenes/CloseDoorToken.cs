using System.Collections;
using Datenshi.Scripts.World.Rooms.Game.Doors;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Datenshi.Scripts.Master.Cutscenes {
    public class CloseDoorToken : Token {
        public ExposedReference<AbstractDoor> Door;
        public bool Open = true;

        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var d = Door.Resolve(player);
            if (d == null) {
                yield break;
            }

            if (Open) {
                d.Open();
            } else {
                d.Close();
            }
        }
    }
}