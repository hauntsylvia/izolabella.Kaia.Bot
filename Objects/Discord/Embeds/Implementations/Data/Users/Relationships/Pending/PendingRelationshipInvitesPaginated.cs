using Kaia.Bot.Objects.KaiaStructures.Relationships;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.AlreadyIn
{
    public class PendingRelationshipInvitesPaginated : KaiaPathEmbedPaginated
    {
        public PendingRelationshipInvitesPaginated(CommandContext Context, int ChunkSize = 3) : base(new(),
                                                                                          Context,
                                                                                          0,
                                                                                          Strings.EmbedStrings.FakePaths.Users,
                                                                                          Context.UserContext.User.Username,
                                                                                          Strings.EmbedStrings.FakePaths.Relationships)
        {
            this.ChunkSize = ChunkSize;
            this.ItemSelected += this.RelSelAsync;
        }

        public int ChunkSize { get; }

        private async void RelSelAsync(KaiaPathEmbedRefreshable Page, int ZeroBasedIndex, SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected)
        {
            string? Item = ItemsSelected.FirstOrDefault();
            if (ulong.TryParse(Item, out ulong Id))
            {
                UserRelationship? Relationship = await DataStores.UserRelationshipsMainDirectory.ReadAsync<UserRelationship>(Id);
                if(Relationship != null)
                {

                }
            }
        }

        protected override async Task ClientRefreshAsync()
        {
            KaiaUser U = new(this.Context.UserContext.User.Id);

            IEnumerable<UserRelationship[]> Rels = (await U.RelationshipsProcessor.GetPendingRelationshipsAsync()).Chunk(this.ChunkSize);

            foreach (UserRelationship[] RelChunk in Rels)
            {
                List<SelectMenuOptionBuilder> Builds = new();
                foreach(UserRelationship R in RelChunk)
                {
                    Builds.Add(new($"relationship {R.Id}", R.Id.ToString(CultureInfo.InvariantCulture), R.Description, R.Emote, false));
                }
                MyRelationshipsPage Embed = new(this.Context, RelChunk);
                this.EmbedsAndOptions.Add(Embed, Builds);
            }
        }
    }
}
