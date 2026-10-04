using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibraryDatabase
{
    public class InvalidLibraryDataException : Exception
    {
        public InvalidLibraryDataException(string message) : base(message)
        {
        }
    }

   
    public interface IBookStatistics
    {
      
        double AverageBookMovement(List<Library> records);

        int CountDaysIssuedMoreThanReturned(List<Library> records);
    }

    public abstract class Library
    {
        protected const string FileName = "libraries.txt";

        public string Name { get; set; }
        public string Address { get; set; }

        public Library()
        {
            Name = "";
            Address = "";
        }

        public Library(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public virtual string GetIndividualResult()
        {
            return "";
        }

        public virtual string ToFileString()
        {
            return $"{Name}|{Address}";
        }

        public virtual void Display()
        {
            Console.WriteLine($"Бібліотека: {Name}");
            Console.WriteLine($"Адреса: {Address}");
        }

        public void AddRecord(List<Library> records)
        {
            try
            {
                Console.Write("Назва бібліотеки: ");
                string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                    throw new InvalidLibraryDataException("Назва не може бути порожньою.");

                Console.Write("Адреса: ");
                string address = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(address))
                    throw new InvalidLibraryDataException("Адреса не може бути порожньою.");

                Console.Write("Дата (дд.мм.рррр): ");
                string dateInput = Console.ReadLine();

                if (!DateTime.TryParse(dateInput, out DateTime date))
                    throw new InvalidLibraryDataException("Некоректна дата.");

                Console.Write("Кількість виданих книг: ");
                if (!int.TryParse(Console.ReadLine(), out int issued) || issued < 0)
                    throw new InvalidLibraryDataException("Некоректна кількість виданих книг.");

                Console.Write("Кількість повернутих книг: ");
                if (!int.TryParse(Console.ReadLine(), out int returned) || returned < 0)
                    throw new InvalidLibraryDataException("Некоректна кількість повернутих книг.");

                WorkDay newRecord = new WorkDay(
                    name,
                    address,
                    date,
                    issued,
                    returned
                );

                records.Add(newRecord);
                SaveToFile(records);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Запис успішно додано.");
                Console.ResetColor();
            }
            catch (InvalidLibraryDataException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Помилка: {ex.Message}");
                Console.ResetColor();
            }
        }

        public void EditRecord(List<Library> records)
        {
            try
            {
                if (records.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("База даних порожня.");
                    Console.ResetColor();
                    return;
                }

                DisplayRecords(records);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Введіть номер запису для редагування: ");
                Console.ResetColor();
                if (!int.TryParse(Console.ReadLine(), out int index))
                    throw new InvalidLibraryDataException("Потрібно ввести число.");

                index--;

                if (index < 0 || index >= records.Count)
                    throw new InvalidLibraryDataException("Такого запису не існує.");

                WorkDay record = records[index] as WorkDay;

                if (record == null)
                    throw new InvalidLibraryDataException("Некоректний тип запису.");

                Console.Write("Нова назва бібліотеки: ");
                string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                    throw new InvalidLibraryDataException("Назва не може бути порожньою.");

                Console.Write("Нова адреса: ");
                string address = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(address))
                    throw new InvalidLibraryDataException("Адреса не може бути порожньою.");

                Console.Write("Нова дата (дд.мм.рррр): ");
                string dateInput = Console.ReadLine();

                if (!DateTime.TryParse(dateInput, out DateTime date))
                    throw new InvalidLibraryDataException("Некоректна дата.");

                Console.Write("Нова кількість виданих книг: ");
                if (!int.TryParse(Console.ReadLine(), out int issued) || issued < 0)
                    throw new InvalidLibraryDataException("Некоректна кількість виданих книг.");

                Console.Write("Нова кількість повернутих книг: ");
                if (!int.TryParse(Console.ReadLine(), out int returned) || returned < 0)
                    throw new InvalidLibraryDataException("Некоректна кількість повернутих книг.");

                record.Name = name;
                record.Address = address;
                record.Date = date;
                record.IssuedBooks = issued;
                record.ReturnedBooks = returned;

                SaveToFile(records);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Запис успішно відредаговано.");
                Console.ResetColor();
            }
            catch (InvalidLibraryDataException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Помилка: {ex.Message}");
                Console.ResetColor();
            }
        }

        public void DeleteRecord(List<Library> records)
        {
            try
            {
                if (records.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("База даних порожня.");
                    Console.ResetColor();
                    return;
                }

                DisplayRecords(records);

                Console.Write("Введіть номер запису для видалення: ");

                if (!int.TryParse(Console.ReadLine(), out int index))
                    throw new InvalidLibraryDataException("Потрібно ввести число.");

                index--;

                if (index < 0 || index >= records.Count)
                    throw new InvalidLibraryDataException("Такого запису не існує.");

                records.RemoveAt(index);
                SaveToFile(records);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Запис успішно видалено.");
                Console.ResetColor();
            }
            catch (InvalidLibraryDataException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Помилка: {ex.Message}");
                Console.ResetColor();
            }
        }

        public void DisplayRecords(List<Library> records)
        {
            if (records.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("База даних порожня.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("========== БАЗА ДАНИХ ==========");

            for (int i = 0; i < records.Count; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"Запис №{i + 1}");
                records[i].Display();
            }

            Console.WriteLine();
            Console.WriteLine("================================");
        }

        protected void SaveToFile(List<Library> records)
        {
            using (StreamWriter writer = new StreamWriter(FileName, false))
            {
                foreach (Library record in records)
                {
                    writer.WriteLine(record.ToFileString());
                }
            }
        }

        public List<Library> LoadFromFile()
        {
            List<Library> records = new List<Library>();

            if (!File.Exists(FileName))
            {
                CreateInitialFile();
            }

            string[] lines = File.ReadAllLines(FileName);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split('|');

                if (parts.Length != 5)
                    continue;

                if (!DateTime.TryParse(parts[2], out DateTime date))
                    continue;

                if (!int.TryParse(parts[3], out int issued))
                    continue;

                if (!int.TryParse(parts[4], out int returned))
                    continue;

                records.Add(
                    new WorkDay(
                        parts[0],
                        parts[1],
                        date,
                        issued,
                        returned
                    )
                );
            }

            return records;
        }

        private void CreateInitialFile()
        {
            List<Library> initialRecords = new List<Library>
            {
                new WorkDay("Міська бібліотека", "вул. Головна, 1",
                    new DateTime(2026, 9, 1), 24, 15),

                new WorkDay("Міська бібліотека", "вул. Головна, 1",
                    new DateTime(2026, 9, 2), 31, 18),

                new WorkDay("Міська бібліотека", "вул. Головна, 1",
                    new DateTime(2026, 9, 3), 20, 21),

                new WorkDay("Міська бібліотека", "вул. Головна, 1",
                    new DateTime(2026, 9, 4), 16, 11),

                new WorkDay("Міська бібліотека", "вул. Головна, 1",
                    new DateTime(2026, 9, 5), 28, 13)
            };

            SaveToFile(initialRecords);
        }

        public static void ClearConsole()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
        }
    }

    // Похідний клас реалізує інтерфейс IBookStatistics
    public class WorkDay : Library, IBookStatistics
    {
        public DateTime Date { get; set; }
        public int IssuedBooks { get; set; }
        public int ReturnedBooks { get; set; }

        public WorkDay() : base()
        {
            Date = DateTime.Now;
            IssuedBooks = 0;
            ReturnedBooks = 0;
        }

        public WorkDay(
            string name,
            string address,
            DateTime date,
            int issuedBooks,
            int returnedBooks) : base(name, address)
        {
            Date = date;
            IssuedBooks = issuedBooks;
            ReturnedBooks = returnedBooks;
        }

        public override string ToFileString()
        {
            return $"{Name}|{Address}|{Date:dd.MM.yyyy}|{IssuedBooks}|{ReturnedBooks}";
        }

        public override void Display()
        {
            Console.WriteLine($"Бібліотека: {Name}");
            Console.WriteLine($"Адреса: {Address}");
            Console.WriteLine($"Дата: {Date:dd.MM.yyyy}");
            Console.WriteLine($"Видано книг: {IssuedBooks}");
            Console.WriteLine($"Повернуто книг: {ReturnedBooks}");
            Console.WriteLine($"Рух книг: {IssuedBooks + ReturnedBooks}");
        }

        public override string GetIndividualResult()
        {
            return $"Дата {Date:dd.MM.yyyy}: видано {IssuedBooks}, повернуто {ReturnedBooks}";
        }

        // ---- Реалізація методів інтерфейсу IBookStatistics ----
        public double AverageBookMovement(List<Library> records)
        {
            List<WorkDay> days = records
                .OfType<WorkDay>()
                .ToList();

            if (days.Count == 0)
                return 0;

            return days.Average(x => x.IssuedBooks + x.ReturnedBooks);
        }

        public int CountDaysIssuedMoreThanReturned(List<Library> records)
        {
            return records
                .OfType<WorkDay>()
                .Count(x => x.IssuedBooks > x.ReturnedBooks);
        }

        // ---- Власні методи класу ----
        public static void DisplayEvenIssuedOddReturnedDays(List<Library> records)
        {
            List<WorkDay> result = records
                .OfType<WorkDay>()
                .Where(x => x.IssuedBooks % 2 == 0 &&
                            x.ReturnedBooks % 2 != 0)
                .ToList();

            Console.WriteLine();
            Console.WriteLine("Дні, коли видана парна кількість книг,");
            Console.WriteLine("а повернута — непарна:");

            if (result.Count == 0)
            {
                Console.WriteLine("Таких днів немає.");
                return;
            }

            foreach (WorkDay day in result)
            {
                Console.WriteLine(
                    $"{day.Date:dd.MM.yyyy} | " +
                    $"видано: {day.IssuedBooks} | " +
                    $"повернуто: {day.ReturnedBooks}"
                );
            }
        }

        public void ShowStatistics(List<Library> records)
        {
            List<WorkDay> days = records
                .OfType<WorkDay>()
                .OrderBy(x => x.Date)
                .ToList();

            if (days.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Немає даних для обчислення.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("========== РЕЗУЛЬТАТИ ==========");
            Console.ResetColor();

            // Виклик методів через інтерфейс
            IBookStatistics stat = this;

            double average = stat.AverageBookMovement(records);

            Console.WriteLine(
                $"Середній рух книжок в день: {average:F2}"
            );

            int count = stat.CountDaysIssuedMoreThanReturned(records);

            Console.WriteLine(
                $"Кількість днів, коли видано більше, ніж повернуто: {count}"
            );

            DisplayEvenIssuedOddReturnedDays(records);

            Console.WriteLine();
            Console.WriteLine("=================================");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            WorkDay library = new WorkDay();
            List<Library> records = library.LoadFromFile();

            while (true)
            {
                Library.ClearConsole();

                Console.WriteLine("==========================================");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("       БАЗА ДАНИХ БІБЛІОТЕКИ");
                Console.ResetColor();
                Console.WriteLine("==========================================");
                Console.WriteLine();
                Console.WriteLine("1 - Додати запис");
                Console.WriteLine("2 - Редагувати запис");
                Console.WriteLine("3 - Видалити запис");
                Console.WriteLine("4 - Вивести всі записи");
                Console.WriteLine("5 - Результати");
                Console.WriteLine("6 - Пошук");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Enter - Вихід");
                Console.ResetColor();
                Console.WriteLine("==========================================");
                Console.Write("Виберіть дію: ");
                ConsoleKeyInfo key = Console.ReadKey(true);

                try
                {
                    if (key.Key == ConsoleKey.Enter)
                        return;

                    switch (key.KeyChar)
                    {
                        case '1':
                            Library.ClearConsole();
                            library.AddRecord(records);
                            Pause();
                            break;

                        case '2':
                            Library.ClearConsole();
                            library.EditRecord(records);
                            Pause();
                            break;

                        case '3':
                            Library.ClearConsole();
                            library.DeleteRecord(records);
                            Pause();
                            break;

                        case '4':
                            Library.ClearConsole();
                            library.DisplayRecords(records);
                            Pause();
                            break;

                        case '5':
                            Library.ClearConsole();
                            library.ShowStatistics(records);
                            Pause();
                            break;

                        case '6':
                            Library.ClearConsole();
                            Search(records);
                            Pause();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Помилка: {ex.Message}");
                    Console.ResetColor();
                    Pause();
                }
            }
        }

        static void Search(List<Library> records)
        {
            if (records.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("База даних порожня.");
                Console.ResetColor();
                return;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Введіть назву бібліотеки для пошуку: ");
            Console.ResetColor();
            string search = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(search))
                throw new InvalidLibraryDataException("Потрібно ввести назву бібліотеки для пошуку.");

            List<Library> result = records
                .Where(x =>
                    x.Name.IndexOf(
                        search,
                        StringComparison.OrdinalIgnoreCase
                    ) >= 0)
                .ToList();

            Console.WriteLine();

            if (result.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Записів не знайдено.");
                Console.ResetColor();
                return;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Результати пошуку:");
            Console.ResetColor();

            foreach (Library record in result)
            {
                record.Display();
                Console.WriteLine("-------------------------");
            }
        }

        static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Натисніть будь-яку клавішу...");
            Console.ReadKey(true);
        }
    }
}