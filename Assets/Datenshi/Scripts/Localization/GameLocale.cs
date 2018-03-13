using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Datenshi.Scripts.Localization {
    [CreateAssetMenu(menuName = "GameLocale")]
    public class GameLocale : SerializedScriptableObject {
        public LocaleNode Root;
    }

    [Serializable]
    public class LocaleNode {
        [OdinSerialize, ShowInInspector]
        public Dictionary<string, LocaleNode> Children;

        [OdinSerialize, ShowInInspector]
        public Dictionary<string, string> SimpleKeys;

        [OdinSerialize, ShowInInspector]
        public Dictionary<string, string[]> MultilineKeys;

        [OdinSerialize, ShowInInspector]
        public Dictionary<string, string[,]> DialogueKeys;
    }
}