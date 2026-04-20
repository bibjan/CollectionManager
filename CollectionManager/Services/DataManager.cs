using CollectionManager.ViewModels;
using System.Collections.ObjectModel;
using System.IO;
using CollectionManager.Models;

namespace CollectionManager.Services
{
    public static class DataManager
    {
        private static ObservableCollection<CollectionItemViewModel> _collections;
        private static string FilePath => Path.Combine(FileSystem.AppDataDirectory, "data.txt");

        public static void Initialize(ObservableCollection<CollectionItemViewModel> collections)
        {
            _collections = collections;
        }

        public static void LoadData(ObservableCollection<CollectionItemViewModel> collections)
        {
            if (!File.Exists(FilePath))
            {
                return;
            }

            collections.Clear();
            CollectionItemViewModel currentCol = null;

            string[] lines = File.ReadAllLines(FilePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (line == "[KOLEKCJA]" && i + 1 < lines.Length)
                {
                    i++;
                    string nazwaKolekcji = lines[i];
                    currentCol = new CollectionItemViewModel(new Collection(nazwaKolekcji));
                    collections.Add(currentCol);
                }
                else if (line == "[PRZEDMIOT]" && currentCol != null && i + 1 < lines.Length)
                {
                    i++;
                    string[] parts = lines[i].Split("||");

                    if (parts.Length >= 4)
                    {
                        var item = new Item(parts[0])
                        {
                            Rating = parts[1],
                            Price = parts[2],
                            Status = parts[3]
                        };

                        if (parts.Length >= 5)
                            item.ImagePath = parts[4];

                        currentCol.Items.Add(item);
                    }
                }
            }
        }

        public static void SaveData()
        {
            if (_collections == null)
            {
                return;
            }

            using StreamWriter writer = new StreamWriter(FilePath, false);

            foreach (var col in _collections)
            {
                writer.WriteLine("[KOLEKCJA]");
                writer.WriteLine(col.Name);

                foreach (var item in col.Items)
                {
                    writer.WriteLine("[PRZEDMIOT]");
                    writer.WriteLine($"{item.Name}||{item.Rating}||{item.Price}||{item.Status}||{item.ImagePath}");
                }
            }
        }
    }
}