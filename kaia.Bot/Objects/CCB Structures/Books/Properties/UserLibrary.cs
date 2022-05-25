﻿using izolabella.Storage.Objects.DataStores;
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

        public UserLibrary(ulong U)
        {
            this.U = U;
        }
        public ulong U { get; }

        public async Task<List<KaiaBook>> GetUserBooksAsync()
        {
            List<KaiaBook> Books = await DataStores.GetUserBookStore(this.U).ReadAllAsync<KaiaBook>();
            Books.ForEach(B => B.BelongsTo = DataStores.GetUserBookStore(this.U));
            return Books;
        }

        public async Task<bool> UserHasBookOfIdAsync(string BookId)
        {
            return (await this.GetUserBooksAsync()).Any(B => B.BookId == BookId);
        }

        public async Task AddBookAsync(KaiaBook Push)
        {
            DataStore DS = DataStores.GetUserBookStore(this.U);
            if (!(await DS.ReadAllAsync<KaiaBook>()).Any(Book => Book.BookId == Push.BookId))
            {
                await DS.SaveAsync(Push);
            }
        }

        public async Task IncrementBookAsync(string BookId)
        {
            DataStore DS = DataStores.GetUserBookStore(this.U);
            KaiaBook? Matching = (await this.GetUserBooksAsync()).FirstOrDefault(B => B.BookId == BookId);
            if (Matching != null)
            {
                Matching.CurrentPageIndex++;
                await DS.SaveAsync(Matching);
            }
        }
    }
}
