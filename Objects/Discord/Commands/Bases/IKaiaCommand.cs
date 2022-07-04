namespace Kaia.Bot.Objects.Discord.Commands.Bases
{
    public abstract class KaiaCommand : IzolabellaCommand
    {
        /// <summary>
        /// DO NOT CHANGE after first compilation with the command.
        /// </summary>
        public abstract string ForeverId { get; }

        public abstract List<GuildPermission> RequiredPermissions { get; }
    }
}
