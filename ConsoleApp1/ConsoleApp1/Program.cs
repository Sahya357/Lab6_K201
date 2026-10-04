using System;
using System.IO;
using System.Collections.Generic;

namespace OSBB
{
 
    interface IOwnerService
    {
        void SaveToFile();
        void LoadFromFile();
        void CreateInitialData();
        void ShowOwners();
        int GetNewCode();
        void AddOwner();
        int FindOwnerByCode(int code);
        void EditOwner();
        void DeleteOwner();
        void SearchOwner();
        void SortOwners();
    }

    class Owner
    {
        private int code;
        private string surname;
        private int houseNumber;
        private int apartmentNumber;
        private string gender;
        private double debt;

        public Owner()
        {
            Code = 1;
            Surname = "Невідомо";
            HouseNumber = 1;
            ApartmentNumber = 1;
            Gender = "чоловіча";
            Debt = 0;
        }

        public Owner(int code, string surname, int houseNumber,
                     int apartmentNumber, string gender, double debt)
        {
            Code = code;
            Surname = surname;
            HouseNumber = houseNumber;
            ApartmentNumber = apartmentNumber;
            Gender = gender;
            Debt = debt;
        }

        public int Code
        {
            get { return code; }
            set
            {
                if (value <= 0)
                    throw new Exception("Код повинен бути більшим за 0");
                code = value;
            }
        }

        public string Surname
        {
            get { return surname; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Прізвище не може бути порожнім");
                surname = value;
            }
        }

        public int HouseNumber
        {
            get { return houseNumber; }
            set
            {
                if (value <= 0)
                    throw new Exception("Номер будинку повинен бути більшим за 0");
                houseNumber = value;
            }
        }

        public int ApartmentNumber
        {
            get { return apartmentNumber; }
            set
            {
                if (value <= 0)
                    throw new Exception("Номер квартири повинен бути більшим за 0");
                apartmentNumber = value;
            }
        }

        public string Gender
        {
            get { return gender; }
            set
            {
                if (value != "чоловіча" && value != "жіноча")
                    throw new Exception("Стать повинна бути 'чоловіча' або 'жіноча'");
                gender = value;
            }
        }

        public double Debt
        {
            get { return debt; }
            set
            {
                if (value < 0)
                    throw new Exception("Сума боргу не може бути від'ємною");
                debt = value;
            }
        }
    }

  
    class OwnerService : IOwnerService
    {
        private string fileName = "owners.txt";
        private List<Owner> owners = new List<Owner>();

        public OwnerService()
        {
            CreateInitialData();
            LoadFromFile();
        }

