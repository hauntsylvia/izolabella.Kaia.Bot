using Kaia.Bot.Objects.CCB_Structures.Books.Covers.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Bot.Objects.CCB_Structures.Books.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserLibrary
    {
        [JsonConstructor]
        public UserLibrary(params KaiaBook[]? Items)
        {
            this.items = Items?.ToList() ?? new();
        }

        [JsonProperty("Items", Required = Required.Default)]
        private readonly List<KaiaBook> items;
        public IReadOnlyList<KaiaBook> Items => this.items;

        public void AddBook(KaiaBook Push)
        {
            if(!this.items.Any(Book => Book.Equals(Push)))
            {
                this.items.Add(Push);
            }
        }
    }
}
