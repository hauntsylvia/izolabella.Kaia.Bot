namespace Kaia.Bot.Objects.CCB_Structures.Derivations
{
    public interface ISelfHandler
    {
        Task OnErrorAsync(Exception Exception);
    }
}
