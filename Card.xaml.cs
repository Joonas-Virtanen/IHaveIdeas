using IHaveIdeas.ViewModel;
using System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace IHaveIdeas.Card
{
    public sealed partial class Card : Page
    {
        public TranslateTransform _transformVerb;
        public TranslateTransform _transformAdjectives;
        public TranslateTransform _transformNoun;
        public TranslateTransform _transformImage;
        public CardViewModel vm;
        StatusBar statusBar = StatusBar.GetForCurrentView();

        public Card()
        {
            this.InitializeComponent();
            vm = new CardViewModel();
            this.DataContext = vm;
            vm.ClearGeneratedCards();
            NewCard_Click(this, null);
            _transformVerb = new TranslateTransform();
            _transformAdjectives = new TranslateTransform();
            _transformNoun = new TranslateTransform();
            _transformImage = new TranslateTransform();

            VerbTextBlock.RenderTransform = _transformVerb;
            AdjectivesTextBlock.RenderTransform = _transformAdjectives;
            NounTextBlock.RenderTransform = _transformNoun;
            Image.RenderTransform = _transformImage;

            // Hide the status bar
            HideStatusbar();
        }

        private async void HideStatusbar()
        {
           await statusBar.HideAsync();
        }

        /// <summary>
        /// Loads previous card.
        /// </summary>
        private void Previous_Button_Click(object sender, RoutedEventArgs e)
        {
            vm.Previous_Button_Click(sender, e);
        }

        /// <summary>
        /// Generated a new card or old card if user is browsing old cards
        /// </summary>
        private void NewCard_Click(object sender, RoutedEventArgs e)
        {
            vm.NewCard_Click(sender, e);
        }

        private void AdjektiiviTextBlock_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            _transformAdjectives.X += e.Delta.Translation.X;
        }

        private void SubsanttiiviTextBlock_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            _transformNoun.X += e.Delta.Translation.X;
        }

        private void RandomKuva_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            _transformImage.X += e.Delta.Translation.X;
        }

        public void RandomKuva_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (_transformImage.X > 80 || _transformImage.X < -80)
            {
                  vm.RandomKuva_ManipulationCompleted();
            }
            _transformImage.X = 0;
        }

        public void AdjektiiviTextBlock_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (_transformAdjectives.X > 80 || _transformAdjectives.X < -80)
            {
                vm.AdjektiiviTextBlock_ManipulationCompleted();
            }
            _transformAdjectives.X = 0;
        }

        public void SubsanttiiviTextBlock_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (_transformNoun.X > 80 || _transformNoun.X < -80)
            {
                 vm.SubsanttiiviTextBlock_ManipulationCompleted();
            }
            _transformNoun.X = 0;
        }
        private void Home_button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void SavePack_Click(object sender, RoutedEventArgs e)
        {
             vm.SavePack_Click(sender, e);
        }

        private void VerbTextBlock_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (_transformVerb.X > 80 || _transformVerb.X < -80)
            {
                vm.VerbiTextBlock_ManipulationCompleted();
            }
            _transformVerb.X = 0;
        }

        private void VerbTextBlock_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            _transformVerb.X += e.Delta.Translation.X;
        }
    }
}
