using System;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace IHaveIdeas.Models
{
    public class CardCurrent : INotifyPropertyChanged
    {
        private string _Verb;
        private string _Noun;
        private string _Adjective;
        private int _Image;
        private string FolderPath = "DatabaseImages";
        private string ImageEnding=".png";


        public int Image
        {
            get
            {
                return this._Image;
            }
            set
            {
                if (_Image != value)
                {

                    _Image = value;
                    Source = "ms-appx:///" + this.FolderPath + "/" + _Image + ImageEnding;
                    NotifyPropertyChanged("Source");

                }
            }
        }
        private string source;

        public string Source
        {
            get { return source; }
            set { source = value; }
        }
      
        public string Verb
         {
            get
            {
                return this._Verb;
            }
            set
            {
                if (_Verb != value)
                {
                    _Verb = value;
                    NotifyPropertyChanged("Verb");
                }
            }
        }

        public string Noun
        {
            get
            {
                return this._Noun;
            }
            set
            {
                if (_Noun != value)
                {
                    _Noun = value;
                    NotifyPropertyChanged("Noun");
                }
            }
        }
        public string Adjective
        {
            get
            {
                return this._Adjective;
            }
            set
            {
                if (_Adjective != value)
                {
                    _Adjective = value;
                    NotifyPropertyChanged("Adjective");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the app that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
