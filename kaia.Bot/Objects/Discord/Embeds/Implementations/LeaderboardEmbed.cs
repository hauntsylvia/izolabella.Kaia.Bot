using Discord.WebSocket;
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
                    .OrderByDescending(U => LType == LeaderboardTypes.UsersHighestNumberCounted ? U.CountingInfo.HighestCountEver : U.CountingInfo.NumbersCounted)
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
                        $"`{(LType == LeaderboardTypes.UsersHighestNumberCounted ? ThisUser.CountingInfo.HighestCountEver : ThisUser.CountingInfo.NumbersCounted)}`" +
                        $"{(LType == LeaderboardTypes.UsersHighestNumberCounted ? " - highest number counted" : " - total numbers counted")}");
                }
            }
            else
            {

            }

            this.WriteListToOneField(Strings.EmbedStrings.FakePaths.Users, Displays, "\n");
        }
    }
}
