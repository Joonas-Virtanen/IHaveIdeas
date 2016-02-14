using System;
using IHaveIdeas.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using System.Diagnostics;

namespace IHaveIdeas.SavedCard
{
    public partial class SavedCard : Page
    {


        /// <summary>
        /// The pack which the user has pressed in BrowseCards.xaml page.
        /// It is the current pack.
        /// </summary>
        int pack;
        /// <summary>
        ///The card which the user has pressed in BrowseCards.xaml page.
        /// It the current card number.
        /// </summary>
        int card;
        public SavedCardsViewModel vm;

        public SavedCard()
        {
            InitializeComponent();
            vm = new SavedCardsViewModel();
            this.DataContext = vm;
        }

        /// <summary>
        /// Reads the parameter values from BrowseCards.xaml page and places the appropriate values to the saved card. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                string parameter1 = string.Empty;
                pack = 0;
                string parameter2 = string.Empty;
                card = 0;
                string parameter = e.Parameter as string;
                int startIndex = parameter.IndexOf("=");
                int endIndex = parameter.IndexOf("&");
                parameter1 = parameter.Substring(startIndex + 1, endIndex - startIndex - 1);
                pack = Convert.ToInt32(parameter1);
                startIndex = parameter.LastIndexOf("=");
                endIndex = parameter.Length;
                parameter2 = parameter.Substring(startIndex + 1, endIndex - startIndex - 1);
                card = Convert.ToInt32(parameter2);

                vm.AddItemsToCard(pack, card);
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
            }
        }


        private void NextCard_Click(object sender, RoutedEventArgs e)
        {
            int size = vm.GetPackSize(pack);

            if (card < size - 1)
            {
                card += 1;
                vm.AddItemsToCard(pack, card);
            }
        }

        private void PreviousCard_Click(object sender, RoutedEventArgs e)
        {
            if (card > 0)
            {
                card += -1;
                vm.AddItemsToCard(pack, card);
            }
        }

        private void To_home_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void Remove_button_Click(object sender, RoutedEventArgs e)
        {
            vm.RemoveCard(pack, card);
            //Navigate back to browsed cards
            Frame.Navigate(typeof(BrowseCards));
        }
    }
}