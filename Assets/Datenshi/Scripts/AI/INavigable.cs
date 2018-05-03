using Datenshi.Scripts.Data;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Movement;
using UPM.Motors;

namespace Datenshi.Scripts.AI {
    public interface INavigable : ILocatable, IVariableHolder, IMovable, IInputReceiver {
        AINavigator AINavigator {
            get;
        }
    }
}