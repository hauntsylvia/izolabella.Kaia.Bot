using izolabella.Kaia.Bot.Objects.KaiaStructures.Derivations;

namespace izolabella.Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases
{
    [JsonObject(MemberSerialization.OptIn)]
    [method: JsonConstructor]
    public class KaiaBook(string Title, string Author, int Pages, double Starting, double ExponentToIncreaseBy, double CostPerPage, double CostPerPageExponent, DateTime AvailableUntil, ulong Id) : Unique(null, Id)
    {
        [JsonProperty(nameof(Title))]
        public string Title { get; } = Title;
        [JsonProperty(nameof(Author))]
        public string Author { get; } = Author;
        [JsonProperty(nameof(Pages))]
        public int Pages { get; } = Pages;
        [JsonProperty(nameof(Starting))]
        private double Starting { get; } = Starting;
        [JsonProperty(nameof(ExponentToIncreaseBy))]
        private double ExponentToIncreaseBy { get; } = ExponentToIncreaseBy;
        [JsonProperty(nameof(CostPerPage))]
        private double CostPerPage { get; } = CostPerPage;
        [JsonProperty(nameof(CostPerPageExponent))]
        private double CostPerPageExponent { get; } = CostPerPageExponent;
        [JsonProperty(nameof(CurrentPageIndex))]
        public int CurrentPageIndex { get; set; } = 0;
        [JsonProperty(nameof(AvailableUntil))]
        public DateTime AvailableUntil { get; } = AvailableUntil;
        public double CurrentEarning => Math.Round(Math.Pow(this.Starting * this.CurrentPageIndex, this.ExponentToIncreaseBy), 2);
        public double NextPageEarning => Math.Round(Math.Pow(this.Starting * (this.CurrentPageIndex + 1), this.ExponentToIncreaseBy), 2);
        public double NextPageTurnCost => Math.Round(Math.Pow(this.CostPerPage * (this.CurrentPageIndex + 1), this.CostPerPageExponent), 2);
        [JsonProperty(nameof(BookId))]
        public string BookId => $"{this.Title}-{(this.AvailableUntil - new DateTime(1970, 1, 1)).TotalMilliseconds}";

        public bool IsFinished => this.CurrentPageIndex >= this.Pages;
    }
}