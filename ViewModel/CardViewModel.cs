using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IHaveIdeas.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.UI.Xaml.Data;
using System.Diagnostics;

namespace IHaveIdeas.ViewModel
{
    public class CardViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// imageNumber is used when selecting an image from the DatabaseImages folder. 
        /// Example imageNumber 1 is image "1.png".
        /// It is image file's name but not the image it self.
        /// </summary>
        public int imageNumber;
        public int id;
        /// <summary>
        /// Is index number, which is used to show previous card.
        /// </summary>
        public int back;
        /// <summary>
        /// How many cards user has. Not same as saved cards. 
        /// </summary>
        int numberOfCards;
        bool firstStart;
        /// <summary>
        /// When "Previous Card" is pressed, program will decrease goingBackNumberOfCards by one.  Less than 0 means user is browsing previous cards. The app will only give user a new card when goingBackNumberOfCards is 0.
        /// It tells the app how many times the user has pressed "previous card".
        /// </summary>
        public int goingBackNumberOfCards;
        /// <summary>
        /// If we are browsing old cards, the variable is true.
        /// If we are browsing new cards, the variable is false
        /// </summary>
        bool goingBack;
        /// <summary>
        /// When user has made changes to previous cards previousCardModified is true. It is used the change generated card in the database.
        /// </summary>
        bool previousCardModified;
        // bool newFirstCardisGoingToBePicked;

        List<Verbs> verbs = new List<Verbs>();
        List<Adjectives> adjectives = new List<Adjectives>();
        List<Nouns> nouns = new List<Nouns>();
        List<Images> images = new List<Images>();

        private CardCurrent _currentCard;

        public CardCurrent currentModel
        {
            get { return _currentCard; }
            set
            {
                _currentCard = value;
                OnpropertyChanged();
            }
        }