        public void SaveToFile()
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                for (int i = 0; i < owners.Count; i++)
                {
                    writer.WriteLine(
                        owners[i].Code + ";" +
                        owners[i].Surname + ";" +
                        owners[i].HouseNumber + ";" +
                        owners[i].ApartmentNumber + ";" +
                        owners[i].Gender + ";" +
                        owners[i].Debt);
                }
            }
        }

        public void LoadFromFile()
        {
            owners.Clear();

            if (!File.Exists(fileName))
                return;

            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] data = line.Split(';');
                    if (data.Length == 6)
                    {
                        owners.Add(new Owner(
                            Convert.ToInt32(data[0]),
                            data[1],
                            Convert.ToInt32(data[2]),
                            Convert.ToInt32(data[3]),
                            data[4],
                            Convert.ToDouble(data[5])));
                    }
                }
            }
        }

        public void CreateInitialData()
        {
            if (File.Exists(fileName))
                return;

            owners.Clear();
            owners.Add(new Owner(1001, "Іваненко", 1, 15, "чоловіча", 1200.50));
            owners.Add(new Owner(1002, "Петренко", 1, 24, "жіноча", 0));
            owners.Add(new Owner(1003, "Шевченко", 2, 7, "чоловіча", 3500));
            owners.Add(new Owner(1004, "Коваленко", 2, 18, "жіноча", 750.25));
            owners.Add(new Owner(1005, "Мельник", 3, 12, "чоловіча", 1500));
            owners.Add(new Owner(1006, "Бондаренко", 3, 31, "жіноча", 4200));
            owners.Add(new Owner(1007, "Ткаченко", 4, 5, "чоловіча", 850.75));
            owners.Add(new Owner(1008, "Кравченко", 4, 19, "жіноча", 2300));
            owners.Add(new Owner(1009, "Поліщук", 5, 8, "жіноча", 500));
            owners.Add(new Owner(1010, "Савченко", 5, 22, "чоловіча", 1750));
            SaveToFile();
        }

        private void PrintTable()
        {
            Console.WriteLine("{0,-8} {1,-18} {2,-10} {3,-12} {4,-12} {5,15}",
                "Код", "Прізвище", "Будинок", "Квартира", "Стать", "Сума боргу");
            Console.WriteLine(new string('-', 85));

            for (int i = 0; i < owners.Count; i++)
            {
                Console.WriteLine("{0,-8} {1,-18} {2,-10} {3,-12} {4,-12} {5,15:F2}",
                    owners[i].Code, owners[i].Surname, owners[i].HouseNumber,
                    owners[i].ApartmentNumber, owners[i].Gender, owners[i].Debt);
            }
        }

        private void PrintError(string message)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void PrintSuccess(string message)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void ShowOwners()
        {
            Console.Clear();
            Console.WriteLine("БАЗА ВЛАСНИКІВ КВАРТИР ОСББ");
            Console.WriteLine();

            if (owners.Count == 0)
            {
                Console.WriteLine("База даних порожня.");
                return;
            }

            PrintTable();
        }

        public int GetNewCode()
        {
            int maxCode = 1000;
            for (int i = 0; i < owners.Count; i++)
                if (owners[i].Code > maxCode)
                    maxCode = owners[i].Code;
            return maxCode + 1;
        }

        public void AddOwner()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("ДОДАВАННЯ ВЛАСНИКА");
                Console.WriteLine();

                int code = GetNewCode();
                Console.WriteLine("Код нового власника: " + code);
                Console.WriteLine();

                Console.Write("Введіть прізвище: ");
                string surname = Console.ReadLine();
                Console.Write("Введіть номер будинку: ");
                int houseNumber = Convert.ToInt32(Console.ReadLine());
                Console.Write("Введіть номер квартири: ");
                int apartmentNumber = Convert.ToInt32(Console.ReadLine());
                Console.Write("Введіть стать (чоловіча/жіноча): ");
                string gender = Console.ReadLine();
                Console.Write("Введіть суму боргу: ");
                double debt = Convert.ToDouble(Console.ReadLine());

                owners.Add(new Owner(code, surname, houseNumber,
                                     apartmentNumber, gender, debt));
                SaveToFile();
                PrintSuccess("Запис успішно додано.");
            }
            catch (FormatException)
            {
                PrintError("Помилка: введено дані неправильного формату.");
            }
            catch (Exception e)
            {
                PrintError("Помилка: " + e.Message);
            }
        }

        public int FindOwnerByCode(int code)
        {
            for (int i = 0; i < owners.Count; i++)
                if (owners[i].Code == code)
                    return i;
            return -1;
        }

        public void EditOwner()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("РЕДАГУВАННЯ ЗАПИСУ");
                Console.WriteLine();

                Console.Write("Введіть код власника: ");
                int code = Convert.ToInt32(Console.ReadLine());
                int index = FindOwnerByCode(code);

                if (index == -1)
                {
                    PrintError("Власника з таким кодом не знайдено.");
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("Знайдено власника: " + owners[index].Surname);
                Console.WriteLine("Код: " + owners[index].Code);
                Console.WriteLine();

                Console.Write("Нове прізвище: ");
                string surname = Console.ReadLine();
                Console.Write("Новий номер будинку: ");
                int houseNumber = Convert.ToInt32(Console.ReadLine());
                Console.Write("Новий номер квартири: ");
                int apartmentNumber = Convert.ToInt32(Console.ReadLine());
                Console.Write("Нова стать (чоловіча/жіноча): ");
                string gender = Console.ReadLine();
                Console.Write("Нова сума боргу: ");
                double debt = Convert.ToDouble(Console.ReadLine());

                owners[index].Surname = surname;
                owners[index].HouseNumber = houseNumber;
                owners[index].ApartmentNumber = apartmentNumber;
                owners[index].Gender = gender;
                owners[index].Debt = debt;

                SaveToFile();
                PrintSuccess("Запис успішно відредаговано.");
            }
            catch (FormatException)
            {
                PrintError("Помилка: введено дані неправильного формату.");
            }
            catch (Exception e)
            {
                PrintError("Помилка: " + e.Message);
            }
        }

        public void DeleteOwner()
        {
            Console.Clear();
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ВИДАЛЕННЯ ЗАПИСУ");
                Console.ResetColor();
                Console.WriteLine();

                Console.Write("Введіть код власника: ");
                int code = Convert.ToInt32(Console.ReadLine());
                int index = FindOwnerByCode(code);

                if (index == -1)
                {
                    PrintError("Власника з таким кодом не знайдено.");
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("Власник: " + owners[index].Surname);
                Console.WriteLine("Код: " + owners[index].Code);
                Console.WriteLine("Квартира: " + owners[index].ApartmentNumber);
                Console.WriteLine();

                Console.Write("Ви дійсно хочете видалити запис? (так/ні): ");
                string answer = Console.ReadLine();

                if (answer != null && answer.ToLower() == "так")
                {
                    owners.RemoveAt(index);
                    SaveToFile();
                    PrintSuccess("Запис видалено.");
                }
                else
                {
                    PrintSuccess("Видалення скасовано.");
                }
            }
            catch (FormatException)
            {
                PrintError("Помилка: код повинен бути числом.");
            }
            catch (Exception e)
            {
                PrintError("Помилка: " + e.Message);
            }
        }

        public void SearchOwner()
        {
            Console.Clear();
            Console.WriteLine("ПОШУК ЗА СУМОЮ БОРГУ");
            Console.WriteLine();

            try
            {
                Console.Write("Введіть суму боргу: ");
                double debt = Convert.ToDouble(Console.ReadLine());
                bool found = false;

                Console.WriteLine();

                for (int i = 0; i < owners.Count; i++)
                {
                    if (owners[i].Debt == debt)
                    {
                        Console.WriteLine("Код: " + owners[i].Code);
                        Console.WriteLine("Прізвище: " + owners[i].Surname);
                        Console.WriteLine("Номер будинку: " + owners[i].HouseNumber);
                        Console.WriteLine("Номер квартири: " + owners[i].ApartmentNumber);
                        Console.WriteLine("Стать: " + owners[i].Gender);
                        Console.WriteLine("Сума боргу: " + owners[i].Debt.ToString("F2"));
                        Console.WriteLine(new string('-', 40));
                        found = true;
                    }
                }

                if (!found)
                    Console.WriteLine("Власника з такою сумою боргу не знайдено.");
            }
            catch (FormatException)
            {
                PrintError("Помилка: введіть число.");
            }
        }

        public void SortOwners()
        {
            Console.Clear();
            Console.WriteLine("СОРТУВАННЯ ЗА ПРІЗВИЩЕМ");
            Console.WriteLine();

            for (int i = 0; i < owners.Count - 1; i++)
            {
                for (int j = 0; j < owners.Count - i - 1; j++)
                {
                    if (string.Compare(owners[j].Surname, owners[j + 1].Surname,
                        StringComparison.CurrentCulture) > 0)
                    {
                        Owner temp = owners[j];
                        owners[j] = owners[j + 1];
                        owners[j + 1] = temp;
                    }
                }
            }

            SaveToFile();
            PrintTable();
        }
    }

    class Program
    {
        static void Pause()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Натисніть будь-яку клавішу...");
            Console.ResetColor();
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            IOwnerService service = new OwnerService();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("======================================");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("       БАЗА ВЛАСНИКІВ ОСББ");
                Console.ResetColor();
                Console.WriteLine("======================================");
                Console.WriteLine();
                Console.WriteLine("1 - Вивести всі записи");
                Console.WriteLine("2 - Додати запис");
                Console.WriteLine("3 - Редагувати запис");
                Console.WriteLine("4 - Видалити запис");
                Console.WriteLine("5 - Пошук за сумою боргу");
                Console.WriteLine("6 - Сортування за прізвищем");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Enter - Вихід");
                Console.ResetColor();
                Console.WriteLine();
                Console.Write("Виберіть дію: ");

                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key == ConsoleKey.Enter)
                    break;

                switch (key.KeyChar)
                {
                    case '1': service.ShowOwners(); break;
                    case '2': service.AddOwner(); break;
                    case '3': service.EditOwner(); break;
                    case '4': service.DeleteOwner(); break;
                    case '5': service.SearchOwner(); break;
                    case '6': service.SortOwners(); break;
                }

                Pause();
            }

            Console.WriteLine();
            Console.WriteLine("Роботу програми завершено.");
        }
    }
}