using izolabella.Discord.Objects.Structures.Discord.Commands;

namespace izolabella.Kaia.Bot.Objects.Discord.Commands.Bases;

public abstract class KaiaSubCommand : IzolabellaSubCommand, IKaiaCommand
{
    /// <summary>
    /// DO NOT CHANGE after first compilation with the command.
    /// </summary>
    public abstract string ForeverId { get; }

    public abstract List<GuildPermission> RequiredPermissions { get; }
}
