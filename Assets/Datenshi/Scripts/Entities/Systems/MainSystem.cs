﻿using Datenshi.Scripts.Entities.Systems.Initialize;
using Datenshi.Scripts.Entities.Systems.Movement;

namespace Datenshi.Scripts.Entities.Systems {
    public class MainSystem : Feature {
        public MainSystem(Contexts contexts) {
            Add(new InitializeSystem(contexts, 0));
            Add(new GameSystem(contexts));
            Add(new MovementSystem(contexts));
        }
    }
}