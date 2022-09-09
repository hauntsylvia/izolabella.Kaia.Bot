using Discord.Net;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;
using izolabella.Kaia.Bot.Objects.Clients;

namespace izolabella.Kaia.Bot.Objects.Discord.Receivers.Implementations;

public class ReactionRoles : IzolabellaReactionReceiver
{
    public override string Name => "Reaction Roles";

    public override Predicate<SocketReaction> ValidPredicate => (Arg) => true;

    public override async Task OnReactionAsync(IzolabellaDiscordClient Reference, SocketReaction Reaction, bool ReactionRemoved)
    {
        if (Reaction.Channel is SocketGuildChannel Channel)
        {
            KaiaGuild Guild = new(Channel.Guild.Id);
            IUser? U = Reaction.User.GetValueOrDefault() ?? await Reference.GetUserAsync(Reaction.UserId);
            if (U is not null and SocketGuildUser User && User.Id != Reference.CurrentUser.Id)
            {
                foreach (KaiaReactionRole Role in Guild.Settings.ReactionRoles)
                {
                    if (Role.ChannelId == Reaction.Channel.Id && Role.MessageId == Reaction.MessageId && Role.Emote.ToString() == Reaction.Emote.ToString())
                    {
                        IRole? ActualRole = await Role.GetRoleAsync(Channel.Guild);
                        if (ActualRole != null)
                        {
                            if (ReactionRemoved)
                            {
                                await User.RemoveRoleAsync(ActualRole.Id);
                            }
                            else
                            {
                                await User.AddRoleAsync(ActualRole);
                            }
                        }
                    }
                }
            }
            await Guild.SaveAsync();
        }
    }

    public override Task OnErrorAsync(HttpException Exception)
    {
        return Task.CompletedTask;
    }
}
