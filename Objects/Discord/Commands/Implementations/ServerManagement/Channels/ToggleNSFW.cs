using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Commands.Bases;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.Channels;

namespace izolabella.Kaia.Bot.Objects.Discord.Commands.Implementations.ServerManagement.Channels;

public class ToggleNSFW : KaiaCommand
{
    public override string ForeverId => CommandForeverIds.ToggleNSFW;

    public override List<GuildPermission> RequiredPermissions => new()
    {
        GuildPermission.ManageChannels
    };

    public override string Name => "Toggle NSFW";

    public override string Description => "Toggles this channel's NSFW property on or off.";

    public override bool GuildsOnly => true;

    public override List<IzolabellaCommandParameter> Parameters => new()
    {
        new("NSFW", "Whether this channel should be marked NSFW.", ApplicationCommandOptionType.Boolean, false),
        new("Channel", "The channel to change the property of.", ApplicationCommandOptionType.Channel, false)
    };

    public override List<IIzolabellaCommandConstraint> Constraints => new()
    {
        new WhitelistPermissionsConstraint(false, GuildPermission.ManageChannels)
    };

    public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
    {
        if (Context.UserContext.User is SocketGuildUser User)
        {
            IzolabellaCommandArgument? Ch = Arguments.FirstOrDefault(A => A.Name == "channel");

            IGuildChannel? Channel = Ch != null && Ch.Value is IGuildChannel A ? A : User.Guild.GetChannel(Context.UserContext.Channel.Id);
            if (Channel is not null and SocketTextChannel TextChannel)
            {
                bool NewNSFW = Arguments.FirstOrDefault(A => A.Name == "nsfw")?.Value is bool B ? B : !TextChannel.IsNsfw;
                await TextChannel.ModifyAsync(C => C.IsNsfw = NewNSFW);
                await Context.UserContext.RespondAsync(embed: new MarkNSFW(User.Guild.Name, Context.UserContext.Channel.Name, NewNSFW).Build());
            }
            else
            {
                await Context.UserContext.RespondAsync(text: "I couldn't find the channel");
            }
        }
    }
}
