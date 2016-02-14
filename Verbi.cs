using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using IHazIdeaz.Resources;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Windows.Resources;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Media;
using IHazIdeaz.Data;




namespace IHazIdeaz.Verbi
{
    public partial class Verbi : PhoneApplicationPage, INotifyPropertyChanged
    {
        const int kuvienMaara = 41;

        public Verbi verbiSana;
        public Database database;

        public int j;
        public int Click;

      

        public Verbi()
        {
       

            // Data context and observable collection are children of the main page.
            this.DataContext = this;
        }


        // Define an observable collection property that controls can bind to.
        public ObservableCollection<Verbi> _Verbit = new ObservableCollection<Verbi>();
        public ObservableCollection<Verbi> Verbit
        {
            get
            {
                // return this.GetTable<Verbi>();
                return this._Verbit;



            }
            set
            {
                if (_Verbit != value)
                {
                    _Verbit = value;
                    NotifyPropertyChanged("Verbit");
                }
            }
        }

        private void NotifyPropertyChanged(string p)
        {
            throw new NotImplementedException();
        }
    
        [Table(Name = "Verbi")]

        public class Verbitaulu : INotifyPropertyChanged, INotifyPropertyChanging
        {
            // Define ID: private field, public property and database column.
            private int _verbiID;

            [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
            public int verbiID
            {
                get
                {
                    return _verbiID;

                }
                set
                {
                    if (_verbiID != value)
                    {
                        NotifyPropertyChanging("verbiID");
                        _verbiID = value;
                        NotifyPropertyChanged("verbiID");
                    }
                }
            }

            // Define item name: private field, public property and database column.
            private string _verbi_sana;

            [Column]
            public string verbi_sana
            {
                get
                {
                    return _verbi_sana;
                }
                set
                {
                    if (_verbi_sana != value)
                    {
                        NotifyPropertyChanging("verbi_sana");
                        _verbi_sana = value;
                        NotifyPropertyChanged("verbi_sana");
                    }
                }
            }


            // Version column aids update performance.
            [Column(IsVersion = true)]
            private Binary _version;

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            // Used to notify the page that a data context property changed
            private void NotifyPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            #endregion

            #region INotifyPropertyChanging Members

            public event PropertyChangingEventHandler PropertyChanging;

            // Used to notify the data context that a data context property is about to change
            private void NotifyPropertyChanging(string propertyName)
            {
                if (PropertyChanging != null)
                {
                    PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
                }
            }

            #endregion

        }

    }
}

