using Datenshi.Scripts.Data;
using Datenshi.Scripts.Movement;
using UPM.Motors;

namespace Datenshi.Scripts.AI {
    public interface INavigable : ILocatable, IVariableHolder, IMovable {
        AINavigator AINavigator {
            get;
        }
    }
}