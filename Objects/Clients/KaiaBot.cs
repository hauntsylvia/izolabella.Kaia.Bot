
using izolabella.Util;
using izolabella.Util.RateLimits.Limiters;
using Kaia.Bot.Objects.Constants.Responses;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds;
using System.Reflection;

namespace Kaia.Bot.Objects.Clients
{
    public class KaiaBot
    {
        public KaiaBot(KaiaParams Parameters)
        {
            this.Parameters = Parameters;
            this.MessageReceivers = BaseImplementationUtil.GetItems<IMessageReceiver>();
            DateRateLimiter Limiter = new(DataStores.RateLimitsStore, "Main Command Rate Limiter", TimeSpan.FromSeconds(4));
            DateRateLimiter LimiterForLimiter = new(DataStores.RateLimitsStore, "Secondary Command Rate Limiter", TimeSpan.FromSeconds(4));
            this.Parameters.CommandHandler.PreCommandInvokeCheck = async (Context) =>
            {
                if(await Limiter.CheckIfPassesAsync(Context.UserContext.User.Id))
                {
                    return true;
                }
                else
                {
                    if(await LimiterForLimiter.CheckIfPassesAsync(Context.UserContext.User.Id))
                    {
                        await Responses.PipeErrors(Context, new RateLimited());
                    }
                    return false;
                }
            };
;
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
                IEnumerable<IMessageReceiver> Receivers = this.MessageReceivers.Where((M) => M.CheckMessageValidityAsync(User, Arg).Result);
                foreach (IMessageReceiver Receiver in Receivers)
                {
                    if (Receiver != null)
                    {
                        try
                        {
                            MessageReceiverResult Result = await Receiver.RunAsync(User, Guild, Arg);
                            if (Result.ItemToUse != null)
                            {
                                await Receiver.CallbackAsync(User, Arg, Result);
                                await User.Settings.Inventory.RemoveItemOfIdAsync(Result.ItemToUse);
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

        private async Task AfterCommandExecutedAsync(CommandContext Context, izolabella.Discord.Objects.Parameters.IzolabellaCommandArgument[] Arguments, IIzolabellaCommand CommandInvoked)
        {
            if (Context.UserContext.User is SocketGuildUser SUser && CommandInvoked is AddCommandConstraintCommand)
            {
                await this.RefreshCommandsAsync(new[] { SUser.Guild });
            }
            KaiaUser U = new(Context.UserContext.User.Id);
            await U.AchievementProcessor.TryAwardAchievements(U, Context, KaiaAchievementRoom.Achievements.ToArray());
            await U.SaveAsync();
            if (Context.UserContext.User is SocketGuildUser SU)
            {
                await new KaiaGuild(SU.Guild.Id).SaveAsync();
            }
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
            List<IIzolabellaCommand> Commands = await IzolabellaDiscordCommandClient.GetIzolabellaCommandsAsync(Assembly.GetAssembly(typeof(KaiaBot)) ?? throw new NullReferenceException());
            foreach (SocketGuild DiscordGuild in RefreshFor)
            {
                KaiaGuild Guild = new(DiscordGuild.Id);
                foreach (IIzolabellaCommand Command in Commands)
                {
                    if (Command is IKaiaCommand CCBLevelCommand)
                    {
                        GuildPermission[]? Permissions = Guild.Settings.OverrideCommandPermissionsConstraint.GetValueOrDefault(CCBLevelCommand.ForeverId);
                        if (Permissions != null)
                        {
                            _ = CCBLevelCommand.Constraints.RemoveAll(C => C.Type == ConstraintTypes.WhitelistPermissions && (C.ConstrainToOneGuildOfThisId == null || C.ConstrainToOneGuildOfThisId == Guild.Id));
                            CCBLevelCommand.Constraints.Add(new WhitelistPermissionsConstraint(true, Permissions)
                            {
                                ConstrainToOneGuildOfThisId = Guild.Id
                            });
                        }
                        ulong[]? Roles = Guild.Settings.OverrideCommandRolesConstraint.GetValueOrDefault(CCBLevelCommand.ForeverId);
                        if (Roles != null)
                        {
                            _ = CCBLevelCommand.Constraints.RemoveAll(C => C.Type == ConstraintTypes.WhitelistRoles && (C.ConstrainToOneGuildOfThisId == null || C.ConstrainToOneGuildOfThisId == Guild.Id));
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
