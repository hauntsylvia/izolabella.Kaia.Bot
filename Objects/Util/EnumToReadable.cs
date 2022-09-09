using izolabella.Discord.Objects.Parameters;
using System.Text.RegularExpressions;

namespace izolabella.Kaia.Bot.Objects.Util;

public class EnumToReadable
{
    public static string GetNameOfEnumType(Enum LType)
    {
        string DisplayAfter = Regex.Replace(LType.ToString(), "([A-Z])", " $1");
        if (LType.GetType() == typeof(Constants.Enums.LeaderboardTypes))
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

    public static IzolabellaCommandParameter MakeChoicesFromEnum(string ParamName, string ParamDescription, Type EnumType)
    {
        List<IzolabellaCommandParameterChoices> Choices = new();
        foreach (Enum Filter in Enum.GetValues(EnumType))
        {
            Choices.Add(new(GetNameOfEnumType(Filter), (int)(object)Filter));
        }
        return new(ParamName, ParamDescription, ApplicationCommandOptionType.Integer, false)
        {
            Choices = Choices
        };
    }

    public static TEnum? GetEnumFromArg<TEnum>(IzolabellaCommandArgument? Argument)
    {
        if (Argument != null && Argument.Value is long RawFilter)
        {
            foreach (TEnum E in Enum.GetValues(typeof(TEnum)))
            {
                if ((int)(object)E == RawFilter)
                {
                    return E;
                }
            }
        }
        return default;
    }
}
