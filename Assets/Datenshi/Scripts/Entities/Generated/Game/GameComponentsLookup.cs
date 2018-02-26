//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentsLookupGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public static class GameComponentsLookup {

    public const int Animated = 0;
    public const int Character = 1;
    public const int DamageHistory = 2;
    public const int Health = 3;
    public const int Interaction = 4;
    public const int GroundMovement = 5;
    public const int GroundMovementBlueprint = 6;
    public const int Player = 7;
    public const int Velocity = 8;
    public const int View = 9;

    public const int TotalComponents = 10;

    public static readonly string[] componentNames = {
        "Animated",
        "Character",
        "DamageHistory",
        "Health",
        "Interaction",
        "GroundMovement",
        "GroundMovementBlueprint",
        "Player",
        "Velocity",
        "View"
    };

    public static readonly System.Type[] componentTypes = {
        typeof(Datenshi.Scripts.Entities.Components.AnimatedComponent),
        typeof(Datenshi.Scripts.Entities.Components.CharacterComponent),
        typeof(Datenshi.Scripts.Entities.Components.Health.DamageHistoryComponent),
        typeof(Datenshi.Scripts.Entities.Components.Health.HealthComponent),
        typeof(Datenshi.Scripts.Entities.Components.Input.InteractionComponent),
        typeof(Datenshi.Scripts.Entities.Components.Movement.GroundMovement),
        typeof(Datenshi.Scripts.Entities.Components.Movement.GroundMovementBlueprint),
        typeof(Datenshi.Scripts.Entities.Components.Player.PlayerComponent),
        typeof(Datenshi.Scripts.Entities.Components.VelocityComponent),
        typeof(Datenshi.Scripts.Entities.Components.ViewComponent)
    };
}
