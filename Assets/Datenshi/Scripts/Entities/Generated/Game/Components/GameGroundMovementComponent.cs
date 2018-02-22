//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Datenshi.Scripts.Entities.Components.Movement.GroundMovement groundMovement { get { return (Datenshi.Scripts.Entities.Components.Movement.GroundMovement)GetComponent(GameComponentsLookup.GroundMovement); } }
    public bool hasGroundMovement { get { return HasComponent(GameComponentsLookup.GroundMovement); } }

    public void AddGroundMovement(float newMaxSpeed, float newMaxJumpHeight, UnityEngine.AnimationCurve newAccelerationCurve, UnityEngine.AnimationCurve newDeaccelerationCurve, Datenshi.Scripts.Util.StateMachine.StateMachine<Datenshi.Scripts.Entities.Components.Movement.GroundState, GameEntity> newStateMachine, Datenshi.Scripts.Entities.Components.Movement.Controller2D newController, float newSpeedMultiplier, Datenshi.Scripts.Controller.IInputProvider newProvider) {
        var index = GameComponentsLookup.GroundMovement;
        var component = CreateComponent<Datenshi.Scripts.Entities.Components.Movement.GroundMovement>(index);
        component.MaxSpeed = newMaxSpeed;
        component.MaxJumpHeight = newMaxJumpHeight;
        component.AccelerationCurve = newAccelerationCurve;
        component.DeaccelerationCurve = newDeaccelerationCurve;
        component.StateMachine = newStateMachine;
        component.Controller = newController;
        component.SpeedMultiplier = newSpeedMultiplier;
        component.Provider = newProvider;
        AddComponent(index, component);
    }

    public void ReplaceGroundMovement(float newMaxSpeed, float newMaxJumpHeight, UnityEngine.AnimationCurve newAccelerationCurve, UnityEngine.AnimationCurve newDeaccelerationCurve, Datenshi.Scripts.Util.StateMachine.StateMachine<Datenshi.Scripts.Entities.Components.Movement.GroundState, GameEntity> newStateMachine, Datenshi.Scripts.Entities.Components.Movement.Controller2D newController, float newSpeedMultiplier, Datenshi.Scripts.Controller.IInputProvider newProvider) {
        var index = GameComponentsLookup.GroundMovement;
        var component = CreateComponent<Datenshi.Scripts.Entities.Components.Movement.GroundMovement>(index);
        component.MaxSpeed = newMaxSpeed;
        component.MaxJumpHeight = newMaxJumpHeight;
        component.AccelerationCurve = newAccelerationCurve;
        component.DeaccelerationCurve = newDeaccelerationCurve;
        component.StateMachine = newStateMachine;
        component.Controller = newController;
        component.SpeedMultiplier = newSpeedMultiplier;
        component.Provider = newProvider;
        ReplaceComponent(index, component);
    }

    public void RemoveGroundMovement() {
        RemoveComponent(GameComponentsLookup.GroundMovement);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherGroundMovement;

    public static Entitas.IMatcher<GameEntity> GroundMovement {
        get {
            if (_matcherGroundMovement == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.GroundMovement);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherGroundMovement = matcher;
            }

            return _matcherGroundMovement;
        }
    }
}
