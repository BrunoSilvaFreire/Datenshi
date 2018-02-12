using Entitas;

namespace Datenshi.Scripts.Entities.Systems {
    public abstract class AbstractCollectorSystem<T> where T : class, IEntity {
        private readonly ICollector<T> collector;

        public AbstractCollectorSystem(ICollector<T> collector) {
            this.collector = collector;
        }

        public ICollector<T> Collector {
            get {
                return collector;
            }
        }

        public void Activate() {
            Collector.Activate();
        }

        public void Deactivate() {
            Collector.Deactivate();
        }

        public void Clear() {
            Collector.ClearCollectedEntities();
        }

        protected abstract void Execute(T entity);
    }


    public abstract class AbstractExecuteSystem<T> : AbstractCollectorSystem<T>, IExecuteSystem where T : class, IEntity {
        protected AbstractExecuteSystem(ICollector<T> collector) : base(collector) { }

        public void Execute() {
            foreach (var entity in Collector.collectedEntities) {
                Execute(entity);
            }
        }
    }
}