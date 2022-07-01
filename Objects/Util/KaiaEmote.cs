namespace Kaia.Bot.Objects.Util
{
    public class KaiaEmote : IEmote
    {
        [JsonConstructor]
        public KaiaEmote(string Name)
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
