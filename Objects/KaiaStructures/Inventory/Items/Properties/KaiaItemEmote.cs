namespace Kaia.Bot.Objects.KaiaStructures.Inventory.Items.Properties
{
    public class KaiaItemEmote : IEmote
    {
        [JsonConstructor]
        public KaiaItemEmote(string Name)
        {
            this.Name = Name;
        }

        [JsonProperty("Name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
