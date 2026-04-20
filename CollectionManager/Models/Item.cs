using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.Models
{
    public partial class Item : ObservableObject
    {
        [ObservableProperty]
        private string _name;

        private int _ratingValue = 0;
        private int _priceValue = 0;
        private string _status;
        private string _imagePath;
        public double ItemOpacity => Status == "Sprzedany" ? 0.5 : 1.0;

        public static List<string> AvailableStatuses { get; } = new()
        {
            "Nowy",
            "Używany",
            "Na sprzedaż",
            "Sprzedany"
        };
        public string Rating
        {
            get => _ratingValue.ToString();
            set
            {
                var clearDigits = new string(value?.Where(char.IsDigit).ToArray());

                if (string.IsNullOrWhiteSpace(clearDigits))
                {
                    clearDigits = "0";
                }

                if (int.TryParse(clearDigits, out int parsedValue))
                {
                    _ratingValue = Math.Clamp(parsedValue, 0, 10);
                }

                OnPropertyChanged(nameof(Rating));
            }
        }

        public string Price
        {
            get => _priceValue.ToString();
            set
            {
                var clearDigits = new string(value?.Where(char.IsDigit).ToArray());

                if (string.IsNullOrWhiteSpace(clearDigits))
                {
                    clearDigits = "0";
                }

                if (int.TryParse(clearDigits, out int parsedValue))
                {
                    _priceValue = Math.Max(parsedValue, 0);
                }

                OnPropertyChanged(nameof(Price));
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(ItemOpacity));
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }

        public Item(string name)
        {
            _name = name;
            Rating = "0";
            Price = "0";
            Status = AvailableStatuses[0];
        }
    }
}
