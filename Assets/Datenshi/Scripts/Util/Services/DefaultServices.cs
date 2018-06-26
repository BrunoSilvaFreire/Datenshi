using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Util.Services {
    public interface ITimedService {
        float TotalDuration {
            get;
        }

        float TimeLeft {
            get;
        }

        float Percentage {
            get;
        }
    }

    public sealed class TimedService<M> : Service<M>, ITimedService where M : IComparable<M> {
        public TimedService(M metadata, byte priority, float totalDuration) : base(metadata, priority) {
            TotalDuration = totalDuration;
            TimeLeft = totalDuration;
        }

        [ShowInInspector]
        public float TotalDuration {
            get;
        }

        [ShowInInspector]
        public float TimeLeft {
            get;
            private set;
        }

        public float Percentage => 1 - TimeLeft / TotalDuration;

        public override void Cancel() {
            TimeLeft = 0;
        }

        public override bool IsFinished() {
            return TimeLeft <= 0;
        }

        public override void Tick() {
            base.Tick();
            TimeLeft -= Time.unscaledDeltaTime;
        }
    }

    public sealed class IndefiniteService<M> : Service<M> where M : IComparable<M> {
        [ShowInInspector]
        private bool cancelled;

        public IndefiniteService(M metadata, byte priority) : base(metadata, priority) { }

        public override void Cancel() {
            cancelled = true;
        }

        public override bool IsFinished() {
            return cancelled;
        }

        public override int CompareTo(Service other) {
            var found = base.CompareTo(other);
            if (found != 0 || other == null) {
                return found;
            }

            return 0;
        }
    }
}