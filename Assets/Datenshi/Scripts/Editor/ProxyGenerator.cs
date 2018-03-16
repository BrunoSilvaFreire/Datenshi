using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Datenshi.Scripts.Editor {
    public static class ProxyMenu {
        [MenuItem("Assets/GenerateProxy")]
        private static void GenerateProxy() {
            foreach (var obj in Selection.objects) {
                Debug.Log("Selected obj = " + obj.GetType().FullName);
                if (!(obj is MonoScript)) {
                    return;
                }

                var file = AssetDatabase.GetAssetPath(obj);
                Debug.Log("Path = '" + file + "'");

                var t = (MonoScript) obj;
                ProxyGenerator.Generate(t.GetClass(), file);
            }
        }
    }

    public static class ProxyGenerator {
        public static void Generate(Type type, string directory) {
            var code = GenerateInitCode(type);
            foreach (var info in type.GetMethods()) {
                if (!IsValid(info)) {
                    continue;
                }

                var method = string.Format("public void {0}({1}) {{ \n{2}\n}}\n\n", info.Name, GetParameters(info),
                    GetMethodCall(info));
                code += method;
            }

            foreach (var field in type.GetFields()) {
                if (field.IsStatic) {
                    continue;
                }

                code += CreateSetter(field, field.FieldType);
            }

            foreach (var property in type.GetProperties()) {
                if (!property.CanWrite || property.GetSetMethod(false) == null) {
                    continue;
                }

                code += CreateSetter(property, property.PropertyType);
            }

            code += "}}";
            var dir = Path.GetDirectoryName(directory) + "/" + type.GetCompilableNiceName() + "Proxy.cs";
            Debug.Log(
                string.Format("Saving to '{0}'", dir));
            File.WriteAllText(dir, code);
            AssetDatabase.Refresh();
        }

        private static string CreateSetter(MemberInfo field, Type fieldFieldType) {
            return string.Format("public void Set{0}({1} value) {{\n    Target.{2} = value;\n}}\n\n",
                Capitalize(field.Name), fieldFieldType.Name, field.Name);
        }

        private static bool IsValid(MethodInfo info) {
            return !info.IsGenericMethod && info.IsPublic && info.GetReturnType() == typeof(void) &&
                   !info.IsSpecialName && !info.IsStatic &&
                   !IsUnityField(info);
        }

        private static bool IsUnityField(MethodInfo info) {
            var d = info.DeclaringType;
            return d == typeof(MonoBehaviour) || d == typeof(Object) || d == typeof(Behaviour) ||
                   d == typeof(Component);
        }

        private static string GenerateInitCode(Type type) {
            return "using UnityEngine;\n" +
                   "using System;" +
                   "namespace " + type.Namespace + " {\n" +
                   "public class " + type.GetCompilableNiceName() + "Proxy : MonoBehaviour {\n" +
                   "public " + type.Name + " Target;";
        }

        private static string GetMethodCall(MethodInfo info) {
            var param = string.Join(", ", (from p in info.GetParameters() select GetParameterCall(p)).ToArray());
            return string.Format("Target.{0}({1});", info.Name, param);
        }

        private static string GetParameterCall(ParameterInfo p) {
            var name = p.Name;
            if (p.ParameterType == typeof(Vector2)) {
                return "new Vector2(" + p.Name + "X, " + p.Name + "Y)";
            }

            return name;
        }

        private static string GetParameters(MethodInfo info) {
            return string.Join(", ", (from p in info.GetParameters() select GetParameterName(p)).ToArray());
        }

        private static string GetParameterName(ParameterInfo p) {
            var typeName = p.ParameterType.Name;
            var name = p.Name;
            if (p.ParameterType == typeof(Vector2)) {
                return "float " + p.Name + "X, float " + p.Name + "Y";
            }

            return typeName + " " + p.Name;
        }

        public static string Capitalize(this string source) {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            var letters = source.ToCharArray();
            letters[0] = char.ToUpper(letters[0]);
            return new string(letters);
        }
    }
}