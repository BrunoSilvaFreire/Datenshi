using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Game
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerInputProvider Player;

        [SerializeField, HideInInspector] private Entity currentEntity;

        public float SlowMoTimeScale = 0.1F;
        public float SlowMoChangeDuration = 1;

        [ShowInInspector]
        public Entity CurrentEntity
        {
            get { return currentEntity; }
            set
            {
                if (currentEntity != null)
                {
                    currentEntity.RevokeOwnership();
                }

                currentEntity = value;
                currentEntity.RequestOwnership(Player);
            }
        }

        private void Start()
        {
            if (currentEntity.InputProvider != Player)
            {
                currentEntity.RevokeOwnership();
                currentEntity.RequestOwnership(Player);
            }
        }

        private void Update()
        {
            var p = Player;
            if (p == null)
            {
                return;
            }
        }
    }
}