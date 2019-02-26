using System.Collections;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Shiroi.Cutscenes;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Datenshi.Scripts.Master.Cutscenes {
    public class EnablePlayerAIToken : Token {
        public override IEnumerator Execute(CutscenePlayer player, CutsceneExecutor executor) {
            var m = PlayerController.GetOrCreateEntity<MovableEntity>();
            if (m == null) {
                yield break;
            }

            m.AutoDrive = true;
            m.OverrideInputProvider(m.GetComponentInChildren<DummyInputProvider>());
        }
    }
}