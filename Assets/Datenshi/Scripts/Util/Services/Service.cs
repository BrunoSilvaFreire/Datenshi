using System;
using Sirenix.OdinInspector;

namespace Datenshi.Scripts.Util.Services {
    public abstract class Service<M> : Service, IComparable<Service<M>> where M : IComparable<M> {
        public M Metadata {
            get;
        }

        private readonly ITickable tickableNoParam;
        private readonly ITickable<Service> serviceTickable;

        protected Service(M metadata, byte priority) : base(priority) {
            Metadata = metadata;
            tickableNoParam = metadata as ITickable;
            serviceTickable = metadata as ITickable<Service>;
        }

        public override void Init() {
            (Metadata as IInitializable)?.Init();
        }

        public override void Tick() {
            tickableNoParam?.Tick();
            serviceTickable?.Tick(this);
        }

        public int CompareTo(Service<M> other) {
            var found = this.CompareTo((Service) other);
            if (found != 0 || other == null) {
                return found;
            }

            if (ReferenceEquals(this, other)) {
                return 0;
            }

            var serviceComparison = base.CompareTo(other);
            return serviceComparison != 0 ? serviceComparison : Metadata.CompareTo(other.Metadata);
        }
    }

    public abstract class Service : ITickable, IInitializable, IComparable<Service> {
        public const byte DefaultPriority = 1;

        [ShowInInspector]
        public byte Priority {
            get;
            set;
        }

        protected Service(byte priority) {
            Priority = priority;
        }

        public abstract void Cancel();
        public abstract bool IsFinished();
        public abstract void Init();

        public abstract void Tick();

        public virtual int CompareTo(Service other) {
            if (ReferenceEquals(this, other)) {
                return 0;
            }

            if (ReferenceEquals(null, other)) {
                return 1;
            }

            return Priority.CompareTo(other.Priority);
        }

        public override string ToString() => $"Service({nameof(Priority)}: {Priority})";
    }
}