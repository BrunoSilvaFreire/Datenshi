using Datenshi.Scripts.Controller;
using Datenshi.Scripts.Misc;
using Datenshi.Scripts.UI.Menus;
using Datenshi.Scripts.Util.StateMachine;
using Datenshi_Input_Constants;
using Entitas;

namespace Datenshi.Scripts.Entities.Components.Player {
    public static class PlayerVariables {
        public static readonly Variable<GameEntity> CurrentEntity = new Variable<GameEntity>("player.currentEntity", null);
        public static readonly Variable<PlayerController> Controller = new Variable<PlayerController>("player.controller", null);
    }

    public class PlayerComponent : IComponent {
        public PlayerController Controller;
        public StateMachine<PlayerState, PlayerComponent> StateMachine;
        public UICharacterSelectionMenu CharacterSelectionMenu;
    }

    public class NormalPlayerState : PlayerState {
        public override void OnExecute(StateMachine<PlayerState, PlayerComponent> stateMachine) {
            var component = stateMachine.Owner;
            var controller = component.Controller;
            component.CharacterSelectionMenu.Showing = controller.GetButtonDown(Action.SlowMotion);
        }
    }


    public class ChangingCharacterPlayerState : PlayerState {
        public override void OnExecute(StateMachine<PlayerState, PlayerComponent> stateMachine) {
            //Execute
        }
    }

    public class PlanningActionPlayerState : PlayerState {
        public override void OnExecute(StateMachine<PlayerState, PlayerComponent> stateMachine) {
            //Execute
        }
    }

    public abstract class PlayerState : State<PlayerState, PlayerComponent> { }
}