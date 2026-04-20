using System.Diagnostics;
using Microsoft.Maui.Storage;

namespace CollectionManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Debug.WriteLine($"Ścieżka do danych aplikacji: {FileSystem.AppDataDirectory}");

            MainPage = new AppShell();
        }
    }
}
