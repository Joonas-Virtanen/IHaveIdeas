using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace IHaveIdeas.Data
{
    class FetchDatabaseData
    {
        public static BitmapImage GetImage(string filename)
        {
            BitmapImage bitmapImage = new BitmapImage();
            Uri uri = new Uri("ms-appx:///DatabaseImages/"+ filename+".png");
            bitmapImage.UriSource = uri;
            return bitmapImage;
        }
    }
}
