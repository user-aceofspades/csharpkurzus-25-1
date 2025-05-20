using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DreamDiary
{
    public class DreamStorage
    {
        public string filePath;
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions() { WriteIndented = true, };

        public DreamStorage(string filePath)
        {
            this.filePath = filePath;
        }

        public async Task<List<Dream>> Load()
        {
            if (!File.Exists(this.filePath))
            {
                return new List<Dream>();
            }
            try
            {
                using FileStream json = File.OpenRead(this.filePath);
                List<Dream>? dreams = await JsonSerializer.DeserializeAsync<List<Dream>>(json);

                return dreams ?? new List<Dream>();
            }
            catch (IOException)
            {
                Console.Error.WriteLine("I/O Exception!");
            }
            catch (JsonException)
            {
                Console.Error.WriteLine("Probléma a JSON fájl feldolgozása során!");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Hupsz! Valami problémába ütköztünk!");
            }
            return new List<Dream>();
        }

        public async Task<bool> Save(List<Dream> dreams)
        {
            try
            {
                using FileStream stream = File.Create(this.filePath);
                await JsonSerializer.SerializeAsync(stream, dreams, this._options);
                return true;
            }
            catch (DirectoryNotFoundException)
            {
                Console.Error.WriteLine("Nincs ilyen könyvtár!");
            }
            catch (IOException)
            {
                Console.Error.WriteLine("I/O Exception!");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Hupsz! Valami problémába ütköztünk!");
            }
            return false;
        }
    }
}
