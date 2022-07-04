using Kaia.Bot.Objects.Clients;
using Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Receivers.Implementations
{
    public class ReactionRoles : Receiver
    {
        public override string Name => "Reaction Roles";

        public override Task CallbackAsync(KaiaUser Author, SocketMessage Message, ReceiverResult CausedCallback)
        {
            return Task.CompletedTask;
        }

        public override async Task<ReceiverResult> OnReactionAsync(KaiaBot From, SocketReaction Reaction, bool Removing = false)
        {
            ReceiverResult Result = new();
            if(Reaction.Channel is SocketGuildChannel Channel)
            {
                KaiaGuild Guild = new(Channel.Guild.Id);
                IUser? U = Reaction.User.GetValueOrDefault() ?? await From.Parameters.CommandHandler.Client.GetUserAsync(Reaction.UserId);
                if (U is not null and SocketGuildUser User)
                {
                    foreach (KaiaReactionRole Role in Guild.Settings.ReactionRoles)
                    {
                        if (Role.ChannelId == Reaction.Channel.Id && Role.MessageId == Reaction.MessageId && Role.Emote.Name == Reaction.Emote.Name)
                        {
                            IRole? ActualRole = await Role.GetRoleAsync(Channel.Guild);
                            if(ActualRole != null)
                            {
                                if(Removing)
                                {
                                    await User.RemoveRoleAsync(ActualRole.Id);
                                }
                                else
                                {
                                    await User.AddRoleAsync(ActualRole);
                                }
                                Result.GuildToSave = Guild;
                            }
                        }
                    }
                }
            }
            return Result;
        }

        public override Task OnErrorAsync(Exception Exception)
        {
            return Task.CompletedTask;
        }
    }
}
