namespace Datenshi.Scripts.Input {
    public interface IInputReceiver {
        DatenshiInputProvider InputProvider {
            get;
        }

        bool RequestOwnership(DatenshiInputProvider provider);
        void RevokeOwnership();
    }
}