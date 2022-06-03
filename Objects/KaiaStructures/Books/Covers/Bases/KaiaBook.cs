using Kaia.Bot.Objects.KaiaStructures.Derivations;

namespace Kaia.Bot.Objects.KaiaStructures.Books.Covers.Bases
{
    [JsonObject(MemberSerialization.OptIn)]
    public class KaiaBook : Unique
    {
        [JsonConstructor]
        public KaiaBook(string Title, string Author, int Pages, double Starting, double ExponentToIncreaseBy, double CostPerPage, double CostPerPageExponent, DateTime AvailableUntil, ulong Id) : base(null, Id)
        {
            this.Title = Title;
            this.Author = Author;
            this.Pages = Pages;
            this.Starting = Starting;
            this.ExponentToIncreaseBy = ExponentToIncreaseBy;
            this.CostPerPage = CostPerPage;
            this.CostPerPageExponent = CostPerPageExponent;
            this.AvailableUntil = AvailableUntil;
            this.CurrentPageIndex = 0;
        }

        [JsonProperty("Title")]
        public string Title { get; }
        [JsonProperty("Author")]
        public string Author { get; }
        [JsonProperty("Pages")]
        public int Pages { get; }
        [JsonProperty("Starting")]
        private double Starting { get; }
        [JsonProperty("ExponentToIncreaseBy")]
        private double ExponentToIncreaseBy { get; }
        [JsonProperty("CostPerPage")]
        private double CostPerPage { get; }
        [JsonProperty("CostPerPageExponent")]
        private double CostPerPageExponent { get; }
        [JsonProperty("CurrentPageIndex")]
        public int CurrentPageIndex { get; set; }
        [JsonProperty("AvailableUntil")]
        public DateTime AvailableUntil { get; }
        public double CurrentEarning => Math.Round(Math.Pow(this.Starting * this.CurrentPageIndex, this.ExponentToIncreaseBy), 2);
        public double NextPageEarning => Math.Round(Math.Pow(this.Starting * (this.CurrentPageIndex + 1), this.ExponentToIncreaseBy), 2);
        public double NextPageTurnCost => Math.Round(Math.Pow(this.CostPerPage * (this.CurrentPageIndex + 1), this.CostPerPageExponent), 2);
        [JsonProperty("BookId")]
        public string BookId => $"{this.Title}-{(this.AvailableUntil - new DateTime(1970, 1, 1)).TotalMilliseconds}";

        public bool IsFinished => this.CurrentPageIndex >= this.Pages;
    }
}
