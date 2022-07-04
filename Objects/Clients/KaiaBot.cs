using Discord.Net;
using izolabella.Util;
using izolabella.Util.RateLimits.Limiters;
using Kaia.Bot.Objects.Constants.Permissions;
using Kaia.Bot.Objects.Constants.Responses;
using Kaia.Bot.Objects.Discord.Commands.Implementations.Guilds;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds;
using Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;
using System.Reflection;

namespace Kaia.Bot.Objects.Clients
{
    public class KaiaBot
    {
        public KaiaBot(KaiaParams Parameters)
        {
            this.Parameters = Parameters;
            this.Receivers = BaseImplementationUtil.GetItems<Receiver>();
            DateRateLimiter Limiter = new(DataStores.RateLimitsStore, "Main Command Rate Limiter", TimeSpan.FromSeconds(4));
            DateRateLimiter LimiterForLimiter = new(DataStores.RateLimitsStore, "Secondary Command Rate Limiter", TimeSpan.FromSeconds(4));
            this.Parameters.CommandHandler.PreCommandInvokeCheck = async (Command, Context) =>
            {
                if(Command is KaiaCommand KaiaCommand && await Limiter.CheckIfPassesAsync(Context.UserContext.User.Id))
                {
                    if (Context != null && Context.UserContext.Channel is SocketGuildChannel C)
                    {
                        GuildPermissions KaiaPerms = C.Guild.GetUser(this.Parameters.CommandHandler.Client.CurrentUser.Id).GuildPermissions;
                        List<GuildPermission> ReqPerms = KaiaCommand.RequiredPermissions.ToList();
                        ReqPerms.AddRange(DefaultPerms.Default);
                        if (ReqPerms.All(RP => KaiaPerms.Has(RP)))
                        {
                            return true;
                        }
                        else
                        {
                            PermissionsProblem V = new(C.Guild.Name, KaiaCommand.Name, KaiaPerms, ReqPerms.ToArray());
                            await Context.UserContext.RespondAsync(ephemeral: true, embed: V.Build());
                            return false;
                        }
                    }
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
            this.Parameters.CommandHandler.AfterJoinedGuild += this.ClientJoinedGuildAsync;
            this.Parameters.CommandHandler.CommandInvoked += this.AfterCommandExecutedAsync;
            this.Parameters.CommandHandler.Client.MessageReceived += this.MessageReceivedAsync;
            this.Parameters.CommandHandler.Client.Ready += this.ClientReadyAsync;
            this.Parameters.CommandHandler.Client.ReactionAdded += this.ClientReactionAddedAsync;
            this.Parameters.CommandHandler.Client.ReactionRemoved += this.ClientReactionRemovedAsync;
            this.Parameters.CommandHandler.Client.UserJoined += this.ClientUserJoinedGuildAsync;

            //this.Parameters.CommandHandler.OnCommandError += this.OnCommandErrorAsync;
        }

        public KaiaParams Parameters { get; }

        public List<Receiver> Receivers { get; }

        private async Task ClientUserJoinedGuildAsync(SocketGuildUser User)
        {
            KaiaGuild G = new(User.Guild.Id);
            foreach(KaiaAutoRole R in G.Settings.AutoRoles)
            {
                IRole? DR = await R.GetRoleAsync(User.Guild);
                if(DR != null)
                {
                    await User.AddRoleAsync(DR);
                }
            }
        }

        private async Task OnCommandErrorAsync(IzolabellaCommand? Command, HttpException Exception)
        {
            if(Command != null)
            {
                Console.WriteLine(Exception);
                Console.WriteLine($"__kaia_restart attempt @ [{DateTime.Now}]");
                await this.Parameters.StopAsync();
                await this.Parameters.StartAsync();
                Console.WriteLine($"__kaia_restart finalized @ [{DateTime.Now}]");
            }
        }

        private static async Task HandleReceiverTaskAsync(KaiaUser User, Receiver R, Task<ReceiverResult> ToHandle)
        {
            try
            {
                ReceiverResult Result = await ToHandle;
                if (Result.ItemToUse != null)
                {
                    await User.Settings.Inventory.RemoveItemOfIdAsync(Result.ItemToUse);
                }
                if (Result.UserToSave != null)
                {
                    await Result.UserToSave.SaveAsync();
                }
                if (Result.GuildToSave != null)
                {
                    await Result.GuildToSave.SaveAsync();
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"message receiver {R.Name} error => message: {Ex.Message} // src: {Ex.Source ?? "no source"}");
                KaiaSessionStatistics.MessageReceiverFailureCount++;
                await R.OnErrorAsync(Ex);
            }
        }

        #region reactions
        private async Task ClientReactionChangedAsync(SocketReaction Reaction, bool Removing)
        {
            KaiaUser User = new(Reaction.UserId);
            foreach (Receiver Receiver in this.Receivers)
            {
                await HandleReceiverTaskAsync(User, Receiver, Receiver.OnReactionAsync(this, Reaction, Removing));
            }
        }

        private async Task ClientReactionAddedAsync(Cacheable<IUserMessage, ulong> CachedMessage, Cacheable<IMessageChannel, ulong> CachedChannel, SocketReaction Reaction)
        {
            await this.ClientReactionChangedAsync(Reaction, false);
        }

        private async Task ClientReactionRemovedAsync(Cacheable<IUserMessage, ulong> CachedMessage, Cacheable<IMessageChannel, ulong> CachedChannel, SocketReaction Reaction)
        {
            await this.ClientReactionChangedAsync(Reaction, true);
        }
        #endregion

        #region messages, commands, startup, roles

        private async Task MessageReceivedAsync(SocketMessage Arg)
        {
            if (!Arg.Author.IsBot || this.Parameters.AllowBotsOnMessageReceivers)
            {
                KaiaUser User = new(Arg.Author.Id);
                KaiaGuild? Guild = Arg.Author is SocketGuildUser GuildUser ? new(GuildUser.Guild.Id) : null;
                IEnumerable<Receiver> Receivers = this.Receivers.Where((M) => M.CheckMessageValidityAsync(User, Arg).Result);
                foreach (Receiver Receiver in Receivers)
                {
                    await HandleReceiverTaskAsync(User, Receiver, Receiver.OnMessageAsync(User, Guild, Arg));
                }
            }
        }

        private async Task AfterCommandExecutedAsync(CommandContext Context, izolabella.Discord.Objects.Parameters.IzolabellaCommandArgument[] Arguments, IzolabellaCommand CommandInvoked)
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
            await Task.Run(() =>
            {
                this.RefreshCommandsAsync(this.Parameters.CommandHandler.Client.Guilds).ConfigureAwait(false);
                this.IterateOverReactionRolesAsync(this.Parameters.CommandHandler.Client.Guilds).ConfigureAwait(false);
                this.IterateOverAutoRolesAsync(this.Parameters.CommandHandler.Client.Guilds).ConfigureAwait(false);
            });
        }

        public async Task IterateOverReactionRolesAsync(IEnumerable<SocketGuild> RefreshFor)
        {
            foreach (SocketGuild DiscordGuild in RefreshFor)
            {
                SocketGuildUser KaiaUser = DiscordGuild.GetUser(this.Parameters.CommandHandler.Client.CurrentUser.Id);
                GuildPermissions KaiaPerms = KaiaUser.GuildPermissions;
                if(KaiaPerms.Has(GuildPermission.ManageRoles) && KaiaPerms.Has(GuildPermission.ReadMessageHistory))
                {
                    KaiaGuild Guild = new(DiscordGuild.Id);
                    await DiscordGuild.DownloadUsersAsync();
                    foreach (KaiaReactionRole Role in Guild.Settings.ReactionRoles.Where(R => R.Enforce))
                    {
                        IRole? DiscordRole = await Role.GetRoleAsync(DiscordGuild);
                        IMessage? Message = await Role.GetMessageAsync(DiscordGuild);
                        if (DiscordRole != null && Message != null)
                        {
                            #region users who reacted but don't have the role
                            IAsyncEnumerable<IReadOnlyCollection<IUser>> UsersThatReacted = Message.GetReactionUsersAsync(Role.Emote.IsCustom ? Emote.Parse(Role.Emote.ToString()) : Role.Emote, int.MaxValue);
                            List<SocketGuildUser> Cached = new();
                            await foreach (IReadOnlyCollection<IUser> U in UsersThatReacted)
                            {
                                foreach (IUser IUs in U.Where(GU => GU.Id != KaiaUser.Id))
                                {
                                    SocketGuildUser? ActualU = DiscordGuild.GetUser(IUs.Id);
                                    if (!ActualU.Roles.Any(R => R.Id == Role.Id))
                                    {
                                        await ActualU.AddRoleAsync(DiscordRole);
                                    }
                                    Cached.Add(ActualU);
                                }
                            }
                            #endregion

                            #region users who have the role but didn't react
                            await foreach (IReadOnlyCollection<IGuildUser> UserList in DiscordGuild.GetUsersAsync())
                            {
                                foreach (IGuildUser User in UserList.Where(GU => GU.Id != KaiaUser.Id))
                                {
                                    if (User.RoleIds.Any(RId => Role.RoleId == RId))
                                    {
                                        SocketGuildUser? Matching = Cached.FirstOrDefault(CU => CU.Id == User.Id);
                                        if(Matching == null)
                                        {
                                            await User.RemoveRoleAsync(Role.RoleId);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
        }

        public async Task IterateOverAutoRolesAsync(IEnumerable<SocketGuild> RefreshFor)
        {
            foreach(SocketGuild Guild in RefreshFor)
            {
                await Guild.DownloadUsersAsync();
                SocketGuildUser KaiaUser = Guild.GetUser(this.Parameters.CommandHandler.Client.CurrentUser.Id);
                if(KaiaUser.GuildPermissions.Has(GuildPermission.ManageRoles))
                {
                    KaiaGuild G = new(Guild.Id);
                    foreach (KaiaAutoRole Role in G.Settings.AutoRoles.Where(AR => AR.Enforce))
                    {
                        IRole? R = await Role.GetRoleAsync(Guild);
                        if (R != null)
                        {
                            foreach (SocketGuildUser User in Guild.Users.Where(GU => GU.Id != KaiaUser.Id))
                            {
                                if (User.Roles.Any(UR => UR.Id != R.Id) && User.Hierarchy < KaiaUser.Hierarchy)
                                {
                                    await User.AddRoleAsync(R);
                                }
                            }
                        }
                    }
                }
            }
        }

        public async Task RefreshCommandsAsync(IEnumerable<SocketGuild> RefreshFor)
        {
            List<IzolabellaCommand> Commands = await IzolabellaDiscordCommandClient.GetIzolabellaCommandsAsync(Assembly.GetAssembly(typeof(KaiaBot)) ?? throw new NullReferenceException());
            foreach (SocketGuild DiscordGuild in RefreshFor)
            {
                KaiaGuild Guild = new(DiscordGuild.Id);
                foreach (IzolabellaCommand Command in Commands)
                {
                    if (Command is KaiaCommand CCBLevelCommand)
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
        #endregion
    }
}
