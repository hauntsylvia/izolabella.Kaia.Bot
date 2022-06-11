using System.Reflection;

namespace Kaia.Bot.Objects.Util
{
    internal class BaseImplementationUtil
    {
        internal static List<T> GetItems<T>()
        {
            List<T> R = new();
            foreach (Type Ty in Assembly.GetCallingAssembly().GetTypes().Where(Ty => typeof(T).IsAssignableFrom(Ty) && !Ty.IsInterface && !Ty.IsAbstract && Ty.GetConstructor(Type.EmptyTypes) != null))
            {
                object? O = Activator.CreateInstance(Ty);
                if (O is not null and T M)
                {
                    R.Add(M);
                }
            }
            return R;
        }
    }
}
