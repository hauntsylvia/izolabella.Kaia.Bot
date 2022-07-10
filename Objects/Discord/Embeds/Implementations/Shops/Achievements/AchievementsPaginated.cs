using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Constants.Enums;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.KaiaAchievementRoom;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Achievements
{
    public class AchievementsPaginated : KaiaPathEmbedPaginated
    {
        public AchievementsPaginated(CommandContext Context, AchievementFilter Filter, int ChunkSize) : base(new(),
                                                                    Context,
                                                                    0,
                                                                    Strings.EmbedStrings.FakePaths.Users,
                                                                    Context.UserContext.User.Username)
        {
            U = new(Context.UserContext.User.Id);
            ItemSelected += AchievementSelected;
            this.Filter = Filter;
            this.ChunkSize = ChunkSize;
        }

        public AchievementFilter Filter { get; }

        public int ChunkSize { get; }

        KaiaUser U { get; }

        protected override async Task ClientRefreshAsync()
        {
            List<KaiaAchievement>? R = (await U.AchievementProcessor.GetUserAchievementsAsync()).Cast<KaiaAchievement>().ToList();
            R.AddRange(KaiaAchievementRoom.Achievements.Where(KaiaAch => !R.Any(RR => RR.Id == KaiaAch.Id)));
            IEnumerable<KaiaAchievement[]> Relevant = R.Where(Ach =>
            {
                return Filter == AchievementFilter.Complete ? Ach.UserAlreadyOwns(U).Result :
                       Filter == AchievementFilter.Incomplete ? !Ach.UserAlreadyOwns(U).Result :
                       Filter == AchievementFilter.All;
            }).OrderBy(Ach => Ach.Category).Chunk(ChunkSize);

            foreach (KaiaAchievement[] AchievementChunk in Relevant)
            {
                AchievementPaginatedPage Embed = new(AchievementChunk, Context, U);
                List<SelectMenuOptionBuilder> SelectMenu = new();
                foreach (KaiaAchievement Ach in AchievementChunk)
                {
                    SelectMenu.Add(new($"{Ach.Title}", Ach.Id.ToString(CultureInfo.InvariantCulture), Ach.GetDescriptionAsync(U).Result, Ach.DisplayEmote));
                }
                EmbedsAndOptions.Add(Embed, SelectMenu);
            }
        }
        private async void AchievementSelected(KaiaPathEmbedRefreshable Page, int ZeroBasedIndex, SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected)
        {
            if (Context.UserContext.IsValidToken)
            {
                await Component.DeferAsync();
                List<KaiaAchievement> UserEarned = (await U.AchievementProcessor.GetUserAchievementsAsync()).Cast<KaiaAchievement>().ToList();
                UserEarned.AddRange(KaiaAchievementRoom.Achievements.Where(KaiaAch => UserEarned.All(UserAch => UserAch.Id != KaiaAch.Id)));
                KaiaAchievement? Selected = UserEarned.FirstOrDefault(Ach => (ItemsSelected.FirstOrDefault() ?? "") == Ach.Id.ToString(CultureInfo.InvariantCulture));
                if (Selected != null)
                {
                    AchievementView V = new(this, Selected, Context);
                    await V.StartAsync(new(Component.User.Id));
                }
                else
                {
                    await Context.UserContext.ModifyOriginalResponseAsync(A =>
                    {
                        A.Embed = new ListOfItemsNotFound().Build();
                        A.Components = new ComponentBuilder().Build();
                        A.Content = null;
                    });
                }
                Dispose();
            }
        }
    }
}
