using izolabella.Util;
using Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships;
using Kaia.Bot.Objects.KaiaStructures.Inventory.Properties;
using Kaia.Bot.Objects.KaiaStructures.Relationships;

namespace Kaia.Bot.Objects.Discord.Embeds.Implementations.Shops.Items
{
    public class MyRelationshipsPaginated : KaiaPathEmbedPaginated
    {
        public MyRelationshipsPaginated(CommandContext Context, int ChunkSize = 3) : base(new(),
                                                                                          Context,
                                                                                          0,
                                                                                          Strings.EmbedStrings.FakePaths.Users,
                                                                                          Context.UserContext.User.Username,
                                                                                          Strings.EmbedStrings.FakePaths.Relationships)
        {
            this.ChunkSize = ChunkSize;
            this.MakeNew = new(Context, "Create", Emotes.Counting.Blessings, false, false);
            this.MakeNew.OnButtonPush += this.MakeNewRelationshipAsync;
        }

        public int ChunkSize { get; }

        public KaiaButton MakeNew { get; }

        private async Task MakeNewRelationshipAsync(SocketMessageComponent Arg, KaiaUser UserWhoPressed)
        {
            await Arg.DeferAsync();
            await new CreateNewRelationshipView(this, this.Context).StartAsync(UserWhoPressed);
        }

        protected override async Task ClientRefreshAsync()
        {
            KaiaUser U = new(this.Context.UserContext.User.Id);

            IEnumerable<UserRelationship[]> Rels = (await U.RelationshipsProcessor.GetRelationshipsAsync()).Chunk(this.ChunkSize);

            foreach (UserRelationship[] RelChunk in Rels)
            {
                MyRelationshipsPage Embed = new(this.Context, RelChunk);
                this.EmbedsAndOptions.Add(Embed, null);
            }
        }

        public override Task<IEnumerable<KaiaButton>?> GetExtraComponentsAsync()
        {
            return Task.FromResult<IEnumerable<KaiaButton>?>( new List<KaiaButton>() { this.MakeNew });
        }
    }
}
