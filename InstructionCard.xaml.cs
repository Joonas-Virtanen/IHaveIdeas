using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace IHaveIdeas
{
   
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InstructionCard : Page
    {
        string parameter1;
        public InstructionCard()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            parameter1 = e.Parameter as string;
            if (parameter1 == "About")
            {
                Ok_button.Content = "Return back to About page";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (parameter1 == "Card")
            {
                this.Frame.Navigate(typeof(Card.Card));
            }
            else
            {
                this.Frame.Navigate(typeof(About));
            }
        }
    }
}
