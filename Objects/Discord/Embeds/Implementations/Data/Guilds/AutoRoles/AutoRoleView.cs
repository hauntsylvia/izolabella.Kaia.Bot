using Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.AutoRoles
{
    public class AutoRoleView : KaiaItemContentView
    {
        public AutoRoleView(AutoRolesPaginated? From, KaiaAutoRole Role, SocketGuild Guild, CommandContext Context) : base(From, Context, true)
        {
            this.Original = From;
            this.Role = Role;
            this.Guild = Guild;
            this.DeleteButton = new(Context, "Delete", Emotes.Counting.Invalid);
            this.DeleteButton.OnButtonPush += this.DeleteReactionRoleAsync;
        }

        public AutoRolesPaginated? Original { get; }

        public KaiaAutoRole Role { get; }

        public SocketGuild Guild { get; }

        public KaiaButton DeleteButton { get; }

        private async Task DeleteReactionRoleAsync(SocketMessageComponent Arg, KaiaUser UserWhoPressed)
        {
            await Arg.DeferAsync();
            KaiaGuild G = new(this.Guild.Id);
            KaiaAutoRole? R = G.Settings.AutoRoles.Find(K => K.Id == this.Role.Id);
            if (R != null)
            {
                G.Settings.AutoRoles.Remove(R);
                await G.SaveAsync();
            }
            if (this.Original != null)
            {
                this.Dispose();
                await this.Original.StartAsync();
            }
        }

        public override async Task<KaiaPathEmbedRefreshable> GetEmbedAsync(KaiaUser? U = null)
        {
            AutoRoleViewRaw V = new(this.Guild, this.Role);
            await V.RefreshAsync();
            return V;
        }

        public async Task<ComponentBuilder> GetComponentsAsync()
        {
            ComponentBuilder CB = (await this.GetDefaultComponents()).WithButton(this.DeleteButton.WithDisabled(false));
            return CB;
        }

        public override async Task StartAsync(KaiaUser? U = null)
        {
            if (!this.Context.UserContext.HasResponded)
            {
                await this.Context.UserContext.RespondAsync(Strings.EmbedStrings.Empty);
            }
            KaiaPathEmbedRefreshable E = await this.GetEmbedAsync(U);
            ComponentBuilder Com = await this.GetComponentsAsync();
            await this.Context.UserContext.ModifyOriginalResponseAsync(M =>
            {
                M.Content = Strings.EmbedStrings.Empty;
                M.Components = Com.Build();
                M.Embed = E.Build();
            });
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            this.DeleteButton.Dispose();
        }
    }
}
