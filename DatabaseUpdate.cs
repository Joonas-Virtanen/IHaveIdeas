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
        /// Reorders saved cards pivot page cards after delete. 
        /// </summary>
        /// <param name="pack">The pack number where a card was removed</param>
        /// <param name="cardPosition">Position number of the card where card was removed</param>
        public void ReOrderSavedCards(int pack, int cardPosition)
        {
            int newCardPosition = cardPosition;
            using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
            {
                IEnumerable<SavedCards> reOrderQuery = db.Table<SavedCards>()
                .Where(card => card.PackNumber == pack && card.CardNumber > cardPosition);
                foreach (SavedCards savedCard in reOrderQuery)
                {
                    savedCard.CardNumber = newCardPosition;
                    db.Update(savedCard);
                    newCardPosition++;
                }
            }
        }
    }
}
