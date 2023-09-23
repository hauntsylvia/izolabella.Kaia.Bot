using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Commands.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.CommandConstrained;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds;

namespace izolabella.Kaia.Bot.Objects.Discord.Commands.Implementations.Guilds
{
    public class GuildSettingsCommand : KaiaCommand
    {
        public override string Name => "Guild Settings";

        public override string Description => "Change my guild's settings.";

        public override bool GuildsOnly => true;

        public override List<IzolabellaCommandParameter> Parameters => new()
    {
        new IzolabellaCommandParameter("Counting Channel", "The channel used for counting.", ApplicationCommandOptionType.Channel, new() { ChannelType.Text }, false),
    };

        public override List<IIzolabellaCommandConstraint> Constraints { get; } = new()
    {
        new WhitelistPermissionsConstraint(true, GuildPermission.ManageGuild)
    };

        public override List<GuildPermission> RequiredPermissions { get; } = new();

        public override string ForeverId => CommandForeverIds.GuildSettingsCommand;

        public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? CountingChannelArgument = Arguments.FirstOrDefault();

            if (Context.UserContext.User is SocketGuildUser SUser)
            {
                KaiaGuild Guild = new(SUser.Guild.Id);
                if (CountingChannelArgument != null && CountingChannelArgument.Value is IGuildChannel NewCountingChannelId)
                {
                    Guild.Settings.CountingChannelId = NewCountingChannelId.Id;
                    Guild.Settings = Guild.Settings;
                }
                await Context.UserContext.RespondAsync(text: Strings.EmbedStrings.Empty, ephemeral: false, embed: new GuildSettingsView(SUser.Guild.Name, Guild).Build());
            }
        }
    }
}