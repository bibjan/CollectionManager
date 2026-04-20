using CollectionManager.Models;
using CollectionManager.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        public IAsyncRelayCommand NewCollectionCommand { get; set; }

        public ObservableCollection<CollectionItemViewModel> Collections { get; } = new();
        public IAsyncRelayCommand<CollectionItemViewModel> GoToCollectionCommand { get; set; }


        public MainPageViewModel()
        {
            NewCollectionCommand = new AsyncRelayCommand(NewCollectionAsync);
            GoToCollectionCommand = new AsyncRelayCommand<CollectionItemViewModel>(GoToCollectionAsync);
            DataManager.LoadData(Collections);
            DataManager.Initialize(Collections);
        }

        private async Task NewCollectionAsync()
        {
            var input = await Shell.Current.DisplayPromptAsync("Nowa kolekcja",
            "Podaj nazwe kolekcji:",
            "OK",
            "Anuluj");
            var collectionName = input?.Trim();

            if (string.IsNullOrWhiteSpace(collectionName))
            {
                return;
            }

            var newCollection = new Collection(collectionName);
            Collections.Add(new CollectionItemViewModel(newCollection));

            DataManager.SaveData();
        }

        private async Task GoToCollectionAsync(CollectionItemViewModel selectedCollection)
        {
            if (selectedCollection == null)
            {
                return;
            }

            var collectionPage = new Dictionary<string, object>
            {
                { "SelectedCollection", selectedCollection }
            };

            await Shell.Current.GoToAsync("//CollectionPage", collectionPage);
        }
    }
}
