using Kaia.Bot.Objects.Constants.Enums;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Others
{
    public class LeaderboardEmbed : KaiaPathEmbedRefreshable
    {
        public LeaderboardEmbed(LeaderboardTypes LType, IzolabellaDiscordClient Reference, int NumberOfElements, string LeaderboardDisplayName)
            : base(Strings.EmbedStrings.FakePaths.Global, Strings.EmbedStrings.FakePaths.Leaderboards, LeaderboardDisplayName)
        {
            this.LType = LType;
            this.Reference = Reference;
            this.NumberOfElements = NumberOfElements;
            this.LeaderboardDisplayName = LeaderboardDisplayName;
            this.ClientRefreshAsync().Wait();
        }

        public LeaderboardTypes LType { get; }

        public IzolabellaDiscordClient Reference { get; }

        public int NumberOfElements { get; }

        public string LeaderboardDisplayName { get; }

        private static string Truncate(string A)
        {
            return A.Length >= 17 ? A[..20] + ".." : A;
        }

        protected override Task ClientRefreshAsync()
        {
            List<string> Displays = new();

            if (this.LType is LeaderboardTypes.UsersHighestNumberCounted or LeaderboardTypes.UsersMostNumbersCounted)
            {
                List<KaiaUser> Users = DataStores.UserStore.ReadAllAsync<KaiaUser>().Result
                    .OrderByDescending(U => this.LType == LeaderboardTypes.UsersHighestNumberCounted ? U.Settings.HighestCountEver : U.Settings.NumbersCounted)
                    .Take(this.NumberOfElements)
                    .ToList(); //5435 3686 6847227915
                int LongestDisplayName = Truncate(Strings.EmbedStrings.UnknownUser).Length;
                foreach (KaiaUser User in Users)
                {
                    SocketUser? DiscordUser = this.Reference.GetUser(User.Id);
                    if (DiscordUser != null && LongestDisplayName < Truncate(DiscordUser.Username).Length)
                    {
                        LongestDisplayName = Truncate(DiscordUser.Username).Length;
                    }
                }
                for (int Index = 0; Index < this.NumberOfElements && Users.Count > Index; Index++)
                {
                    KaiaUser ThisUser = Users[Index];
                    SocketUser? DiscordUser = this.Reference.GetUser(ThisUser.Id);
                    string DisplayName = Truncate(DiscordUser != null ? DiscordUser.Username : Strings.EmbedStrings.UnknownUser);
                    DisplayName = DisplayName.Length < LongestDisplayName ? DisplayName + new string(' ', LongestDisplayName - DisplayName.Length) : DisplayName;
                    Displays.Add($"@`{DisplayName}` - " +
                        $"`{(this.LType == LeaderboardTypes.UsersHighestNumberCounted ? ThisUser.Settings.HighestCountEver : ThisUser.Settings.NumbersCounted)}`" +
                        $"{(this.LType == LeaderboardTypes.UsersHighestNumberCounted ? " - highest number counted" : " - total numbers counted")}");
                }
            }
            else
            {
                List<KaiaGuild> Guilds = DataStores.GuildStore.ReadAllAsync<KaiaGuild>().Result
                    .OrderByDescending(U => this.LType == LeaderboardTypes.GuildsHighestNumberCounted ? U.Settings.HighestCountEver : U.Settings.LastSuccessfulNumber)
                    .Take(this.NumberOfElements)
                    .ToList();
                int LongestDisplayName = Truncate(Strings.EmbedStrings.UnknownGuild).Length;
                foreach (KaiaGuild Guild in Guilds)
                {
                    SocketGuild? DGuild = this.Reference.GetGuild(Guild.Id);
                    if (DGuild != null && LongestDisplayName < Truncate(DGuild.Name).Length)
                    {
                        LongestDisplayName = Truncate(DGuild.Name).Length;
                    }
                }
                for (int Index = 0; Index < this.NumberOfElements && Guilds.Count > Index; Index++)
                {
                    KaiaGuild ThisGuild = Guilds[Index];
                    SocketGuild? DiscordGuild = this.Reference.GetGuild(ThisGuild.Id);
                    string DisplayName = Truncate(DiscordGuild != null ? DiscordGuild.Name : Strings.EmbedStrings.UnknownGuild);
                    DisplayName = DisplayName.Length < LongestDisplayName ? DisplayName + new string(' ', LongestDisplayName - DisplayName.Length) : DisplayName;
                    Displays.Add($"@`{DisplayName}` - " +
                        $"`{(this.LType == LeaderboardTypes.GuildsHighestNumberCounted ? ThisGuild.Settings.HighestCountEver : ThisGuild.Settings.LastSuccessfulNumber)}`" +
                        $"{(this.LType == LeaderboardTypes.GuildsHighestNumberCounted ? " - highest number counted" : " - current number")}");
                }
            }

            this.WithListWrittenToField(this.LType is LeaderboardTypes.GuildsHighestNumberCounted or LeaderboardTypes.GuildsCurrentHighestNumber ?
                Strings.EmbedStrings.FakePaths.Guilds : Strings.EmbedStrings.FakePaths.Users, Displays, "\n");
            return Task.CompletedTask;
        }
    }
}
