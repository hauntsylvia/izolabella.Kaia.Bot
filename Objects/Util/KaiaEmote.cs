namespace izolabella.Kaia.Bot.Objects.Util
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

        public bool IsCustom => Emote.TryParse(Name, out Emote _);

        public override string ToString()
        {
            return !IsCustom ? Name : Emote.Parse(Name).ToString();
        }
    }
}
