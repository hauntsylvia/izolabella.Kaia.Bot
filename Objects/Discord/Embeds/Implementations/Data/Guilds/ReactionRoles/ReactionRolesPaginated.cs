using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Constants.Responses;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds.Roles;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.ErrorEmbeds;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Guilds.ReactionRoles
{
    public class ReactionRolesPaginated : KaiaPathEmbedPaginated
    {
        public ReactionRolesPaginated(CommandContext Context, SocketGuild Guild, bool Private)
            : base(new(), Context, 0, Strings.EmbedStrings.FakePaths.Guilds, Guild.Name, Strings.EmbedStrings.FakePaths.ReactionRoles, Private: Private)
        {
            this.Guild = Guild;
            ItemSelected += ItemSelectedAsync;
        }

        public SocketGuild Guild { get; }

        protected override async Task ClientRefreshAsync()
        {
            IEnumerable<KaiaReactionRole> Roles = new KaiaGuild(Guild.Id).Settings.ReactionRoles;

            foreach (KaiaReactionRole[] Chunk in Roles.Chunk(4))
            {
                ReactionRolesPage Embed = new(Guild, Chunk);
                List<SelectMenuOptionBuilder> B = new();
                foreach (KaiaReactionRole Role in Chunk)
                {
                    IRole? R = await Role.GetRoleAsync(Guild);
                    B.Add(new(R?.Name ?? Role.Id.ToString(CultureInfo.InvariantCulture), Role.Id.ToString(CultureInfo.InvariantCulture), R?.Name ?? Strings.EmbedStrings.Empty, Role.Emote.IsCustom ? Emote.Parse(Role.Emote.ToString()) : Role.Emote));
                }
                EmbedsAndOptions.Add(Embed, B);
            }
        }

        private async void ItemSelectedAsync(KaiaPathEmbedRefreshable Page, int ZeroBasedIndex, SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected)
        {
            string? RoleStr = ItemsSelected.FirstOrDefault();
            if (RoleStr != null && ulong.TryParse(RoleStr, out ulong RoleId))
            {
                KaiaReactionRole? Role = new KaiaGuild(Guild.Id).Settings.ReactionRoles.FirstOrDefault(R => R.Id == RoleId);
                if (Role != null)
                {
                    ReactionRoleView V = new(this, Role, Guild, Context);
                    await Component.DeferAsync(true);
                    Dispose();
                    await V.StartAsync(null);
                }
                else
                {
                    await Responses.PipeErrors(Context, new SingleItemNotFound());
                }
            }
        }
    }
}
