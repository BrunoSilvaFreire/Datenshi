//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Datenshi.Scripts.Entities.Components.VelocityComponent velocity { get { return (Datenshi.Scripts.Entities.Components.VelocityComponent)GetComponent(GameComponentsLookup.Velocity); } }
    public bool hasVelocity { get { return HasComponent(GameComponentsLookup.Velocity); } }

    public void AddVelocity(UnityEngine.Vector2 newVelocity) {
        var index = GameComponentsLookup.Velocity;
        var component = CreateComponent<Datenshi.Scripts.Entities.Components.VelocityComponent>(index);
        component.Velocity = newVelocity;
        AddComponent(index, component);
    }

    public void ReplaceVelocity(UnityEngine.Vector2 newVelocity) {
        var index = GameComponentsLookup.Velocity;
        var component = CreateComponent<Datenshi.Scripts.Entities.Components.VelocityComponent>(index);
        component.Velocity = newVelocity;
        ReplaceComponent(index, component);
    }

    public void RemoveVelocity() {
        RemoveComponent(GameComponentsLookup.Velocity);
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

    static Entitas.IMatcher<GameEntity> _matcherVelocity;

    public static Entitas.IMatcher<GameEntity> Velocity {
        get {
            if (_matcherVelocity == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Velocity);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherVelocity = matcher;
            }

            return _matcherVelocity;
        }
    }
}
