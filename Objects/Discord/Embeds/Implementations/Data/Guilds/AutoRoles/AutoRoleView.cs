using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Components;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.AutoRoles
{
    public class AutoRoleView : KaiaItemContentView
    {
        public AutoRoleView(AutoRolesPaginated? From, KaiaAutoRole Role, SocketGuild Guild, CommandContext Context) : base(From, Context, true)
        {
            Original = From;
            this.Role = Role;
            this.Guild = Guild;
            DeleteButton = new(Context, "Delete", Emotes.Counting.Invalid);
            DeleteButton.OnButtonPush += DeleteReactionRoleAsync;
        }

        public AutoRolesPaginated? Original { get; }

        public KaiaAutoRole Role { get; }

        public SocketGuild Guild { get; }

        public KaiaButton DeleteButton { get; }

        private async Task DeleteReactionRoleAsync(SocketMessageComponent Arg, KaiaUser UserWhoPressed)
        {
            await Arg.DeferAsync();
            KaiaGuild G = new(Guild.Id);
            KaiaAutoRole? R = G.Settings.AutoRoles.Find(K => K.Id == Role.Id);
            if (R != null)
            {
                G.Settings.AutoRoles.Remove(R);
                await G.SaveAsync();
            }
            if (Original != null)
            {
                Dispose();
                await Original.StartAsync();
            }
        }

        public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser? U = null)
        {
            AutoRoleViewRaw V = new(Guild, Role);
            await V.RefreshAsync();
            return V;
        }

        public async Task<ComponentBuilder> GetComponentsAsync()
        {
            ComponentBuilder CB = (await GetDefaultComponents()).WithButton(DeleteButton.WithDisabled(false));
            return CB;
        }

        public override async Task StartAsync(KaiaUser? U = null)
        {
            if (!Context.UserContext.HasResponded)
            {
                await Context.UserContext.RespondAsync(Strings.EmbedStrings.Empty);
            }
            KaiaPathEmbedRefreshable E = await GetEmbedAsync(U);
            ComponentBuilder Com = await GetComponentsAsync();
            await Context.UserContext.ModifyOriginalResponseAsync(M =>
            {
                M.Content = Strings.EmbedStrings.Empty;
                M.Components = Com.Build();
                M.Embed = E.Build();
            });
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            DeleteButton.Dispose();
        }
    }
}
