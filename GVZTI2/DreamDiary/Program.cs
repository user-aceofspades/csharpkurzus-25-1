using System.Text;

namespace DreamDiary
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DreamStorage storage = new DreamStorage("dreams.json");
            List<Dream> dreams = await storage.Load();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("-- ÁLOMNAPLÓ 1.0 --");
                Console.WriteLine("1. Új álom hozzáadása");
                Console.WriteLine("2. Álmok listázása");
                Console.WriteLine("3. Kulcsszavas keresés");
                Console.WriteLine("4. Statisztika");
                Console.WriteLine("9. Kilépés");

                Console.Write("Válassz menüpontot: ");
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await AddDream(dreams, storage);
                        break;
                    case "2":
                        ListDreams(dreams);
                        break;
                    case "3":
                        SearchDreams(dreams);
                        break;
                    case "4":
                        ShowStatistics(dreams);
                        break;
                    case "9":
                        await storage.Save(dreams);
                        return;
                    default:
                        Console.WriteLine("Érvénytelen! " + PleasePressEnter());
                        Console.ReadLine();
                        break;
                }
            }
        }

        private static async Task AddDream(List<Dream> dreams, DreamStorage storage)
        {
            Console.Clear();
            Console.Write("Álom címe: ");
            string? title = Console.ReadLine();

            string? mood;
            do {
                Console.Write("Álom hangulata (pozitív / negatív / semleges): ");
                mood = Console.ReadLine();
            }
            while (mood != "pozitív" && mood != "negatív" && mood != "semleges");

            StringBuilder descriptionStringBuilder = new StringBuilder();

            Console.WriteLine("Írd be a \"--VÉGE--\" sort, ha végeztél!");
            while (true)
            {
                string? line = Console.ReadLine();
                if (line == null || line == "--VÉGE--")
                {
                    break;
                }

                descriptionStringBuilder.AppendLine(line);
            }

            string description = descriptionStringBuilder.ToString();

            Dream newDream = new Dream(title ?? "Névtelen álom", description, DateTime.Now, mood);

            dreams.Add(newDream);
            bool saveSuccess = await storage.Save(dreams);

            if (saveSuccess)
            {
                Console.WriteLine("Álom sikeresen hozzáadva!");
            }
            else
            {
                Console.WriteLine("Nem sikerült az álom mentése!");
            }

            Console.WriteLine(PleasePressEnter());
            Console.ReadLine();
        }

        private static void ListDreams(List<Dream> dreams)
        {
            Console.Clear();
            if (dreams.Count == 0)
            {
                Console.WriteLine("Nincs rögzített álom.");
            }
            else
            {
                foreach (Dream dream in dreams)
                {
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine(
                        $"[{dream.date.Year}-{dream.date.Month}-{dream.date.Day} {dream.date.Hour}:{dream.date.Minute}]" +
                        $" - {dream.title} ({dream.mood} hangulatú)");
                    Console.WriteLine(dream.description);
                    Console.WriteLine();
                }
            }

            Console.WriteLine(PleasePressEnter());
            Console.ReadLine();
        }
        private static void SearchDreams(List<Dream> dreams)
        {
            Console.Clear();
            string? keyword;
            do
            {
                Console.Write("Kulcsszó a kereséshez: ");
                keyword = Console.ReadLine();
            }
            while (keyword == null);

            List<Dream> matchingDreams = dreams.Where(dream => dream.description.Contains(keyword)).ToList();
            Console.Clear();

            if (matchingDreams.Count == 0)
            {
                Console.WriteLine("Nincs találat.");
            }
            else
            {
                Console.WriteLine($"{matchingDreams.Count} álomban van benne a \"{keyword}\"");
                foreach (Dream dream in matchingDreams)
                {
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine(
                        $"[{dream.date.Year}-{dream.date.Month}-{dream.date.Day} {dream.date.Hour}:{dream.date.Minute}]" +
                        $" - {dream.title} ({dream.mood} hangulatú)");
                    Console.WriteLine(dream.description);
                    Console.WriteLine();
                }
            }

            Console.WriteLine(PleasePressEnter());
            Console.ReadLine();
        }

        private static void ShowStatistics(List<Dream> dreams)
        {
            Console.Clear();

            Console.WriteLine("Álmok száma hangulat szerint:");

            if (dreams.Count > 0)
            {
                var moods = dreams.GroupBy(dream => dream.mood);
                foreach (var mood in moods)
                {
                    Console.WriteLine($"{mood.Key}: {mood.Count()} db");
                }
            }
            else
            {
                Console.WriteLine("Nincs rögzített álom.");
            }

                Console.WriteLine(PleasePressEnter());
            Console.ReadLine();
        }

        private static string PleasePressEnter()
        {
            return "Nyomj egy entert...";
        }
    }
}
