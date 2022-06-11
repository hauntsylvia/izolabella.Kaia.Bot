using System.Reflection;

namespace Kaia.Bot.Objects.KaiaControllers
{
    internal class InterfaceImplementationController
    {
        internal static List<T> GetItems<T>()
        {
            List<T> R = new();
            foreach (Type Ty in Assembly.GetCallingAssembly().GetTypes())
            {
                if (typeof(T).IsAssignableFrom(Ty) && !Ty.IsInterface && !Ty.IsAbstract && Ty.GetConstructor(Type.EmptyTypes) != null)
                {
                    object? O = Activator.CreateInstance(Ty);
                    if (O != null && O is T M)
                    {
                        R.Add(M);
                    }
                }
            }
            return R;
        }
    }
}
