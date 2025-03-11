using System.Collections.Generic;
using System.Reflection;

namespace Midnight;

public static class ReflectionHelper {
    public static IEnumerable<(System.Type, T)> IterateTypesWithAttribute<T>() where T : System.Attribute {
        System.Type attributeType = typeof(T);

        foreach (Assembly assembly in IterateAssemblies(attributeType.Assembly)) {
            foreach (System.Type type in assembly.GetTypes()) {
                if (!(type.GetCustomAttribute(attributeType, true) is T attribute)) {
                    continue;
                }

                yield return (type, attribute);
            }
        }
    }

    public static IEnumerable<System.Type> IterateDescendantsTypes<T>() {
        System.Type ancestorType = typeof(T);

        foreach (Assembly assembly in IterateAssemblies(ancestorType.Assembly)) {
            foreach (System.Type type in assembly.GetTypes()) {
                if (!type.IsSubclassOf(ancestorType)) {
                    continue;
                }

                yield return type;
            }
        }
    }

    public static IEnumerable<System.Type> IterateAssignablesTypes<T>() {
        System.Type ancestorType = typeof(T);

        foreach (Assembly assembly in IterateAssemblies(ancestorType.Assembly)) {
            foreach (System.Type type in assembly.GetTypes()) {
                if (!ancestorType.IsAssignableFrom(type)) {
                    continue;
                }

                yield return type;
            }
        }
    }

    public static List<System.Type> GetDescendantsTypes<T>() {
        List<System.Type> descendantsTypes = new List<System.Type>();
        System.Type ancestorType = typeof(T);

        foreach (Assembly assembly in IterateAssemblies(ancestorType.Assembly)) {
            foreach (System.Type type in assembly.GetTypes()) {
                if (!ancestorType.IsAssignableFrom(type)) {
                    continue;
                }

                descendantsTypes.Add(type);
            }
        }

        return descendantsTypes;
    }

    public static IEnumerable<Assembly> IterateAssemblies(Assembly assembly) {
        string assemblyName = assembly.GetName().Name;

        foreach (Assembly a in System.AppDomain.CurrentDomain.GetAssemblies()) {
            if (assembly.GetName().Name != assemblyName) {
                bool isReferenced = false;

                foreach (AssemblyName refAssemblyName in assembly.GetReferencedAssemblies()) {
                    if (refAssemblyName.Name == assemblyName) {
                        isReferenced = true;
                        break;
                    }
                }

                if (!isReferenced) {
                    continue;
                }
            }

            yield return a;
        }
    }
}
