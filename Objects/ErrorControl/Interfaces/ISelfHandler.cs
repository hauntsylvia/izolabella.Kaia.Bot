namespace Kaia.Bot.Objects.ErrorControl.Interfaces
{
    public interface ISelfHandler
    {
        Task OnErrorAsync(Exception Exception);
    }
}
