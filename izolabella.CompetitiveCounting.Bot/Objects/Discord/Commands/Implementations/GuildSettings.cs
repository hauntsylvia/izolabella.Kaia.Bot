﻿using Discord;
using Discord.WebSocket;
using izolabella.CompetitiveCounting.Bot.Objects.CCB_Structures;
using izolabella.CompetitiveCounting.Bot.Objects.Discord.Commands.Bases;
using izolabella.CompetitiveCounting.Bot.Objects.Discord.Embeds.Implementations;
using izolabella.CompetitiveCounting.Bot.Objects.Exceptions;
using izolabella.Discord.Objects.Arguments;
using izolabella.Discord.Objects.Constraints.Implementations;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izolabella.CompetitiveCounting.Bot.Objects.Discord.Commands.Implementations
{
    public class GuildSettings : ICCBCommand
    {
        public string Name => "Guild Settings";

        public string Description => "Change my guild's settings.";

        public List<IzolabellaCommandParameter> Parameters => new()
        {
            new IzolabellaCommandParameter("Counting Channel", "The channel used for counting.", ApplicationCommandOptionType.Channel, false)
        };
        public List<IIzolabellaCommandConstraint> Constraints => new()
        {
            {
                new WhitelistPermissionsConstraint(true, GuildPermission.Administrator)
            },
            {
                new WhitelistUsersConstraint(916140079309787137)
            }
        };

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? CountingChannelArgument = Arguments.FirstOrDefault();
            
            if(Context.UserContext.User is SocketGuildUser SUser)
            {
                CCBGuild Guild = await CCBGuild.GetOrCreateAsync(SUser.Guild.Id);
                if (CountingChannelArgument != null && CountingChannelArgument.Value is IGuildChannel NewCountingChannelId)
                {
                    Guild = await Guild.ChangeGuildSettings(new(NewCountingChannelId.Id, Guild.Settings.LastSuccessfulNumber, Guild.Settings.LastUserWhoCounted, Guild.Settings.HighestCountEver));
                }
                await Context.UserContext.RespondAsync(text: "", ephemeral: false, embed: new GuildSettingsView(SUser.Guild.Name, Guild).Build());

            }
            else
            {
            }
        }

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }
    }
}
