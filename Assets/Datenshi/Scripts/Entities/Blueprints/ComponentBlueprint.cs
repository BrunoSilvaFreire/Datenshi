using System;
using System.Collections.Generic;
using DesperateDevs.Utils;
using Entitas;

namespace Datenshi.Scripts.Entities.Blueprints {
    [Serializable]
    public class ComponentBlueprint {
        public int Index;
        public string FullTypeName;
        public SerializableMember[] Members;

        private Type type;
        private Dictionary<string, PublicMemberInfo> componentMembers;

        public ComponentBlueprint() { }

        public ComponentBlueprint(int index, IComponent component) {
            type = component.GetType();
            componentMembers = null;

            this.Index = index;
            FullTypeName = type.FullName;

            var memberInfos = type.GetPublicMemberInfos();
            Members = new SerializableMember[memberInfos.Count];
            for (int i = 0; i < memberInfos.Count; i++) {
                var info = memberInfos[i];
                Members[i] = new SerializableMember(
                    info.name,
                    info.GetValue(component)
                );
            }
        }

        public IComponent CreateComponent(IEntity entity) {
            if (type == null) {
                type = FullTypeName.ToType();

                if (type == null) {
                    throw new ComponentBlueprintException(
                        "Type '" + FullTypeName +
                        "' doesn't exist in any assembly!",
                        "Please check the full type name."
                    );
                }

                if (!type.ImplementsInterface<IComponent>()) {
                    throw new ComponentBlueprintException(
                        "Type '" + FullTypeName +
                        "' doesn't implement IComponent!",
                        typeof(ComponentBlueprint).Name +
                        " only supports IComponent."
                    );
                }
            }

            var component = entity.CreateComponent(Index, type);

            if (componentMembers == null) {
                var memberInfos = type.GetPublicMemberInfos();
                componentMembers = new Dictionary<string, PublicMemberInfo>(
                    memberInfos.Count
                );
                for (int i = 0; i < memberInfos.Count; i++) {
                    var info = memberInfos[i];
                    componentMembers.Add(info.name, info);
                }
            }

            for (int i = 0; i < Members.Length; i++) {
                var member = Members[i];

                PublicMemberInfo memberInfo;
                if (componentMembers.TryGetValue(member.name, out memberInfo)) {
                    memberInfo.SetValue(component, member.value);
                } else {
                    Console.WriteLine(
                        "Could not find member '" + member.name +
                        "' in type '" + type.FullName + "'!\n" +
                        "Only non-static public members are supported."
                    );
                }
            }

            return component;
        }
    }
}