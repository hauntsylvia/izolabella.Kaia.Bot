using Discord.Net;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.ReactionRoles;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Exploration;
using Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations.ServerManagement.Roles
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
            new("Enforce", "(Recommended: true) The role is granted or removed even when Kaia is offline (on startup).", ApplicationCommandOptionType.Boolean, true),
        };

        public override List<IIzolabellaCommandConstraint> Constraints { get; } = new()
        {
            new WhitelistPermissionsConstraint(false, GuildPermission.ManageRoles)
        };

        public override List<GuildPermission> RequiredPermissions => new()
        {
            GuildPermission.ManageRoles,
            GuildPermission.ReadMessageHistory,
            GuildPermission.AddReactions
        };

        public override async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? RoleArg = Arguments.FirstOrDefault(A => A.Name == "role");
            IzolabellaCommandArgument? MessageIdArg = Arguments.FirstOrDefault(A => A.Name == "message");
            IzolabellaCommandArgument? ChannelArg = Arguments.FirstOrDefault(A => A.Name == "channel");
            IzolabellaCommandArgument? EmoteArg = Arguments.FirstOrDefault(A => A.Name == "emote");
            IzolabellaCommandArgument? EnforceArg = Arguments.FirstOrDefault(A => A.Name == "enforce");
            if (RoleArg != null && MessageIdArg != null && ChannelArg != null && EmoteArg != null && EnforceArg != null &&
                RoleArg.Value is IRole Role &&
                MessageIdArg.Value is string MessageIdS &&
                Context.UserContext.User is SocketGuildUser User &&
                ChannelArg.Value is IGuildChannel Channel &&
                EmoteArg.Value is string EmoteS &&
                EnforceArg.Value is bool Enforce)
            {
                if (Channel is SocketTextChannel SChannel)
                {
                    if (ulong.TryParse(MessageIdS, out ulong MessageId) && (Emote.TryParse(EmoteS, out Emote Emo) || Emoji.TryParse(EmoteS, out Emoji Emoj)))
                    {
                        IMessage? Message = await SChannel.GetMessageAsync(MessageId);
                        if (Message != null)
                        {
                            IEmote? A = Emote.TryParse(EmoteS, out Emote A1) ? A1 : Emoji.TryParse(EmoteS, out Emoji A2) ? A2 : null;
                            string? As = A?.ToString();
                            if (A != null && As != null)
                            {
                                KaiaReactionRole NewReactionRole = new(Context.UserContext.User.Id, Message.Id, Channel.Id, Role.Id, new(As), Enforce);
                                KaiaGuild G = new(User.Guild.Id);
                                G.Settings.ReactionRoles.Add(NewReactionRole);
                                await G.SaveAsync();
                                await Message.AddReactionAsync(A);
                                await new ReactionRolesPaginated(Context, User.Guild, false).StartAsync();
                            }
                        }
                    }
                }
            }
        }

        public override Task OnErrorAsync(CommandContext? Context, HttpException Error)
        {
            return Task.CompletedTask;
        }
    }
}
