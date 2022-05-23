using Discord;
using Discord.WebSocket;
using izolabella.CompetitiveCounting.Bot.Objects.CCB_Structures;
using izolabella.CompetitiveCounting.Bot.Objects.Discord.Commands.Bases;
using izolabella.Discord.Objects.Arguments;
using izolabella.Discord.Objects.Clients;
using izolabella.Discord.Objects.Constraints.Implementations;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Interfaces;
using izolabella.Discord.Objects.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.Client_Parameters
{
    public class ClientParameters
    {
        public ClientParameters(DiscordSocketConfig Config, string? Token)
        {
            this.CommandHandler = new(Config);
            this.Token = Token;
        }

        public IzolabellaDiscordCommandClient CommandHandler { get; }
        private string? Token { get; }
        public async Task StartAsync(string? Token = null)
        {
            string? T = this.Token ?? Token;
            if(T != null)
            {
                this.CommandHandler.Client.Ready += async () =>
                {
                    await this.RefreshCommandsAsync();
                };
                await this.CommandHandler.StartAsync(T, false);
            }
            else
            {
                throw new NullReferenceException(nameof(this.Token));
            }
        }

        public async Task RefreshCommandsAsync(params SocketGuild[] RefreshFor)
        {
            List<IIzolabellaCommand> Commands = await IzolabellaDiscordCommandClient.GetIzolabellaCommandsAsync();
            foreach (SocketGuild DiscordGuild in RefreshFor)
            {
                CCBGuild Guild = await CCBGuild.GetOrCreateAsync(DiscordGuild.Id);
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
            await this.CommandHandler.UpdateCommandsAsync(Commands.ToArray());
        }
    }
}
