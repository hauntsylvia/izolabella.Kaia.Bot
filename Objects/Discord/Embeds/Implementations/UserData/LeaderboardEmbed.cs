using Discord.WebSocket;
using izolabella.Discord.Objects.Clients;
using Kaia.Bot.Objects.Constants.Enums;
using Kaia.Bot.Objects.KaiaStructures.Guilds;
using Kaia.Bot.Objects.KaiaStructures.Users;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.UserData
{
    internal class LeaderboardEmbed : KaiaPathEmbed
    {
        internal LeaderboardEmbed(LeaderboardTypes LType, IzolabellaDiscordCommandClient Reference, int NumberOfElements, string LeaderboardDisplayName)
            : base(Strings.EmbedStrings.PathIfNoGuild, Strings.EmbedStrings.FakePaths.Leaderboards, LeaderboardDisplayName)
        {
            List<string> Displays = new();

            if (LType == LeaderboardTypes.UsersHighestNumberCounted || LType == LeaderboardTypes.UsersMostNumbersCounted)
            {
                List<KaiaUser> Users = DataStores.UserStore.ReadAllAsync<KaiaUser>().Result
                    .OrderByDescending(U => LType == LeaderboardTypes.UsersHighestNumberCounted ? U.Settings.HighestCountEver : U.Settings.NumbersCounted)
                    .Take(NumberOfElements)
                    .ToList();
                int LongestDisplayName = Strings.EmbedStrings.UnknownUser.Length;
                foreach (KaiaUser User in Users)
                {
                    SocketUser? DiscordUser = Reference.Client.GetUser(User.Id);
                    if (DiscordUser != null && LongestDisplayName < DiscordUser.Username.Length)
                    {
                        LongestDisplayName = DiscordUser.Username.Length;
                    }
                }
                for (int Index = 0; Index < NumberOfElements && Users.Count > Index; Index++)
                {
                    KaiaUser ThisUser = Users[Index];
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
                List<KaiaGuild> Guilds = DataStores.GuildStore.ReadAllAsync<KaiaGuild>().Result
                    .OrderByDescending(U => LType == LeaderboardTypes.GuildsHighestNumberCounted ? U.Settings.HighestCountEver : U.Settings.LastSuccessfulNumber)
                    .Take(NumberOfElements)
                    .ToList();
                int LongestDisplayName = Strings.EmbedStrings.UnknownGuild.Length;
                foreach (KaiaGuild Guild in Guilds)
                {
                    SocketGuild? DGuild = Reference.Client.GetGuild(Guild.Id);
                    if (DGuild != null && LongestDisplayName < DGuild.Name.Length)
                    {
                        LongestDisplayName = DGuild.Name.Length;
                    }
                }
                for (int Index = 0; Index < NumberOfElements && Guilds.Count > Index; Index++)
                {
                    KaiaGuild ThisGuild = Guilds[Index];
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
