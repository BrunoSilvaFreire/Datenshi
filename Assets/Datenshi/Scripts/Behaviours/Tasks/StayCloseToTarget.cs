using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Entities;
using Lunari.Tsuki;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class StayCloseToTarget : Action {
        public SharedCombatant Target;
        public AINavigator Navigator;
        public MovableEntity Entity;
        public float DistanceThreshold;
        public float AllowedMargin;
        public DummyInputProvider InputProvider;

        public override TaskStatus OnUpdate() {
            var t = Target.Value as LivingEntity;
            if (t == null) {
                return TaskStatus.Failure;
            }

            var targetPos = Navigator.GetFavourablePosition(t);
            var d = Vector2.Distance(targetPos, Entity.Center);
            if (d < AllowedMargin) {
                var p = (DummyInputProvider) Entity.InputProvider;
                p.Reset();
                return TaskStatus.Success;
            }

            Navigator.SetTarget(targetPos);
            Navigator.Execute(Entity, InputProvider);
            return d < DistanceThreshold ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnDrawGizmos() {
            var t = Target.Value as LivingEntity;
            if (t == null) {
                return;
            }

            var targetPos = t.Center;
            var b = Navigator.transform.position;
            var dist = Vector2.Distance(targetPos, Navigator.transform.position);
            var c = dist > DistanceThreshold ? Color.red : Color.green;
            var msg = $"Distance {Navigator.name}-{t.AnimatorUpdater.gameObject.name}: {dist}/{DistanceThreshold}";
            //Debugging.DrawLabel(targetPos, msg);
            Debug.DrawLine(targetPos, b, c);
            Debugging.DrawWireCircle2D(b, DistanceThreshold, c);
        }
    }
}