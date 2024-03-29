﻿using izolabella.Kaia.Bot.Objects.Constants;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Derivations;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Guilds
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class KaiaGuild : Unique
    {
        [JsonConstructor]
        public KaiaGuild(ulong Id, KaiaGuildSettings? Settings = null) : base(DataStores.GuildStore, Id)
        {
            this.Id = Id;
            this.settings = Settings ?? this.GetAsync<KaiaGuild>().Result?.Settings ?? new();
        }

        public new ulong Id { get; }

        private KaiaGuildSettings settings;

        [JsonProperty(nameof(Settings), Required = Required.Always)]
        public KaiaGuildSettings Settings
        {
            get => this.settings;
            set
            {
                this.settings = value;
                this.SaveAsync().GetAwaiter().GetResult();
            }
        }
    }
}