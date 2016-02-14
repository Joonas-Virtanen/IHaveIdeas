using IHaveIdeas.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IHaveIdeas
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static int numberOfPacks { get; set; }
        StatusBar statusBar = StatusBar.GetForCurrentView();
        public MainPage()
        {
            numberOfPacks = 5;
            this.InitializeComponent();
            HideStatusbar();
        }

        private async void HideStatusbar()
        {
            await statusBar.HideAsync();
        }
        private void Saved_Cards_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BrowseCards));
        }

        private void Pick_a_Card_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Card.Card));
        }
        private void About_button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(About));
        }
    }
}
