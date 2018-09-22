using Datenshi.Scripts.Data;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Movement;
using UnityEngine;

namespace Datenshi.Scripts.AI {
    public interface INavigable : IVariableHolder, IInputReceiver, IMovable {

        AINavigator AINavigator {
            get;
        }
    }
}