using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CollectionManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectionManager.Models;

namespace CollectionManager.ViewModels
{
    [QueryProperty(nameof(SelectedCollection), "SelectedCollection")]

    public partial class CollectionPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private CollectionItemViewModel _selectedCollection;
        public IAsyncRelayCommand NewItemCommand { get; set; }
        public IAsyncRelayCommand CollectionSummaryCommand { get; set; }
        public IAsyncRelayCommand<Item> EditItemCommand { get; set; }
        public IAsyncRelayCommand<Item> EditImageCommand { get; set; }
        public IAsyncRelayCommand<Item> DeleteItemCommand { get; set; }

        public CollectionPageViewModel()
        {
            NewItemCommand = new AsyncRelayCommand(NewItemAsync);
            EditItemCommand = new AsyncRelayCommand<Item>(EditItemAsync);
            CollectionSummaryCommand = new AsyncRelayCommand(CollectionSummaryAsync);
            EditImageCommand = new AsyncRelayCommand<Item>(EditImageAsync);
            DeleteItemCommand = new AsyncRelayCommand<Item>(DeleteItemAsync);
        }

        private async Task DeleteItemAsync(Item selectedItem)
        {
            if (selectedItem == null) return;

            bool confirm = await Shell.Current.DisplayAlert("Usuwanie",
                $"Czy na pewno chcesz usunąć przedmiot '{selectedItem.Name}' z tej kolekcji?",
                "Tak",
                "Nie");

            if (confirm)
            {
                SelectedCollection.Items.Remove(selectedItem);
                DataManager.SaveData();
            }
        }

        private async Task EditImageAsync(Item selectedItem)
        {
            if (selectedItem == null) return;

            try
            {
                var photo = await MediaPicker.Default.PickPhotoAsync();
                if (photo != null)
                {
                    selectedItem.ImagePath = photo.FullPath;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", $"Nie udało się zmienić zdjęcia: {ex.Message}", "OK");
            }

            DataManager.SaveData();
        }

        public async Task CollectionSummaryAsync()
        {
            int soldItemsCount = SelectedCollection.Items.Count(item => item.Status == "Sprzedany");
            int forSaleItemsCount = SelectedCollection.Items.Count(item => item.Status == "Na sprzedaż");

            await Shell.Current.DisplayAlert("Podsumowanie kolekcji",
                $"Nazwa kolekcji: {SelectedCollection.Name}\nLiczba przedmiotów: {SelectedCollection.Items.Count}\nIlość sprzedanych przedmiotów: {soldItemsCount}\nIlość przedmiotów na sprzedaż: {forSaleItemsCount}",
                "OK");
        }

        public async Task NewItemAsync()
        {
            var input = await Shell.Current.DisplayPromptAsync("Nowy przedmiot",
                "Podaj nazwe przedmiotu:",
                "OK",
                "Anuluj");
            var itemName = input?.Trim();

            if (string.IsNullOrWhiteSpace(itemName))
            {
                return;
            }

            foreach (var item in SelectedCollection.Items)
            {
                if (item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                {
                    bool confirm = await Shell.Current.DisplayAlert("Przedmiot o tej nazwie już istnieje", 
                        "Czy chcesz dodać drugi taki przedmiot?", 
                        "OK", 
                        "Anuluj");

                    if(!confirm)
                    {
                        return;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            var newItem = new Item(itemName);
            SelectedCollection.Items.Add(newItem);

            bool addImageConfirm = await Shell.Current.DisplayAlert("Zdjęcie przedmiotu",
               "Czy chcesz dodać zdjęcie do tego przedmiotu?",
               "Tak",
               "Nie");

            if (addImageConfirm)
            {
                try
                {
                    var photo = await MediaPicker.Default.PickPhotoAsync();
                    if (photo != null)
                    {
                        newItem.ImagePath = photo.FullPath;
                    }
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Błąd", $"Nie udało się dodać zdjęcia: {ex.Message}", "OK");
                }
            }

            DataManager.SaveData();
        }

        private async Task EditItemAsync(Item selectedItem)
        {
            if (selectedItem == null)
            {
                return;
            }

            var navigationParameter = new Dictionary<string, object>
            {
                { "SelectedItem", selectedItem }
            };

            await Shell.Current.GoToAsync("ItemEditPage", navigationParameter);
        }

        partial void OnSelectedCollectionChanged(CollectionItemViewModel value)
        {
            if (value == null) return;

            foreach (var item in value.Items)
            {
                item.PropertyChanged -= OnItemPropertyChanged;
                item.PropertyChanged += OnItemPropertyChanged;
            }

            value.Items.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (Item newItem in e.NewItems)
                    {
                        newItem.PropertyChanged += OnItemPropertyChanged;
                    }
                }
            };
        }

        private void OnItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            DataManager.SaveData();

            if (e.PropertyName == nameof(Item.Status) && sender is Item item)
            {
                if (item.Status == "Sprzedany")
                {
                    var items = SelectedCollection.Items;

                    if (items.Contains(item) && items.IndexOf(item) != items.Count - 1)
                    {
                        items.Remove(item);
                        items.Add(item);
                    }
                }
            }
        }
    }
}
