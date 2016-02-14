using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace IHazIdeaz.Data
{
    public partial class Database :Page, INotifyPropertyChanged
    {
        const int kuvienMaara = 176;

        public Verb verb;
        public Adjective adjective;
        public Noun noun;
        public Image image;
        public SavedCardPack pack;

        public GeneratedCard generatedCard;

        public int j;
        public int Click;


        // Data context for the local database
        public CardDataContext wordImageDB;

        public Database()
        {

            wordImageDB = new CardDataContext(CardDataContext.DBConnectionString);
            

            // Data context and observable collection are children of the main page.
            this.DataContext = this;
        }


        // Define an observable collection property that controls can bind to.
        public ObservableCollection<Verb> _Verbs = new ObservableCollection<Verb>();
        public ObservableCollection<Verb> Verbs
        {
            get
            {
                // return this.GetTable<Verb>();
                return this._Verbs;
            }
            set
            {
                if (_Verbs != value)
                {
                    _Verbs = value;
                    NotifyPropertyChanged("Verbs");
                }
            }
        }




        private ObservableCollection<Adjective> _Adjectives = new ObservableCollection<Adjective>();
        public ObservableCollection<Adjective> Adjectives
        {
            get
            {
                return this._Adjectives;
            }
            set
            {
                if (_Adjectives != value)
                {
                    _Adjectives = value;
                    NotifyPropertyChanged("Adjectives");
                }
            }
        }

        private ObservableCollection<Noun> _Nouns = new ObservableCollection<Noun>();
        public ObservableCollection<Noun> Nouns
        {
            get
            {
                return this._Nouns;
            }
            set
            {
                if (_Nouns != value)
                {
                    _Nouns = value;
                    NotifyPropertyChanged("Nouns");
                }
            }
        }

        private ObservableCollection<Image> _Images = new ObservableCollection<Image>();
        public ObservableCollection<Image> Images
        {
            get
            {
                return this._Images;
            }
            set
            {
                if (_Images != value)
                {
                    _Images = value;
                    NotifyPropertyChanged("Images");
                }
            }
        }

        private ObservableCollection<SavedCardPack> _SavedCardsPack = new ObservableCollection<SavedCardPack>();
        public ObservableCollection<SavedCardPack> SavedCardsPack
        {
            get
            {
                return this._SavedCardsPack;
            }
            set
            {
                if (_SavedCardsPack != value)
                {
                    _SavedCardsPack = value;
                    NotifyPropertyChanged("SavedCardPack");
                }
            }
        }

        private ObservableCollection<GeneratedCard> _GeneratedCards = new ObservableCollection<GeneratedCard>();
        public ObservableCollection<GeneratedCard> GeneratedCards
        {
            get
            {
                return this._GeneratedCards;
            }
            set
            {
                if (_GeneratedCards != value)
                {
                    _GeneratedCards = value;
                    NotifyPropertyChanged("GeneratedCards");
                }
            }
        }



        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Call the base method.
            base.OnNavigatedFrom(e);

            // Save changes to the database.
            wordImageDB.SubmitChanges();

            
        }
        

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var VerbsInDB = from Verb words in wordImageDB.Verbs
                            select words;

            var AdjectivesInDB = from Adjective words in wordImageDB.Adjectives
                                 select words;

            var NounsInDB = from Noun words in wordImageDB.Nouns
                            select words;

            var ImagesInDB = from Image images in wordImageDB.Images
                             select images;


            var SavedCardsInDB = from SavedCardPack cards in wordImageDB.SavedCardPacks
                                 select cards;


            var GeneratedCardsInDB = from GeneratedCard cards in wordImageDB.GeneratedCards
                                     select cards;


            // Execute the query and place the results into a collection.
            Verbs = new ObservableCollection<Verb>(VerbsInDB);
            Adjectives = new ObservableCollection<Adjective>(AdjectivesInDB);
            Nouns = new ObservableCollection<Noun>(NounsInDB);
            Images = new ObservableCollection<Image>(ImagesInDB);
            GeneratedCards = new ObservableCollection<GeneratedCard>(GeneratedCardsInDB);
            SavedCardsPack = new ObservableCollection<SavedCardPack>(SavedCardsInDB);


            // Call the base method.
            base.OnNavigatedTo(e);
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

        [Table(Name = "Verb")]

        public class Verb
        {

            [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
            public int verbID
            {
                get;
                set;
            }

            [Column]
            public string verb_word
            {
                get;
                set;
            }


            // Version column aids update performance.
            [Column(IsVersion = true)]
            private Binary _version;

        }

        [Table(Name = "Adjective")]

        public class Adjective
        {

            [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
            public int adjectiveID
            {
                get;
                set;
            }

            [Column]
            public string adjective_word
            {
                get;
                set;
            }

            // Version column aids update performance.
            [Column(IsVersion = true)]
            private Binary _version;
        }


        [Table(Name = "Noun")]

        public class Noun 
        {

            [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
            public int nounID
            {
                get;
                set;
            }

            [Column]
            public string noun_word
            {
                get;
                set;
            }


            // Version column aids update performance.
            [Column(IsVersion = true)]
            private Binary _version;

        }

        [Table(Name = "Image")]

        public class Image 
        {
            [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
            public int imageID
            {
                get;
                set;
            }

            [Column]
            public int image
            {
                get;
                set;
            }

            // Version column aids update performance.
            [Column(IsVersion = true)]
            private Binary _version;

        }

        [Table(Name = "SavedCardPack")]

        public class SavedCardPack
        {
          
            [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
            public int savedcardPackID
            {
                get;
                set;
            }

            [Column]
            public string verb_word
            {
                get;
                set;
            }

            [Column]
            public string adjective_word
            {
                get;
                set;
            }
           
            [Column]
            public string noun_word
            {
               get;
               set;
                
            }


            [Column]
            public int image
            {
                get;
                set;
            }

            [Column]
            public int packCard
            {
                get;
                set;
            }

            [Column]
            public int packNumber
            {
                get;
                set;
            }

            // Version column aids update performance.
            [Column(IsVersion = true)]
            private Binary _version;
     
        }


            [Table(Name = "GeneratedCard")]

            public class GeneratedCard
            {

                [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
                public int generatedcardID
                {
                    get;
                    set;
                }

                [Column]
                public string verb_word
                {
                    get;
                    set;
                }

                [Column]
                public string adjective_word
                {
                    get;
                    set;
                }

                [Column]
                public string noun_word
                {
                    get;
                    set;
                }

                [Column]
                public int image
                {
                    get;
                    set;
                }

                // Version column aids update performance.
                [Column(IsVersion = true)]
                private Binary _version;

          
            }
          
            public class CardDataContext 
            {


            }
            
    }


    }

    
