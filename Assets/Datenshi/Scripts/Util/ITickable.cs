namespace Datenshi.Scripts.Util {
    public interface ITickable {
        void Tick();
    }
    public interface ITickable<in T> {
        void Tick(T value);
    }

    public interface IInitializable {
        void Init();
    }
}