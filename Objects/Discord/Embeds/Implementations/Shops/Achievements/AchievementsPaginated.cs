using Kaia.Bot.Objects.Constants.Enums;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds;
using Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Achievements
{
    public class AchievementsPaginated : KaiaPathEmbedPaginated
    {
        public AchievementsPaginated(CommandContext Context, AchievementFilter Filter, int ChunkSize) : base(new(),
                                                                    Context,
                                                                    0,
                                                                    Strings.EmbedStrings.FakePaths.Users,
                                                                    Context.UserContext.User.Username)
        {
            this.U = new(Context.UserContext.User.Id);
            List<KaiaAchievement>? R = this.U.AchievementProcessor.GetUserAchievementsAsync().Result.Cast<KaiaAchievement>().ToList();
            R.AddRange(KaiaAchievementRoom.Achievements.Where(KaiaAch => !R.Any(RR => RR.Id == KaiaAch.Id)));
            IEnumerable<KaiaAchievement[]> Relevant = R.Where(Ach =>
            {
                return Filter == AchievementFilter.Complete ? Ach.UserAlreadyOwns(this.U).Result :
                       Filter == AchievementFilter.Incomplete ? !Ach.UserAlreadyOwns(this.U).Result :
                       Filter == AchievementFilter.All;
            }).OrderBy(Ach => Ach.Category).Chunk(ChunkSize);

            foreach (KaiaAchievement[] AchievementChunk in Relevant)
            {
                AchievementPaginatedPage Embed = new(AchievementChunk, this.Context, this.U);
                List<SelectMenuOptionBuilder> SelectMenu = new();
                foreach (KaiaAchievement Ach in AchievementChunk)
                {
                    SelectMenu.Add(new($"{Ach.Title}", Ach.Id.ToString(CultureInfo.InvariantCulture), Ach.GetDescriptionAsync(this.U).Result, Ach.DisplayEmote));
                }
                this.EmbedsAndOptions.Add(Embed, SelectMenu);
            }

            this.ItemSelected += this.AchievementSelected;
        }

        KaiaUser U { get; }

        private async void AchievementSelected(KaiaPathEmbedRefreshable Page, int ZeroBasedIndex, SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected)
        {
            if (this.Context.UserContext.IsValidToken)
            {
                await Component.DeferAsync();
                List<KaiaAchievement> UserEarned = (await this.U.AchievementProcessor.GetUserAchievementsAsync()).Cast<KaiaAchievement>().ToList();
                UserEarned.AddRange(KaiaAchievementRoom.Achievements.Where(KaiaAch => UserEarned.All(UserAch => UserAch.Id != KaiaAch.Id)));
                KaiaAchievement? Selected = UserEarned.FirstOrDefault(Ach => (ItemsSelected.FirstOrDefault() ?? "") == Ach.Id.ToString(CultureInfo.InvariantCulture));
                if (Selected != null)
                {
                    AchievementView V = new(this, Selected, this.Context);
                    await V.StartAsync(new(Component.User.Id));
                }
                else
                {
                    await this.Context.UserContext.ModifyOriginalResponseAsync(A =>
                    {
                        A.Embed = new ListOfItemsNotFound().Build();
                        A.Components = new ComponentBuilder().Build();
                        A.Content = null;
                    });
                }
                this.Dispose();
            }
        }
    }
}
