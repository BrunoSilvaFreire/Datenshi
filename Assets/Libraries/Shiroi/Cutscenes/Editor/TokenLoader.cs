﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Shiroi.Cutscenes.Editor.Config;
using Shiroi.Cutscenes.Tokens;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Shiroi.Cutscenes.Editor {
    [InitializeOnLoad]
    public static class TokenLoader {
        public static readonly Type TokenType = typeof(IToken);
        public static List<Type> KnownTokenTypes;

        private static void Reload() {
            KnownTokenTypes = new List<Type>();
            MappedToken.Clear();
        }

        static TokenLoader() {
            LoadTokens();
            EditorApplication.playModeStateChanged += OnPlayerStateChanged;
        }

        private static void LoadTokens() {
            Reload();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                RegisterAssembly(assembly);
            }
            AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoaded;
        }

        private static void OnPlayerStateChanged(PlayModeStateChange playModeStateChange) {
            if (playModeStateChange == PlayModeStateChange.EnteredEditMode) {
                LoadTokens();
            }
        }

        private static void OnAssemblyLoaded(object sender, AssemblyLoadEventArgs args) {
            RegisterAssembly(args.LoadedAssembly);
        }

        private static void RegisterAssembly(Assembly assembly) {
            ushort total = 0;
            foreach (var type in assembly.GetTypes()) {
                if (type == TokenType || !TokenType.IsAssignableFrom(type)) {
                    continue;
                }
                if (KnownTokenTypes.Contains(type)) {
                    continue;
                }
                var name = type.Name;
                KnownTokenTypes.Add(type);
                total++;
            }
            if (Configs.ShowDebug && total > 0) {
                Debug.LogFormat("[ShiroiCutscenes] Loaded '{0}' tokens from '{1}'", total, assembly);
            }
        }

        private static void Func(object userData) { }
    }
}