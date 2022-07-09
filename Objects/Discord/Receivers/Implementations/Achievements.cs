using Discord.Net;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Achievements.Classes.KaiaAchievementRoom;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Receivers.Implementations
{
    public class Achievements : IzolabellaMessageReceiver
    {
        public override string Name => "Achievements";

        public override Predicate<SocketMessage> ValidPredicate => (Arg) => true;

        public override Task OnErrorAsync(HttpException Exception)
        {
            return Task.CompletedTask;
        }

        public override async Task OnMessageAsync(IzolabellaDiscordClient Reference, SocketMessage Message)
        {
            KaiaUser Author = new(Message.Author.Id);
            await Author.AchievementProcessor.TryAwardAchievements(Author, null, KaiaAchievementRoom.Achievements.ToArray());
        }
    }
}
