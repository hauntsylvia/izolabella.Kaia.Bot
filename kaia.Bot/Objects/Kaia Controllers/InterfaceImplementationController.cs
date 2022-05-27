﻿using System.Reflection;

namespace Kaia.Bot.Objects.CCB_Controllers
{
    internal class InterfaceImplementationController
    {
        internal static List<T> GetItems<T>()
        {
            List<T> R = new();
            foreach (Type Ty in Assembly.GetCallingAssembly().GetTypes())
            {
                if (typeof(T).IsAssignableFrom(Ty) && !Ty.IsInterface)
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
