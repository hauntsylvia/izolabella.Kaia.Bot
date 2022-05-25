using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.CCB_Structures.Books.Covers.Bases
{
    public class KaiaBook
    {
        public KaiaBook(string Title, string Author, int Pages, double Starting, double ExponentToIncreaseBy, double CostPerPage, double CostPerPageExponent, DateTime AvailableUntil)
        {
            this.Title = Title;
            this.Author = Author;
            this.Pages = Pages;
            this.Starting = Starting;
            this.ExponentToIncreaseBy = ExponentToIncreaseBy;
            this.CostPerPage = CostPerPage;
            this.CostPerPageExponent = CostPerPageExponent;
            this.AvailableUntil = AvailableUntil;
            this.CurrentBookStageIndex = 0;
            this.CurrentPage = 0;
        }

        public string Title { get; }
        public string Author { get; }
        public int Pages { get; }
        public double Starting { get; }
        public double ExponentToIncreaseBy { get; }
        public double CostPerPage { get; }
        public double CostPerPageExponent { get; }
        public int CurrentBookStageIndex { get; }
        public int CurrentPage { get; set; }
        public DateTime AvailableUntil { get; }
        public double CurrentEarning => Math.Pow(this.Starting * this.CurrentPage, this.ExponentToIncreaseBy);
    }
}
