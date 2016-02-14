using IHaveIdeas.Data;
using IHaveIdeas.Models;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace IHaveIdeas.ViewModel
{
    public class BrowseCardViewModel
    {
        /// <summary>
        /// Check if card is null
        /// If it is null, then return true
        /// </summary>
        /// <param name="pack">The pack to be check</param>
        /// <param name="card">The card  to be check</param>
        /// <returns>true if card in the pack is null</returns>
        /// <returns>false if card in the pack is not null </returns>
        public bool CheckIfCardIsNull(int pack, int card)
        {
            using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLitePlatformWinRT(), App.path))
            {
                try
                {
                    int size = db.Table<SavedCards>().Where(packs => packs.PackNumber == pack).Count();
                    if (size > card)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return true;
                }
             
            }
        }
    }
}
