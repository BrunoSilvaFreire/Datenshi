using System;
using System.Collections.Generic;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Debugging {
    public interface IDebugabble {
        string GetTitle();
    }

    public class DebugInfo {
        private static readonly Func<List<string>> Instantiator = () => new List<string>();
        private readonly Dictionary<IDebugabble, List<string>> messages = new Dictionary<IDebugabble, List<string>>();
        private IDebugabble currentDebugabble;

        public IDebugabble CurrentDebugabble {
            get {
                return currentDebugabble;
            }
            set {
                currentDebugabble = value;
                if (value == null) {
                    currentList = null;
                    return;
                }
                currentList = messages.GetOrPut(value, Instantiator);
            }
        }

        private List<string> currentList;

        public void Clear() {
            messages.Clear();
        }

        public void AddInfo(string message) {
            currentList.Add(message);
        }

        public Dictionary<IDebugabble, List<string>> Messages {
            get {
                return messages;
            }
        }
    }
}