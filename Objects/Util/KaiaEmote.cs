namespace izolabella.Kaia.Bot.Objects.Util;

public class KaiaEmote : IEmote
{
    [JsonConstructor]
    public KaiaEmote(string Name)
    {
        this.Name = Name;
    }

    [JsonProperty("DisplayName")]
    public string Name { get; set; }

    public bool IsCustom => Emote.TryParse(this.Name, out Emote _);

    public override string ToString()
    {
        return !this.IsCustom ? this.Name : Emote.Parse(this.Name).ToString();
    }
}
