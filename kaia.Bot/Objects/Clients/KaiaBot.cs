using Discord;
using Discord.WebSocket;
using izolabella.Discord.Objects.Clients;
using izolabella.Discord.Objects.Constraints.Implementations;
using izolabella.Discord.Objects.Interfaces;
using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.CCB_Structures.Inventory.Items.Implementations;
using Kaia.Bot.Objects.Client_Parameters;
using Kaia.Bot.Objects.Discord.Commands.Bases;
using Kaia.Bot.Objects.Discord.Commands.Implementations;
using Kaia.Bot.Objects.Discord.Events.Interfaces;
using Kaia.Bot.Objects.Discord.Message_Receivers.Results;

namespace Kaia.Bot.Objects.Clients
{
    public class KaiaBot
    {
        public KaiaBot(KaiaParams Parameters)
        {
            this.Parameters = Parameters;
            this.MessageReceivers = InterfaceImplementationController.GetItems<IMessageReceiver>();
            this.Parameters.CommandHandler.AfterJoinedGuild += this.ClientJoinedGuildAsync;
            this.Parameters.CommandHandler.CommandInvoked += this.AfterCommandExecutedAsync;
            this.Parameters.CommandHandler.Client.MessageReceived += this.MessageReceivedAsync;
            this.Parameters.CommandHandler.Client.Ready += this.ClientReadyAsync;
        }

        public KaiaParams Parameters { get; }

        public List<IMessageReceiver> MessageReceivers { get; }

        private async Task MessageReceivedAsync(SocketMessage Arg)
        {
            if (!Arg.Author.IsBot || this.Parameters.AllowBotsOnMessageReceivers)
            {
                KaiaUser User = new(Arg.Author.Id);
                KaiaGuild? Guild = Arg.Author is SocketGuildUser GuildUser ? new(GuildUser.Guild.Id) : null;
                IEnumerable<IMessageReceiver> Receivers = this.MessageReceivers.Where((M) =>
                {
                    return M.CheckMessageValidityAsync(User, Arg).Result;
                });
                foreach (IMessageReceiver Receiver in Receivers)
                {
                    if (Receiver != null)
                    {
                        try
                        {
                            MessageReceiverResult Result = await Receiver.RunAsync(User, Guild, Arg);
                            if (Result.ItemToUse != null && Result.ItemToUse is CountingRefresher CRef)
                            {
                                await Receiver.CallbackAsync(User, Arg, Result);
                                User.Settings.Inventory.Items.Remove(CRef);
                            }
                            if (Result.SaveUser)
                            {
                                await User.SaveAsync();
                            }
                            if (Result.SaveUserGuild && Guild != null)
                            {
                                await Guild.SaveAsync();
                            }
                        }
                        catch (Exception Ex)
                        {
                            Console.WriteLine($"message receiver {Receiver.Name} error => {Ex.Message}");
                            await Receiver.OnErrorAsync(Ex);
                        }
                    }
                }
                await User.SaveAsync();
            }
        }

        private async Task AfterCommandExecutedAsync(izolabella.Discord.Objects.Arguments.CommandContext Context, izolabella.Discord.Objects.Parameters.IzolabellaCommandArgument[] Arguments, izolabella.Discord.Objects.Interfaces.IIzolabellaCommand CommandInvoked)
        {
            if (Context.UserContext.User is SocketGuildUser SUser && CommandInvoked is AddCommandConstraint)
            {
                await this.RefreshCommandsAsync(new[] { SUser.Guild });
            }
            await new KaiaUser(Context.UserContext.User.Id).SaveAsync();
            if (Context.UserContext.User is SocketGuildUser SU)
            {
                await new KaiaGuild(SU.Guild.Id).SaveAsync();
            }
            Console.WriteLine("saved data");
        }

        private async Task ClientJoinedGuildAsync(SocketGuild Arg)
        {
            await this.RefreshCommandsAsync(new[] { Arg });
        }

        private async Task ClientReadyAsync()
        {
            await this.RefreshCommandsAsync(this.Parameters.CommandHandler.Client.Guilds);
        }

        public async Task RefreshCommandsAsync(IEnumerable<SocketGuild> RefreshFor)
        {
            List<IIzolabellaCommand> Commands = await IzolabellaDiscordCommandClient.GetIzolabellaCommandsAsync();
            foreach (SocketGuild DiscordGuild in RefreshFor)
            {
                KaiaGuild Guild = new(DiscordGuild.Id);
                foreach (IIzolabellaCommand Command in Commands)
                {
                    if (Command is ICCBCommand CCBLevelCommand)
                    {
                        GuildPermission[]? Permissions = Guild.Settings.OverrideCommandPermissionsConstraint.GetValueOrDefault(CCBLevelCommand.ForeverId);
                        if (Permissions != null)
                        {
                            CCBLevelCommand.Constraints.RemoveAll(C => C.Type == izolabella.Discord.Objects.Enums.ConstraintTypes.WhitelistPermissions && (C.ConstrainToOneGuildOfThisId == null || C.ConstrainToOneGuildOfThisId == Guild.Id));
                            CCBLevelCommand.Constraints.Add(new WhitelistPermissionsConstraint(true, Permissions)
                            {
                                ConstrainToOneGuildOfThisId = Guild.Id
                            });
                        }
                        ulong[]? Roles = Guild.Settings.OverrideCommandRolesConstraint.GetValueOrDefault(CCBLevelCommand.ForeverId);
                        if (Roles != null)
                        {
                            CCBLevelCommand.Constraints.RemoveAll(C => C.Type == izolabella.Discord.Objects.Enums.ConstraintTypes.WhitelistRoles && (C.ConstrainToOneGuildOfThisId == null || C.ConstrainToOneGuildOfThisId == Guild.Id));
                            CCBLevelCommand.Constraints.Add(new WhitelistRolesConstraint(true, Roles)
                            {
                                ConstrainToOneGuildOfThisId = Guild.Id
                            });
                        }
                    }
                }
            }
            await this.Parameters.CommandHandler.UpdateCommandsAsync(Commands.ToArray());
        }
    }
}
