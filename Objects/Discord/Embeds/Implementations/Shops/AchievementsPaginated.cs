using Kaia.Bot.Objects.Constants.Embeds;
using Kaia.Bot.Objects.Constants.Enums;
using Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Bases;
using Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops
{
    public class AchievementsPaginated : KaiaPathEmbedPaginated
    {
        public AchievementsPaginated(CommandContext Context, AchievementFilter Filter, int ChunkSize) : base(new(),
                                                                    EmbedDefaults.DefaultEmbedForNoItemsPresent,
                                                                    Context,
                                                                    0,
                                                                    Strings.EmbedStrings.FakePaths.Users,
                                                                    Context.UserContext.User.Username)
        {
            this.U = new(Context.UserContext.User.Id);
            List<KaiaAchievement>? R = this.U.Settings.AchievementProcessor.GetUserAchievementsAsync().Result.Cast<KaiaAchievement>().ToList();
            R.AddRange(KaiaAchievementRoom.Achievements.Where(KaiaAch => !R.Any(RR => RR.Id == KaiaAch.Id)));
            IEnumerable<KaiaAchievement[]> Relevant = R.Where(Ach =>
            {
                return Filter == AchievementFilter.Complete ? Ach.UserAlreadyOwns(this.U).Result :
                       Filter == AchievementFilter.Incomplete ? !Ach.UserAlreadyOwns(this.U).Result :
                       Filter == AchievementFilter.All;
            }).OrderBy(Ach => Ach.Category).Chunk(ChunkSize);

            foreach (KaiaAchievement[] AchievementChunk in Relevant)
            {
                KaiaPathEmbed Embed = new(Strings.EmbedStrings.FakePaths.Users, Context.UserContext.User.Username, Strings.EmbedStrings.FakePaths.Achievements);
                List<SelectMenuOptionBuilder> B = new();
                foreach (KaiaAchievement Ach in AchievementChunk)
                {
                    Embed.WithField($"{Ach.DisplayEmote} {Ach.Title} : `earned: {(Ach.UserAlreadyOwns(this.U).Result ? Emotes.Counting.Check : Emotes.Counting.Invalid)}`", $"{Ach.GetDescriptionAsync(this.U).Result}");
                    B.Add(new ($"{Ach.Title}", Ach.Id.ToString(CultureInfo.InvariantCulture), Ach.GetDescriptionAsync(this.U).Result, Ach.DisplayEmote));
                }
                this.EmbedsAndOptions.Add(Embed, B);
            }

            this.ItemSelected += this.AchievementSelected;
        }

        KaiaUser U { get; }

        private async void AchievementSelected(KaiaPathEmbed Page, int ZeroBasedIndex, SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected)
        {
            if (this.Context.UserContext.IsValidToken)
            {
                await Component.DeferAsync();
                List<KaiaAchievement> UserEarned = (await this.U.Settings.AchievementProcessor.GetUserAchievementsAsync()).Cast<KaiaAchievement>().ToList();
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
                        A.Embed = EmbedDefaults.DefaultEmbedForNoItemsPresent.Build();
                        A.Components = new ComponentBuilder().Build();
                        A.Content = null;
                    });
                }
                this.Dispose();
            }
        }
    }
}
