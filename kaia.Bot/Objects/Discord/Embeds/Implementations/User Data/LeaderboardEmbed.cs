﻿using Discord.WebSocket;
using izolabella.Discord.Objects.Clients;
using Kaia.Bot.Objects.CCB_Structures;
using Kaia.Bot.Objects.Clients;
using Kaia.Bot.Objects.Constants.Enums;
using Kaia.Bot.Objects.Discord.Embeds.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations
{
    internal class LeaderboardEmbed : CCBPathEmbed
    {
        internal LeaderboardEmbed(LeaderboardTypes LType, IzolabellaDiscordCommandClient Reference, int NumberOfElements, string LeaderboardDisplayName) 
            : base(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.Leaderboards, LeaderboardDisplayName)
        {
            List<string> Displays = new();

            if (LType == LeaderboardTypes.UsersHighestNumberCounted || LType == LeaderboardTypes.UsersMostNumbersCounted)
            {
                List<CCBUser> Users = DataStores.UserStore.ReadAllAsync<CCBUser>().Result
                    .OrderByDescending(U => LType == LeaderboardTypes.UsersHighestNumberCounted ? U.Settings.HighestCountEver : U.Settings.NumbersCounted)
                    .Take(NumberOfElements)
                    .ToList();
                int LongestDisplayName = Strings.EmbedStrings.UnknownUser.Length;
                foreach (CCBUser User in Users)
                {
                    SocketUser? DiscordUser = Reference.Client.GetUser(User.Id);
                    if (DiscordUser != null && LongestDisplayName < DiscordUser.Username.Length)
                    {
                        LongestDisplayName = DiscordUser.Username.Length;
                    }
                }
                for (int Index = 0; Index < NumberOfElements && Users.Count > Index; Index++)
                {
                    CCBUser ThisUser = Users[Index];
                    SocketUser? DiscordUser = Reference.Client.GetUser(ThisUser.Id);
                    string DisplayName = DiscordUser != null ? DiscordUser.Username : Strings.EmbedStrings.UnknownUser;
                    DisplayName = DisplayName.Length < LongestDisplayName ? DisplayName + new string(' ', LongestDisplayName - DisplayName.Length) : DisplayName;
                    Displays.Add($"@`{DisplayName}` - " +
                        $"`{(LType == LeaderboardTypes.UsersHighestNumberCounted ? ThisUser.Settings.HighestCountEver : ThisUser.Settings.NumbersCounted)}`" +
                        $"{(LType == LeaderboardTypes.UsersHighestNumberCounted ? " - highest number counted" : " - total numbers counted")}");
                }
            }
            else
            {
                List<CCBGuild> Guilds = DataStores.GuildStore.ReadAllAsync<CCBGuild>().Result
                    .OrderByDescending(U => LType == LeaderboardTypes.GuildsHighestNumberCounted ? U.Settings.HighestCountEver : U.Settings.LastSuccessfulNumber)
                    .Take(NumberOfElements)
                    .ToList();
                int LongestDisplayName = Strings.EmbedStrings.UnknownGuild.Length;
                foreach (CCBGuild Guild in Guilds)
                {
                    SocketGuild? DGuild = Reference.Client.GetGuild(Guild.Id);
                    if (DGuild != null && LongestDisplayName < DGuild.Name.Length)
                    {
                        LongestDisplayName = DGuild.Name.Length;
                    }
                }
                for (int Index = 0; Index < NumberOfElements && Guilds.Count > Index; Index++)
                {
                    CCBGuild ThisGuild = Guilds[Index];
                    SocketGuild? DiscordGuild = Reference.Client.GetGuild(ThisGuild.Id);
                    string DisplayName = DiscordGuild != null ? DiscordGuild.Name : Strings.EmbedStrings.UnknownGuild;
                    DisplayName = DisplayName.Length < LongestDisplayName ? DisplayName + new string(' ', LongestDisplayName - DisplayName.Length) : DisplayName;
                    Displays.Add($"@`{DisplayName}` - " +
                        $"`{(LType == LeaderboardTypes.GuildsHighestNumberCounted ? ThisGuild.Settings.HighestCountEver : ThisGuild.Settings.LastSuccessfulNumber)}`" +
                        $"{(LType == LeaderboardTypes.GuildsHighestNumberCounted ? " - highest number counted" : " - current number")}");
                }
            }

            this.WriteListToOneField(LType == LeaderboardTypes.GuildsHighestNumberCounted || LType == LeaderboardTypes.GuildsCurrentHighestNumber ? 
                Strings.EmbedStrings.FakePaths.Guilds : Strings.EmbedStrings.FakePaths.Users, Displays, "\n");
        }
    }
}