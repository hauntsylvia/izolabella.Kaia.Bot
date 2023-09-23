namespace izolabella.Kaia.Bot.Objects.Util
{
    [method: JsonConstructor]
    public class KaiaEmote(string Name) : IEmote
    {
        [JsonProperty("DisplayName")]
        public string Name { get; set; } = Name;

        public bool IsCustom => Emote.TryParse(this.Name, out Emote _);

        public override string ToString()
        {
            return !this.IsCustom ? this.Name : Emote.Parse(this.Name).ToString();
        }
    }
}