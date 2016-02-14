using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using IHaveIdeas.Models;
using Windows.UI.Popups;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Reflection;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.WinRT;
//using SQLite.Net.Platform.WinRT;

namespace IHaveIdeas
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }
        const int numberOfImages = 176;
        public static string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "SQLitedb.sqlite");
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            //   CopyDatabase();
            

            //using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), path))
            //{
            //    conn.CreateTable<Adjectives>();
            //    conn.CreateTable<Verbs>();
            //    conn.CreateTable<Nouns>();
            //    conn.CreateTable<SavedCards>();
            //    conn.CreateTable<GeneratedCards>();
            //    conn.CreateTable<Images>();
            //}
            CopyDatabase();
            //Initialize();
            //}
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }
        public async void CopyDatabase()
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///SQLitedb.sqlite"));
                await file.CopyAsync(ApplicationData.Current.LocalFolder);
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.Message);
                Debug.WriteLine(error.StackTrace);
            }
        }


        public async void Initialize()
        {
            try
            {
                using (SQLiteConnection db = new SQLiteConnection(new SQLitePlatformWinRT(), path))
                {
                    StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///DatabaseWords/Verbs.txt"));
                    using (StreamReader Read = new StreamReader(await file.OpenStreamForReadAsync()))
                    {
                        while (!Read.EndOfStream)
                        {
                            Verbs verb = new Verbs { VerbWord = (Convert.ToString(Read.ReadLine())) };
                            db.Insert(verb);
                        }
                    }
                    file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///DatabaseWords/Adjectives.txt"));
                    using (StreamReader reader = new StreamReader(await file.OpenStreamForReadAsync()))
                    {
                        while (!reader.EndOfStream)
                        {
                            Adjectives adjective = new Adjectives { AdjectiveWord = (Convert.ToString(reader.ReadLine())) };
                            db.Insert(adjective);
                        }
                    }
                    file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///DatabaseWords/Nouns.txt"));
                    using (StreamReader reader = new StreamReader(await file.OpenStreamForReadAsync()))
                    {
                        while (!reader.EndOfStream)
                        {
                            Nouns noun = new Nouns { NounWord = (Convert.ToString(reader.ReadLine())) };
                            db.Insert(noun);
                        }
                    }
                    int i;
                    Images image = new Images();
                    for (i = 0; i < numberOfImages; i++)
                    {
                        image = new Models.Images { ImageNumber = i };
                        db.Insert(image);
                    }
                }

            }
            catch (Exception error)
            {
                var dialog = new MessageDialog("Error on " + error.Message + " Stacktrace " + error.StackTrace);
                await dialog.ShowAsync();
            }
        }

        //private async Task<bool> CheckFileExists(string fileName)
        //{
        //    try
        //    {
        //        var store = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
        //        return true;
        //    }
        //    catch
        //    {
        //    }
        //    return false;
        //}


        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
