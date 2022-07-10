using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Bases;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Exploration.Spells.Properties;
using izolabella.Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Bases;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Users
{
    [JsonObject(MemberSerialization.OptIn)]
    public class KaiaUserReward
    {
        public KaiaUserReward(double Petals)
        {
            this.Petals = Petals;
        }

        public KaiaUserReward(double Petals, params Spell[] Spells)
        {
            this.Petals = Petals;
            SpellIds = Spells.Select(S => S.Id).ToArray();
        }

        public KaiaUserReward(double Petals, params KaiaInventoryItem[] Items)
        {
            this.Petals = Petals;
            this.Items = Items;
        }

        public KaiaUserReward(double Petals, Spell[] Spells, params KaiaInventoryItem[] Items)
        {
            this.Petals = Petals;
            SpellIds = Spells.Select(S => S.Id).ToArray();
            this.Items = Items;
        }

        [JsonConstructor]
        public KaiaUserReward(double Petals, SpellId[]? SpellIds, KaiaInventoryItem[]? Items)
        {
            this.Petals = Petals;
            if (SpellIds != null)
            {
                this.SpellIds = SpellIds;
            }
            if (Items != null)
            {
                this.Items = Items;
            }
        }

        [JsonProperty("Petals")]
        public double Petals { get; }

        [JsonProperty("SpellIds")]
        private SpellId[]? SpellIds { get; }

        public IEnumerable<Spell> Spells => KaiaSpellsRoom.GetSpellsFromIds(SpellIds ?? Array.Empty<SpellId>());

        [JsonProperty("Items")]
        public KaiaInventoryItem[] Items { get; } = Array.Empty<KaiaInventoryItem>();
    }
}
