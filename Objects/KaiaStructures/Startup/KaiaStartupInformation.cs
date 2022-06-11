﻿using izolabella.Storage.Objects.DataStores;
using Kaia.Bot.Objects.KaiaStructures.Derivations;

namespace Kaia.Bot.Objects.KaiaStructures.Startup
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemRequired = Required.Always)]
    public class StartupProfile : Unique
    {
        [JsonConstructor]
        public StartupProfile(DataStore Belongs, string Token) : base(Belongs)
        {
            this.Token = Token;
        }

        [JsonProperty("Token", Required = Required.Always)]
        public string Token { get; }
    }
}
