using IHaveIdeas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHaveIdeas.Data
{
    class DatabaseUpdate
    {
        /// <summary>
        /// Reorders saved cards after delete. 
        /// </summary>
        /// <param name="pack">The pack where a card was removed</param>
        /// <param name="card">Position of the card where it was removed</param>
        public void ReOrderSavedCards(int pack, int card)
        {
            int newCardNumber = card;
            using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
            {
                IEnumerable<SavedCards> reOrderQuery = db.Table<SavedCards>()
                .Where(x => x.PackNumber == pack && x.CardNumber > card);
                foreach (SavedCards savedCard in reOrderQuery)
                {
                    savedCard.CardNumber = newCardNumber;
                    db.Update(savedCard);
                    newCardNumber++;
                }
            }
        }
    }
}
