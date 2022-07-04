using Discord.Net;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Exploration;
using Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations.Self
{
    public class ReactionRole : KaiaCommand
    {
        public override string ForeverId => CommandForeverIds.ReactionRole;

        public override string Name => "Reaction Role";

        public override string Description => "Automatically grant users a role when they react to a message.";

        public override bool GuildsOnly => true;

        public override List<IzolabellaCommandParameter> Parameters { get; } = new()
        {            
            new("Role", "The role to give or remove when the message is reacted to.", ApplicationCommandOptionType.Role, true),
            new("Message", "The message's id.", ApplicationCommandOptionType.String, true),
            new("Channel", "The channel the message is in.", ApplicationCommandOptionType.Channel, true),
            new("Emote", "The emote users must react with.", ApplicationCommandOptionType.String, true),
        };

        public override List<IIzolabellaCommandConstraint> Constraints { get; } = new()
        {
            new WhitelistPermissionsConstraint(false, GuildPermission.ManageRoles)
        };

        public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? RoleArg = Arguments.FirstOrDefault(A => A.Name == "role");
            IzolabellaCommandArgument? MessageIdArg = Arguments.FirstOrDefault(A => A.Name == "message");
            IzolabellaCommandArgument? ChannelArg = Arguments.FirstOrDefault(A => A.Name == "channel");
            IzolabellaCommandArgument? EmoteArg = Arguments.FirstOrDefault(A => A.Name == "emote");
            if(RoleArg != null && MessageIdArg != null && ChannelArg != null && EmoteArg != null &&
                RoleArg.Value is IRole Role && 
                MessageIdArg.Value is string MessageIdS && 
                Context.UserContext.User is SocketGuildUser User && 
                ChannelArg.Value is IGuildChannel Channel &&
                EmoteArg.Value is string EmoteS)
            {
                if(Channel is SocketTextChannel SChannel)
                {
                    if (ulong.TryParse(MessageIdS, out ulong MessageId) && Emoji.TryParse(EmoteS, out Emoji E))
                    {
                        IMessage? Message = await SChannel.GetMessageAsync(MessageId);
                        if (Message != null)
                        {
                            KaiaReactionRole NewReactionRole = new(Context.UserContext.User.Id, Message.Id, Channel.Id, Role.Id, new(E.Name));
                            KaiaGuild G = new(User.Guild.Id);
                            G.Settings.ReactionRoles.Add(NewReactionRole);
                            await G.SaveAsync();
                            await Message.AddReactionAsync(E);
                            await new ReactionRolesPaginated(Context, User.Guild, false).StartAsync();
                        }
                    }
                }
            }
        }

        public override Task OnErrorAsync(CommandContext? Context, HttpException Error)
        {
            if(Context != null)
            {

            }
            return base.OnErrorAsync(Context, Error);
        }
    }
}
