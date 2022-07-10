﻿using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.Discord.Components;
using izolabella.Kaia.Bot.Objects.Discord.Embeds.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Relationships;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Users;

namespace izolabella.Kaia.Bot.Objects.Discord.Embeds.Implementations.Data.Users.Relationships.AlreadyIn
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
            MakeNew = new(Context, "Create", Emotes.Counting.Blessings, false, false);
            MakeNew.OnButtonPush += MakeNewRelationshipAsync;
            ItemSelected += RelationshipSelectedAsync;
        }

        public int ChunkSize { get; }

        public KaiaButton MakeNew { get; }

        private async void RelationshipSelectedAsync(KaiaPathEmbedRefreshable Page, int ZeroBasedIndex, SocketMessageComponent Component, IReadOnlyCollection<string> ItemsSelected)
        {
            string? Item = ItemsSelected.FirstOrDefault();
            if (ulong.TryParse(Item, out ulong Id))
            {
                UserRelationship? Relationship = await DataStores.UserRelationshipsMainDirectory.ReadAsync<UserRelationship>(Id);
                if (Relationship != null)
                {
                    await Component.DeferAsync();
                    await new RelationshipView(this, Context, Relationship).StartAsync(new KaiaUser(Component.User.Id));
                    Dispose();
                }
            }
        }

        private async Task MakeNewRelationshipAsync(SocketMessageComponent Arg, KaiaUser UserWhoPressed)
        {
            Dispose();
            await Arg.DeferAsync();
            await new CreateNewRelationshipView(this, Context).StartAsync(UserWhoPressed);
        }

        protected override async Task ClientRefreshAsync()
        {
            KaiaUser U = new(Context.UserContext.User.Id);

            IEnumerable<UserRelationship[]> Rels = (await U.RelationshipsProcessor.GetRelationshipsAsync()).Chunk(ChunkSize);

            foreach (UserRelationship[] RelChunk in Rels)
            {
                List<SelectMenuOptionBuilder> Builds = new();
                foreach (UserRelationship R in RelChunk)
                {
                    Builds.Add(new($"relationship {R.Id}", R.Id.ToString(CultureInfo.InvariantCulture), R.Description, R.Emote, false));
                }
                MyRelationshipsPage Embed = new(Context, 3, RelChunk);
                EmbedsAndOptions.Add(Embed, Builds);
            }
        }

        public override Task<IEnumerable<KaiaButton>?> GetExtraComponentsAsync()
        {
            return Task.FromResult<IEnumerable<KaiaButton>?>(new List<KaiaButton>() { MakeNew });
        }
    }
}
