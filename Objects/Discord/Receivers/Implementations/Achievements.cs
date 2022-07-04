namespace Kaia.Bot.Objects.Discord.Receivers.Implementations
{
    public class Achievements : Receiver
    {
        public override string Name => "Achievements";

        public override Task CallbackAsync(KaiaUser Author, SocketMessage Message, ReceiverResult CausedCallback)
        {
            return Task.CompletedTask;
        }

        public override Task<bool> CheckMessageValidityAsync(KaiaUser Author, SocketMessage Message)
        {
            return Task.FromResult(true);
        }

        public override Task OnErrorAsync(Exception Exception)
        {
            return Task.CompletedTask;
        }

        public override async Task<ReceiverResult> OnMessageAsync(KaiaUser Author, KaiaGuild? Guild, SocketMessage Message)
        {
            await Author.AchievementProcessor.TryAwardAchievements(Author, null, KaiaAchievementRoom.Achievements.ToArray());
            return new ReceiverResult();
        }
    }
}
