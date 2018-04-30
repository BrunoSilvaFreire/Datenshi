using Datenshi.Scripts.AI.Behaviour;
using Datenshi.Scripts.AI.Traits;
using Datenshi.Scripts.Debugging;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Util;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.Input {
    public class AIStateInputProvider : InputProvider {
        public BehaviourState CurrentState;

        public Entity Entity;

        [SerializeField]
        private Trait trait;

        public Trait Trait {
            get {
                return trait;
            }
        }

        public float Horizontal;
        public float Vertical;
        public bool Jump;
        public bool Attack;
        public bool Walk;
        public bool Dash;
        public bool Submit;
        public bool Defend;
        public bool ExecuteState = true;


        public override float GetHorizontal() {
            return Fetch(Horizontal);
        }

        private static T Fetch<T>(T horizontal) {
            return !RuntimeVariables.Instance.AllowPlayerInput ? default(T) : horizontal;
        }

        public override float GetVertical() {
            return Fetch(Vertical);
        }

        public override bool GetJump() {
            return Fetch(Jump);
        }

        public override bool GetJumpDown() {
            return Fetch(Jump);
        }

        public override bool GetAttack() {
            return Fetch(Attack);
        }

        public override bool GetWalk() {
            return Fetch(Walk);
        }

        public override bool GetDash() {
            return Fetch(Dash);
        }

        public override bool GetDefend() {
            return Fetch(Defend);
        }

        public override bool GetSubmit() {
            return Fetch(Submit);
        }

        private readonly DebugInfo info = new DebugInfo();
        private const float XOffset = 0.2F;

        public override void DrawGizmos(Entity entity) {
            info.Clear();
            info.CurrentDebugabble = CurrentState;
            CurrentState.DrawGizmos(this, entity, info);
            uint currentLine = 0;
            var pos = entity.Center;
            pos.y += entity.Hitbox.bounds.size.y / 2;
            var messages = info.Messages;
            var totalMessages = messages.Count;
            foreach (var pair in info.Messages) {
                totalMessages += pair.Value.Count + 1;
            }

            pos.y += totalMessages * DebugUtil.LineHeight;
            foreach (var pair in info.Messages) {
                DebugUtil.DrawLabel(pos, pair.Key.GetTitle(), currentLine);
                currentLine++;
                pos.x += XOffset;
                foreach (var s in pair.Value) {
                    DebugUtil.DrawLabel(pos, s, currentLine);
                    currentLine++;
                }

                pos.x -= XOffset;
            }
        }

        private void Start() {
            Entity.RevokeOwnership();
            Entity.RequestOwnership(this);
            if (trait == null) {
                trait = GetComponentInChildren<Trait>();
            }
        }

        private void Update() {
            if (CurrentState == null) {
                return;
            }

            if (ExecuteState) {
                CurrentState.Execute(this, Entity, info);
            }

            if (Trait != null) {
                Trait.Execute();
            }
        }

        public void Reset() {
            Vertical = 0;
            Horizontal = 0;
            Walk = false;
            Attack = false;
            Defend = false;
            Submit = false;
            Jump = false;
        }
    }
}