        public CardViewModel()
        {
            try
            {
                currentModel = new CardCurrent();
                firstStart = true;
                previousCardModified = false;
                goingBack = false;
                goingBackNumberOfCards = 1;
                imageNumber = 0;
                id = 0;

                //There was a problem with database initialization
                using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
                {
                    verbs = db.Table<Verbs>().ToList();
                    adjectives = db.Table<Adjectives>().ToList();
                    nouns = db.Table<Nouns>().ToList();
                    images = db.Table<Images>().ToList();
                    if (verbs.Count == 0 || adjectives.Count == 0 || nouns.Count == 0 || images.Count == 0)
                    {
                        Debug.WriteLine("Database not initialized");
                        App app = new App();
                        app.Initialize();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
        }

        public async void SavePack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isAlreadyInDatabase = false;
                AppBarButton button = new AppBarButton { };
                button = sender as AppBarButton;
                int packnumber = 1;
                packnumber = Convert.ToInt32(button.Label.Substring(13, 1));

                using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
                {
                    int sizeQuery = db.Table<SavedCards>().Where(packs => packs.PackNumber == packnumber && packs.NounWord == currentModel.Noun &&
                                                                                        packs.Image == currentModel.Image &&
                                                                                        packs.VerbWord == currentModel.Verb &&
                                                                                        packs.AdjectiveWord == currentModel.Adjective).Count();

                    if (sizeQuery > 0)
                    {
                        isAlreadyInDatabase = true;
                    }

                    if (isAlreadyInDatabase == false)
                    {
                        int size = db.Table<SavedCards>().Where(me => me.PackNumber == packnumber).Count();
                        if (size < 12)
                        {
                            try
                            {
                                SavedCards savedcard = new SavedCards() { VerbWord = currentModel.Verb, AdjectiveWord = currentModel.Adjective, NounWord = currentModel.Noun, Image = currentModel.Image, CardNumber = size, PackNumber = packnumber };
                                db.Insert(savedcard);
                            }
                            catch (Exception error)
                            {
                                Debug.WriteLine(error.Message);
                            }
                        }
                        else
                        {
                            var dialog = new MessageDialog("Pack " + packnumber + " has maximum number of cards!");
                            await dialog.ShowAsync();
                        }
                    }
                    else
                    {
                        var dialog = new MessageDialog("This card is already in the database");
                        await dialog.ShowAsync();
                    }
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
            }
        }

        /// <summary>
        /// Deletes every card in genereatedCards.
        /// </summary>
        public void ClearGeneratedCards()
        {
            using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
            {
                int size = db.Table<GeneratedCards>().Count();

                if (size > 0)
                {
                    IEnumerable<GeneratedCards> entityQuery = db.Table<GeneratedCards>().ToList();
                    db.DeleteAll<GeneratedCards>();
                }
            }

        }

        /// <summary>
        /// Loads previous card
        /// </summary>
        public void Previous_Button_Click(object sender, RoutedEventArgs e)
        {
            if (firstStart == false)
            {
                using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
                {
                    goingBackNumberOfCards -= 1;
                    int count = db.Table<GeneratedCards>().Count();
                    //We are browsing previous card and saving generated card. It's the first time user have pressed "previous" button when genereting a new card
                    if (goingBackNumberOfCards == -1 && goingBack == false)
                    {
                        SaveGeneratedCard(false, 0);
                        previousCardModified = false;
                        id += 1;
                    }
                    if (goingBackNumberOfCards < -1 && previousCardModified == true && Math.Abs(goingBackNumberOfCards) != count)
                    {
                        SaveGeneratedCard(true, back);
                        previousCardModified = false;
                    }

                    if (previousCardModified == true)
                    {
                        SaveGeneratedCard(true, back);
                        previousCardModified = false;
                    }

                    goingBack = true;

                    if (Math.Abs(goingBackNumberOfCards) == count + 1) //Check if card now is the first card. If it is the first card--> do nothing.
                    {
                        goingBackNumberOfCards += 1;
                    }
                    else
                    {
                        if (id > 1)//if user has two cards
                        {
                            back = count + goingBackNumberOfCards;
                            var card = new CardCurrent();

                            card.Verb = db.Table<GeneratedCards>().Skip(back).First().VerbWord;
                            card.Adjective = db.Table<GeneratedCards>().Skip(back).First().AdjectiveWord;
                            card.Noun = db.Table<GeneratedCards>().Skip(back).First().NounWord;
                            card.Image = db.Table<GeneratedCards>().Skip(back).First().Image;
                            currentModel = card;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Generated a new card
        /// </summary>
        public void NewCard_Click(object sender, RoutedEventArgs e)
        {
            if (goingBackNumberOfCards >= 0) //If we are not in browsing previous cards. Generate a new card.
            {
                GetNewCard();
                numberOfCards++;
            }
            else //We are browsing previous cards.
            {
                GetOldCard();
            }
        }

        public void GetOldCard()
        {
            using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
            {
                int count = db.Table<GeneratedCards>().Count() - 1;
                if (goingBackNumberOfCards <= -1 && previousCardModified == true)
                {
                    SaveGeneratedCard(true, back);
                    previousCardModified = false;
                }

                goingBackNumberOfCards++;
                back = count + goingBackNumberOfCards;

                var card = new CardCurrent();

                card.Verb = db.Table<GeneratedCards>().Skip(back).First().VerbWord;
                card.Adjective = db.Table<GeneratedCards>().Skip(back).First().AdjectiveWord;
                card.Noun = db.Table<GeneratedCards>().Skip(back).First().NounWord;
                card.Image = db.Table<GeneratedCards>().Skip(back).First().Image;
                currentModel = card;

            }

        }
        public async void GetNewCard()
        {
            using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
            {
                int i = 0;
                Random rnd = new Random();
                try
                {
                    goingBack = false;
                    if (goingBackNumberOfCards <= 0)
                    {
                        SaveGeneratedCard(false, 0);
                        previousCardModified = false;
                    }
               
                    //If previous card was an old card and it was modified
                    if (goingBackNumberOfCards == 0 && previousCardModified == true)
                    {
                        SaveGeneratedCard(true, back);
                        previousCardModified = false;
                    }

                    if (goingBackNumberOfCards == 1)
                    {
                        goingBackNumberOfCards = 0;
                    }
                    id += 1;

                 
                    imageNumber = 0;
                    var card = new CardCurrent();
                    //Example values:
                    //card.Verb = "help";
                    //card.Adjective = "worried";
                    //card.Noun = "girls";
                    //card.Image = 143;
                    i = rnd.Next(db.Table<Verbs>().Count());
                    card.Verb = db.Table<Verbs>().Skip(i).First().VerbWord;
                    i = rnd.Next(db.Table<Adjectives>().Count());
                    card.Adjective = db.Table<Adjectives>().Skip(i).First().AdjectiveWord;
                    i = rnd.Next(db.Table<Nouns>().Count());
                    card.Noun = db.Table<Nouns>().Skip(i).First().NounWord;
                    i = rnd.Next(1, db.Table<Images>().Count());
                    card.Image = db.Table<Images>().Skip(i).First().ImageNumber;

                    previousCardModified = false;

                    currentModel = card;

                    firstStart = false;
                }
                catch (Exception error)
                {
                    Debug.WriteLine(db.Table<Verbs>().Count());
                    Debug.WriteLine(db.Table<Adjectives>().Count());
                    Debug.WriteLine(db.Table<Nouns>().Count());
                    Debug.WriteLine(db.Table<Images>().Count());
                    var dialog = new MessageDialog("Error on " + error.Message + " Stacktrace " + error.StackTrace);
                    await dialog.ShowAsync();
                    Debug.WriteLine(error.Message + "||" + error.StackTrace);
                }
            }
        }

        /// <summary>
        /// Gets random image
        /// </summary>
        public void GetImage()
        {
            using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
            {
                Random random = new Random();
                currentModel.Image = random.Next(db.Table<Images>().Count() + 1);
            }
            previousCardModified = false;
        }

        public void VerbiTextBlock_ManipulationCompleted()
        {
            using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
            {
                int i = 0;
                Random rnd = new Random();
                i = rnd.Next(db.Table<Verbs>().Count());
                currentModel.Verb = db.Table<Verbs>().Skip(i).First().VerbWord;
            }
            if (goingBackNumberOfCards < 0)
            {
                previousCardModified = true;
            }
            if (goingBackNumberOfCards == 0)
            {
                previousCardModified = true;
            }
        }

        public void RandomKuva_ManipulationCompleted()
        {
            using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
            {
                Random rnd = new Random();
                currentModel.Image = rnd.Next(db.Table<Images>().Count());
            }
            if (goingBackNumberOfCards < 0)
            {
                previousCardModified = true;
            }
            if (goingBackNumberOfCards == 0)
            {
                previousCardModified = true;
            }
        }

        public void AdjektiiviTextBlock_ManipulationCompleted()
        {
            using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
            {
                int i = 0;
                Random rnd = new Random();
                i = rnd.Next(db.Table<Adjectives>().Count());
                currentModel.Adjective = db.Table<Adjectives>().Skip(i).First().AdjectiveWord;
            }

            if (goingBackNumberOfCards < 0)
            {
                previousCardModified = true;
            }
            if (goingBackNumberOfCards == 0)
            {
                previousCardModified = true;
            }
        }

        public void SubsanttiiviTextBlock_ManipulationCompleted()
        {
            using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
            {
                int i = 0;
                Random rnd = new Random();
                i = rnd.Next(db.Table<Nouns>().Count());
                currentModel.Noun = db.Table<Nouns>().Skip(i).First().NounWord;
            }
            if (goingBackNumberOfCards < 0)
            {
                previousCardModified = true;
            }
            if (goingBackNumberOfCards == 0)
            {
                previousCardModified = true;
            }
        }
        /// <summary>
        /// Used to save a card, when user is browsing generated cards.
        /// </summary>
        /// <param name="overWrite">If we are overwriting existing genererated card then overWrite should be true, else false</param>
        /// <param name="index">Generated card index which we are overwriting</param>
        public void SaveGeneratedCard(bool overWrite, int index)
        {
            using (SQLite.Net.SQLiteConnection db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path))
            {
                if (overWrite == false)
                {
                    GeneratedCards generatedCards = new GeneratedCards() { VerbWord = currentModel.Verb, AdjectiveWord = currentModel.Adjective, NounWord = currentModel.Noun, Image = currentModel.Image };
                    try
                    {
                        db.Insert(generatedCards);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
                else
                {
                    try
                    {
                        GeneratedCards generatedCard = db.Table<GeneratedCards>().Skip(index).First();
                        generatedCard.Image = currentModel.Image;
                        generatedCard.VerbWord = currentModel.Verb;
                        generatedCard.AdjectiveWord = currentModel.Adjective;
                        generatedCard.NounWord = currentModel.Noun;
                        db.Update(generatedCard);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }
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

