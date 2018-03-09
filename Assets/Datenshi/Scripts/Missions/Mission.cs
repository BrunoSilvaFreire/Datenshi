using System.Collections.Generic;
using Datenshi.Scripts.Missions.Objectives;
using Shiroi.Cutscenes;
using UnityEngine;

namespace Datenshi.Scripts.Missions {
    [CreateAssetMenu(menuName = "Datenshi/Missions/Mission")]
    public class Mission : ScriptableObject {
        public Cutscene OnStart;
        public Cutscene OnFinish;

        [SerializeField]
        private Objective[] objectives = new Objective[1];

        public IEnumerable<Objective> Objectives {
            get {
                return objectives;
            }
        }

        public Objective this[uint currentObjective] {
            get {
                return objectives[currentObjective];
            }
        }
    }
}