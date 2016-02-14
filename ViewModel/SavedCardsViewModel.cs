using IHaveIdeas.Data;
using IHaveIdeas.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Popups;

namespace IHaveIdeas.ViewModel
{
    public  class SavedCardsViewModel :INotifyPropertyChanged
    {

        private CardCurrent _currentModel;

        /// <summary>
        /// This is were UI elements are binded
        /// </summary>
        public CardCurrent currentModel
        {
            get { return _currentModel; }
            set
            {
                _currentModel = value;
                OnpropertyChanged();
            }
        }

        public SavedCardsViewModel()
        {
            currentModel = new CardCurrent();
        }


        /// <summary>
        /// Adds words and image to the card
        /// </summary>
        /// <param name="pack">Current packnumber</param>
        /// <param name="card">Current cardnumber</param>
        public async void AddItemsToCard(int pack, int card)
        {
            try
            {
                int cardIndex = 0;
                using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
                {
                    IEnumerable<SavedCards> query = db.Table<SavedCards>().Where(savedCard => savedCard.PackNumber == pack && savedCard.CardNumber == card);

                    if (cardIndex > -1)
                    {
                        var cardCurrent = new CardCurrent();
                        cardCurrent.Verb = query.First().VerbWord;
                        cardCurrent.Adjective = query.First().AdjectiveWord;
                        cardCurrent.Noun = query.First().NounWord;
                        cardCurrent.Image = query.First().Image;

                        currentModel = cardCurrent;
                    }
                    else
                    {
                        var dialog = new MessageDialog("Something went wrong. Program didn't found any cards");
                        await dialog.ShowAsync();
                    }
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                Debug.WriteLine(error.StackTrace);
            }
        }

        public void RemoveCard(int pack, int card)
        {
            DatabaseDelete delete = new DatabaseDelete();
            DatabaseUpdate update = new DatabaseUpdate();
            //Delete saved card
            delete.DeleteSavedCard(pack, card);
            //Reorder the database
            update.ReOrderSavedCards(pack, card);
        }
        public int GetPackSize(int pack)
        {
            int size=0;
            using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
            {
                size = db.Table<SavedCards>().Where(packs => packs.PackNumber == pack).Count();
            }
            return size;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnpropertyChanged([CallerMemberName] string propertyname = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
