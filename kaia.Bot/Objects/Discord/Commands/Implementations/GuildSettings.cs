﻿using Discord;
using Discord.WebSocket;
using izolabella.Discord.Objects.Arguments;
using izolabella.Discord.Objects.Constraints.Implementations;
using izolabella.Discord.Objects.Constraints.Interfaces;
using izolabella.Discord.Objects.Interfaces;
using izolabella.Discord.Objects.Parameters;
using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.Discord.Commands.Bases;
using Kaia.Bot.Objects.Discord.Embeds.Implementations;

namespace Kaia.Bot.Objects.Discord.Commands.Implementations
{
    public class GuildSettings : ICCBCommand
    {
        public string Name => "Guild Settings";

        public string Description => "Change my guild's settings.";

        public List<IzolabellaCommandParameter> Parameters => new()
        {
            new IzolabellaCommandParameter("Counting Channel", "The channel used for counting.", ApplicationCommandOptionType.Channel, false),
        };

        public List<IIzolabellaCommandConstraint> Constraints { get; } = new()
        {
            new WhitelistPermissionsConstraint(true, GuildPermission.Administrator)
        };
        public string ForeverId => CommandForeverIds.GuildSettingsCommand;

        public async Task RunAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments)
        {
            IzolabellaCommandArgument? CountingChannelArgument = Arguments.FirstOrDefault();

            if (Context.UserContext.User is SocketGuildUser SUser)
            {
                KaiaGuild Guild = new(SUser.Guild.Id);
                if (CountingChannelArgument != null && CountingChannelArgument.Value is IGuildChannel NewCountingChannelId)
                {
                    Guild.Settings.CountingChannelId = NewCountingChannelId.Id;
                    Guild.Settings = Guild.Settings;
                }
                await Context.UserContext.RespondAsync(text: "", ephemeral: false, embed: new GuildSettingsView(SUser.Guild.Name, Guild).Build());

            }
            else
            {
            }
        }

        public Task OnLoadAsync(IIzolabellaCommand[] AllCommands)
        {
            return Task.CompletedTask;
        }

        public Task OnConstrainmentAsync(CommandContext Context, IzolabellaCommandArgument[] Arguments, IIzolabellaCommandConstraint ConstraintThatFailed)
        {
            return Task.CompletedTask;
        }
    }
}
