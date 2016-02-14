using System.Collections.Generic;
using IHaveIdeas.Models;

namespace IHaveIdeas.Data
{
    class DatabaseDelete
    {
        /// <summary>
        /// Deletes a individual card in a pack
        /// </summary>
        /// <param name="pack">A packnumber where you want the card to be removed</param>
        /// <param name="card">Cardnumber the be removed</param>
        public void DeleteSavedCard(int pack, int card)
        {
            using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
            {
                IEnumerable<SavedCards> entityQuery = db.Table<SavedCards>().Where(x => x.PackNumber == pack && x.CardNumber == card);  // from savedCards in context.SavedCardPacks where savedCards.packNumber == pack && savedCards.packCard == card select savedCards;
                foreach (SavedCards savedCard in entityQuery)
                {
                    db.Delete(savedCard);
                }
            }
        }
        /// <summary>
        /// Deletes all cards in a pack
        /// </summary>
        /// <param name="pack">Pack which you want to remove</param>
        public void DeleteSavedPack(int pack)
        {
            using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
            {
                IEnumerable<SavedCards> packQuery = db.Table<SavedCards>().Where(card => card.PackNumber == pack);
                foreach (SavedCards savedCard in packQuery)
                {
                    db.Delete(savedCard);
                }
            }
        }

    }
}