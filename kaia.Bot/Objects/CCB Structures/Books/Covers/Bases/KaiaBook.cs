using Kaia.Bot.Objects.CCB_Structures.Books.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.CCB_Structures.Books.Covers.Bases
{
    public class KaiaBook
    {
        public KaiaBook(string Title, string Author, decimal IncreasePerPage, IEnumerable<BookStage> Stages)
        {
            this.Title = Title;
            this.Author = Author;
            this.IncreasePerPage = IncreasePerPage;
            this.Stages = Stages;
        }

        public string Title { get; }
        public string Author { get; }
        public decimal IncreasePerPage { get; }
        public IEnumerable<BookStage> Stages { get; }
    }
}
