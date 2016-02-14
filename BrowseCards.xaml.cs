using IHaveIdeas.Data;
using IHaveIdeas.Models;
using IHaveIdeas.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using SQLite.Net.Platform.WinRT;
using SQLite.Net;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace IHaveIdeas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BrowseCards : Page
    {
        private string FolderPath = "DatabaseImages";
        private string ImageEnding = ".png";
        private string selectedPivotItem;

        BrowseCardViewModel vm = new BrowseCardViewModel();

        public BrowseCards()
        {
            this.InitializeComponent();
            CheckCardImages();
        }

        /// <summary>
        /// Checks what card images shoud be shown and places them
        /// </summary>
        public void CheckCardImages()
        {
            try
            {
                int card = 0;
                string name;
                using (SQLiteConnection db = new SQLiteConnection(new SQLitePlatformWinRT(), App.path))
                {
                    //Number of packs
                    int size = MainPage.numberOfPacks;
                    IList<int> savedCardList = null;
                    //Retrieves all savedcards
                    IEnumerable<SavedCards> query = db.Table<SavedCards>().OrderBy(x => x.CardNumber);
                    //Used to retrieve all cards in a pack
                    IEnumerable<int> packquery;
                    for (int pack = 1; pack <= size; pack++)
                    {
                        packquery = query.Where(x => x.PackNumber == pack).Select(x => x.Image);
                        savedCardList = packquery.ToList();
                        for (card = 0; card <= savedCardList.Count - 1; card++)// Place correct images to the buttons
                        {
                            Button button = new Button { };
                            name = "SavedCardP" + pack + "C" + card;
                            button = (Button)FindName(name);
                            var brush = new ImageBrush();
                            //Select a image that corresponds with card number
                            int image = savedCardList.ElementAt(card);
                            brush.ImageSource = new BitmapImage(new Uri(@"ms-appx:///" + FolderPath + "/" + Convert.ToString(image) + ImageEnding));
                            button.Background = brush;
                        }
                        for (int emptyCard = savedCardList.Count; emptyCard <= 11; emptyCard++) // Place empty images
                        {
                            Button button = new Button { };
                            name = "SavedCardP" + pack + "C" + emptyCard;
                            button = (Button)FindName(name);
                            var brush = new ImageBrush();
                            brush.ImageSource = new BitmapImage(new Uri(@"ms-appx:///Images/Card2.png"));
                            button.Background = brush;
                        }

                    }
                }
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
            }
        }

        public void SavedCard_Click(object sender, RoutedEventArgs e)
        {
            int packNumber;
            int cardNumber;

            Button button = sender as Button;
            string[] packAndCard = button.Tag.ToString().Split(' ');
            packNumber = Convert.ToInt32(packAndCard[0]);
            cardNumber = Convert.ToInt32(packAndCard[1]);

            if (vm.CheckIfCardIsNull(packNumber, cardNumber) == false)
            {
                this.Frame.Navigate(typeof(SavedCard.SavedCard), "Pack=" + packNumber + "&Card=" + cardNumber);
            }
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pivot pivot = sender as Pivot;
            PivotItem pivotItem = (PivotItem)pivot.SelectedItem;
            selectedPivotItem = pivotItem.Header.ToString();
            pivotItem.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(50, 50, 50, 50));
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void RemovePack_Click(object sender, RoutedEventArgs e)
        {
            DatabaseDelete delete = new DatabaseDelete();
            Button button = sender as Button;
            int pack = Convert.ToInt32(selectedPivotItem);
            delete.DeleteSavedPack(pack);
            CheckCardImages();
        }
    }
}
