using Cinemachine;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Cinemachine {
    public class CinemachineWorldConfiner : MonoBehaviour {
        private void Start() {
            var w = World.Rooms.Game.World.Instance;
            CinemachineSingletons.Instance.PlayerConfiner.m_BoundingShape2D = w.GetOrAddComponent<CompositeCollider2D>();
        }
    }
}