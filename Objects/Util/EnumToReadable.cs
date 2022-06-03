using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Util
{
    internal class EnumToReadable
    {
        internal static string GetNameOfEnumType(Enum LType)
        {
            string DisplayAfter = Regex.Replace(LType.ToString(), "([A-Z])", " $1");
            if(LType.GetType() == typeof(Constants.Enums.LeaderboardTypes))
            {
                string CategoryDisplay = DisplayAfter.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries).First();
                DisplayAfter = DisplayAfter.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries).Last();
                string DisplayFull = $"[{CategoryDisplay}] {DisplayAfter}";
                return DisplayFull;
            }
            else
            {
                return DisplayAfter;
            }
        }
    }
}
