using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;

namespace izolabella.Kaia.Bot.Objects.Constants.Responses;

internal class Responses
{
    internal static async Task PipeErrors(CommandContext Context, KaiaPathEmbedRefreshable E)
    {
        await E.RefreshAsync();
        if (Context.UserContext.HasResponded)
        {
            await Context.UserContext.FollowupAsync(embed: E.Build(), ephemeral: true);
        }
        else
        {
            await Context.UserContext.RespondAsync(embed: E.Build(), ephemeral: true);
        }
    }
}
