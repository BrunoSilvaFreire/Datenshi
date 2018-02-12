//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Datenshi.Scripts.Entities.Components.Input.ControllableComponent controllable { get { return (Datenshi.Scripts.Entities.Components.Input.ControllableComponent)GetComponent(GameComponentsLookup.Controllable); } }
    public bool hasControllable { get { return HasComponent(GameComponentsLookup.Controllable); } }

    public void AddControllable(Datenshi.Scripts.Controller.IInputProvider newProvider) {
        var index = GameComponentsLookup.Controllable;
        var component = CreateComponent<Datenshi.Scripts.Entities.Components.Input.ControllableComponent>(index);
        component.Provider = newProvider;
        AddComponent(index, component);
    }

    public void ReplaceControllable(Datenshi.Scripts.Controller.IInputProvider newProvider) {
        var index = GameComponentsLookup.Controllable;
        var component = CreateComponent<Datenshi.Scripts.Entities.Components.Input.ControllableComponent>(index);
        component.Provider = newProvider;
        ReplaceComponent(index, component);
    }

    public void RemoveControllable() {
        RemoveComponent(GameComponentsLookup.Controllable);
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

    static Entitas.IMatcher<GameEntity> _matcherControllable;

    public static Entitas.IMatcher<GameEntity> Controllable {
        get {
            if (_matcherControllable == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Controllable);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherControllable = matcher;
            }

            return _matcherControllable;
        }
    }
}
