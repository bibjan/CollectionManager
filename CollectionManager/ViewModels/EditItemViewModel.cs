using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectionManager.Models;
using CollectionManager.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CollectionManager.ViewModels
{
    [QueryProperty(nameof(SelectedItem), "SelectedItem")]
    public partial class EditItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private Item _selectedItem;

        public IAsyncRelayCommand SaveCommand { get; }
        public IAsyncRelayCommand EditImageCommand { get; }
        public IAsyncRelayCommand CancelCommand { get; }

        public EditItemViewModel()
        {
            SaveCommand = new AsyncRelayCommand(SaveAsync);
            EditImageCommand = new AsyncRelayCommand(EditImageAsync);
            CancelCommand = new AsyncRelayCommand(CancelAsync);
        }

        private async Task EditImageAsync()
        {
            if (SelectedItem == null) return;
            try
            {
                var photo = await MediaPicker.Default.PickPhotoAsync();
                if (photo != null)
                {
                    SelectedItem.ImagePath = photo.FullPath;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", $"Nie udało się zmienić zdjęcia: {ex.Message}", "OK");
            }
        }

        private async Task SaveAsync()
        {
            DataManager.SaveData();
            await Shell.Current.GoToAsync("..");
        }

        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
