namespace Kaia.Bot.Objects.Discord.MessageReceivers.Implementations
{
    internal class Achievements : IMessageReceiver
    {
        public string Name => "Achievements";

        public Task CallbackAsync(KaiaUser Author, SocketMessage Message, MessageReceiverResult CausedCallback)
        {
            return Task.CompletedTask;
        }

        public Task<bool> CheckMessageValidityAsync(KaiaUser Author, SocketMessage Message)
        {
            return Task.FromResult(true);
        }

        public Task OnErrorAsync(Exception Exception)
        {
            return Task.CompletedTask;
        }

        public async Task<MessageReceiverResult> RunAsync(KaiaUser Author, KaiaGuild? Guild, SocketMessage Message)
        {
            await Author.Settings.AchievementProcessor.TryAwardAchievements(Author, null, KaiaAchievementRoom.Achievements.ToArray());
            return new MessageReceiverResult(true, true, null);
        }
    }
}
