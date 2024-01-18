using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;

namespace HasanOfficeExpense
{
    internal class Program
    {
        private static readonly string userPath = @"Data\Users\";
        private static readonly string reportsFolderPath = @"Data\Reports\";
        private static string user = "Guest";
        private static List<string> availableCategories = new List<string> { "Офісні матеріали", "Послуги", "Їжа та напої", "Транспорт" };
        private static int quantityUsers = 0;
        private static RegistrationManager registrationManager = new RegistrationManager(userPath);

        private static string GetDailyExpensesFilePath()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            return Path.Combine(reportsFolderPath, $"expenses_{currentDate}.txt");
        }
        static void Main()
        {
            user = "Гість";
            string pathProject = Directory.GetCurrentDirectory();
            string pathData = Path.Combine(pathProject, "Data");

            if (!UserAuthentication.CheckDirectoryeExist(@"Data\"))
            {
                Directory.CreateDirectory(pathData);
                Directory.CreateDirectory(Path.Combine(pathData, "Reports"));
                Directory.CreateDirectory(Path.Combine(pathData, "Users"));
            }
            else
            {
                if (!UserAuthentication.CheckDirectoryeExist(userPath))
                {
                    Directory.CreateDirectory(Path.Combine(pathData, "Users"));
                }

                if (!UserAuthentication.CheckDirectoryeExist(Path.Combine(pathData, "Reports")))
                {
                    Directory.CreateDirectory(Path.Combine(pathData, "Reports"));
                }
            }
            Console.OutputEncoding = Encoding.UTF8;
            Console.Clear();
            UserAuthentication.WriteMenu(user, "Registration", 2, "Назад,Створити аккаунт");
            Console.WriteLine("│             ┍━━━━━━━━━━━━━━━━━━━━━━━┑            │");
            Console.WriteLine("│             │      Меню входу       │            │");
            Console.WriteLine("│             ┕━━━━━━━━━━━━━━━━━━━━━━━┙            │");
            Console.WriteLine("│  ┍━━━┯━━━━━━━━━━━━━━━━━━━┑                       │");
            Console.WriteLine("│  │ 1 │ Авторизація       │                       │");
            Console.WriteLine("│  ┝━━━┿━━━━━━━━━━━━━━━━━━━┥                       │");
            Console.WriteLine("│  │ 2 │ Реєстрація        │                       │");
            Console.WriteLine("│  ┝━━━┿━━━━━━━━━━━━━━━━━━━┥                       │");
            Console.WriteLine("│  │ 0 │ Вихід             │                       │");
            Console.WriteLine("│  ┕━━━┷━━━━━━━━━━━━━━━━━━━┙                       │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");
            char key = Console.ReadKey().KeyChar;
            switch (key)
            {
                case '0':
                    Environment.Exit(0);
                    break;
            }
            if (key.ToString() == "1")
            {
                Authorization(true);
            }
            if (key.ToString() == "2")
            {
                Registration(true);
            }
            else
            {
                Main();
            }

            string role = UserAuthentication.ReadUserRole(user);
            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                AdminFunctionality();
            }
            else if (role.Equals("User", StringComparison.OrdinalIgnoreCase))
            {
                UserFunctionality();
            }
            else
            {
                Mainmenu();
            }
        }
        static void Registration(bool clear)
        {
            if (clear)
            {
                Console.Clear();
            }
            UserAuthentication.WriteMenu(user, "Registration", 2, "Назад,Створити аккаунт");
            Console.WriteLine("│             ┍━━━━━━━━━━━━━━━━━━━━━━━┑            │");
            Console.WriteLine("│             │    Меню реєстрації    │            │");
            Console.WriteLine("│             ┕━━━━━━━━━━━━━━━━━━━━━━━┙            │");
            Console.WriteLine("│  ┍━━━┯━━━━━━━━━━━━━━━━━━━┑                       │");
            Console.WriteLine("│  │ 1 │ Назад             │                       │");
            Console.WriteLine("│  ┝━━━┿━━━━━━━━━━━━━━━━━━━┥                       │");
            Console.WriteLine("│  │ 2 │ Створити аккаунт  │                       │");
            Console.WriteLine("│  ┝━━━┿━━━━━━━━━━━━━━━━━━━┥                       │");
            Console.WriteLine("│  │ 0 │ Вихід             │                       │");
            Console.WriteLine("│  ┕━━━┷━━━━━━━━━━━━━━━━━━━┙                       │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");
            char key = Console.ReadKey(true).KeyChar;
            switch (key)
            {
                case '2':
                    registrationManager.RegisterUser();
                    break;
                case '0':
                    Environment.Exit(0);
                    break;
                case '1':
                    Main();
                    break;
            }
        }
        public class RegistrationManager
        {
            public string HashPassword(string password)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < hashedBytes.Length; i++)
                    {
                        builder.Append(hashedBytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }
            private string userPath;
            public RegistrationManager(string userPath)
            {
                this.userPath = userPath;
            }
            public void RegisterUser()
            {
                Console.Write("Створіть ім'я:");
                string name = Console.ReadLine();
                name = name.Replace(" ", "");
                name = name.Replace(";", "");

                if (name == "")
                {
                    Console.Clear();
                    Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
                    Console.WriteLine("│  Ви нічого не ввели.     │");
                    Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━┙");
                    Registration(false);
                }

                foreach (string item in UserAuthentication.GettingAllUsers(userPath))
                {
                    if (name == item)
                    {
                        Console.Clear();
                        Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
                        Console.WriteLine("│  Це ім'я уже зайнято.    │");
                        Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━┙");
                        Registration(false);
                    }
                }
                name = CleanFileName(name);
                Console.Write("Створіть пароль:");
                string password = UserAuthentication.SecretPassword();

                Console.Write("Підтвердіть пароль:");
                if (UserAuthentication.SecretPassword() == password && password != "")
                {
                    Console.WriteLine("Акаунт успішно створено! Натисніть будь-яку клавішу для продовження.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Паролі не співпадають. Натисніть будь-яку клавішу для спроби знову.");
                    Console.ReadKey();
                    Registration(false);
                }
                RegistrationManager registrationManager = new RegistrationManager(userPath);
                string hashedPassword = registrationManager.HashPassword(password);

                using (StreamWriter sw = new StreamWriter(userPath + name + ".txt", false, Encoding.UTF8))
                {
                    sw.WriteLine($"{name};{hashedPassword};{(quantityUsers == 0 ? 1 : 0)}", false, Encoding.UTF8);
                }

                user = name;
                quantityUsers++;
                Mainmenu();
            }
            
            private string CleanFileName(string fileName)
            {
                foreach (char invalidChar in Path.GetInvalidFileNameChars())
                {
                    fileName = fileName.Replace(invalidChar.ToString(), "");
                }
                return fileName;
            }
        }
        static void Authorization(bool clear)
        {
            Console.Clear();
            UserAuthentication.WriteMenu(user, "Authorization", 2, "Назад,Авторизуватися");
            Console.WriteLine("│             ┍━━━━━━━━━━━━━━━━━━━━━━━┑            │");
            Console.WriteLine("│             │    Меню авторизації   │            │");
            Console.WriteLine("│             ┕━━━━━━━━━━━━━━━━━━━━━━━┙            │");
            Console.WriteLine("│  ┍━━━┯━━━━━━━━━━━━━━━━━━━┑                       │");
            Console.WriteLine("│  │ 1 │ Назад             │                       │");
            Console.WriteLine("│  ┝━━━┿━━━━━━━━━━━━━━━━━━━┥                       │");
            Console.WriteLine("│  │ 2 │ Авотризуватися    │                       │");
            Console.WriteLine("│  ┝━━━┿━━━━━━━━━━━━━━━━━━━┥                       │");
            Console.WriteLine("│  │ 0 │ Вихід             │                       │");
            Console.WriteLine("│  ┕━━━┷━━━━━━━━━━━━━━━━━━━┙                       │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");
            char key = Console.ReadKey(true).KeyChar;
            switch (key)
            {
                case '0':
                    Environment.Exit(0);
                    break;
                case '1':
                    Main();
                    break;
                case '2':
                    bool flag = false;
                    Console.Write("Введіть логін:");
                    string login = Console.ReadLine();
                    if (key.ToString() == "2")
                    {
                        string name = Console.ReadLine();
                        name = name.Replace(" ", "");
                        name = name.Replace(";", "");
                        if (name == "")
                        {
                            Console.Clear();
                            Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
                            Console.WriteLine("│  Ви нічого не ввели.     │");
                            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━┙");
                            Authorization(false);
                        }
                    }
                    foreach (string item in UserAuthentication.GettingAllUsers(userPath))
                    {
                        if (login == item)
                        {
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
                        Console.WriteLine("│     Користувача з таким іменнем не знайдено.     │");
                        Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");
                    }
                    else
                    {
                        Console.Write("Введіть пароль:");
                        string pathUserAccount = Path.Combine(userPath, login) + ".txt";
                        string storedHashedPassword = File.ReadAllLines(pathUserAccount)[0].Split(';')[1];

                        RegistrationManager registrationManager = new RegistrationManager(userPath);
                        string enteredPassword = UserAuthentication.SecretPassword();
                        string hashedEnteredPassword = registrationManager.HashPassword(enteredPassword);

                        if (hashedEnteredPassword == storedHashedPassword)
                        {
                            user = login;
                            string role = UserAuthentication.ReadUserRole(user);

                            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                            {
                                AdminFunctionality();
                            }
                            else if (role.Equals("User", StringComparison.OrdinalIgnoreCase))
                            {
                                UserFunctionality();
                            }
                            else
                            {
                                Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
                                Console.WriteLine("│      Помилка: Роль користувача не визначена.     │");
                                Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");
                            }
                        }
                    }
                    break;
                default:
                    Authorization(true);
                    break;
            }
        }

        
        static void Mainmenu()
        {
            Console.Clear();
            UserAuthentication.WriteMenu(user, "Registration", 2, "Назад,Створити аккаунт");
            Console.WriteLine("│             ┍━━━━━━━━━━━━━━━━━━━━━━━┑            │");
            Console.WriteLine("│             │     Головне меню      │            │");
            Console.WriteLine("│             ┕━━━━━━━━━━━━━━━━━━━━━━━┙            │");
            Console.WriteLine("│ Будь ласка, оберіть свою роль для продовження:   │");
            Console.WriteLine("│  ┍━━━┯━━━━━━━━━━━━━━━━━━━┑                       │");
            Console.WriteLine("│  │ 1 │ Адміністратор     │                       │");
            Console.WriteLine("│  ┝━━━┿━━━━━━━━━━━━━━━━━━━┥                       │");
            Console.WriteLine("│  │ 2 │ Користувач        │                       │");
            Console.WriteLine("│  ┝━━━┿━━━━━━━━━━━━━━━━━━━┥                       │");
            Console.WriteLine("│  │ 0 │ Вихід             │                       │");
            Console.WriteLine("│  ┕━━━┷━━━━━━━━━━━━━━━━━━━┙                       │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");
            char key = Console.ReadKey().KeyChar;
            switch (key)
            {
                case '1':
                    UserAuthentication.WriteUserRole(user, "Admin");
                    AdminFunctionality();
                    break;
                case '2':
                    UserAuthentication.WriteUserRole(user, "User");
                    UserFunctionality();
                    break;
                case '0':
                    Environment.Exit(0);
                    break;
                default:
                    Mainmenu();
                    break;
            }
        }
        static void AdminFunctionality()
        {
            Console.Clear();
            UserAuthentication.WriteUserRole(user, "Admin");
            Console.WriteLine("│           ┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑      │");
            Console.WriteLine("│           │   Функціонал адміністратора   │      │");
            Console.WriteLine("│           ┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙      │");
            Console.WriteLine("│   ┍━━━┯━━━━━━━━━━━━━━━━━━━━━━━━━━━┑              │");
            Console.WriteLine("│   │ 1 │ Додати витрати            │              │");
            Console.WriteLine("│   ┝━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━┥              │");
            Console.WriteLine("│   │ 2 │ Генерація аналітики       │              │");
            Console.WriteLine("│   ┝━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━┥              │");
            Console.WriteLine("│   │ 3 │ Пошук виратрат            │              │");
            Console.WriteLine("│   ┝━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━┥              │");
            Console.WriteLine("│   │ 0 │ Повернутися в меню входу  │              │");
            Console.WriteLine("│   ┕━━━┷━━━━━━━━━━━━━━━━━━━━━━━━━━━┙              │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");
            char key = Console.ReadKey().KeyChar;
            List<Expense> expensesList = new List<Expense>();
            switch (key)
            {
                case '1':
                    AddExpense();
                    break;
                case '2':
                    GenerateAnalytics();
                    break;
                case '3':
                    ChooseSearchCriteriaAdmin();
                    break;
                case '0':
                    Main();
                    break;
                default:
                    AdminFunctionality();
                    break;
            }
        }
        static void UserFunctionality()
        {
            Console.Clear();
            UserAuthentication.WriteUserRole(user, "User");
            Console.WriteLine("│           ┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑      │");
            Console.WriteLine("│           │ Функціонал користувача        │      │");
            Console.WriteLine("│           ┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙      │");
            Console.WriteLine("│   ┍━━━┯━━━━━━━━━━━━━━━━━━━━━━━━━━━┑              │");
            Console.WriteLine("│   │ 1 │ Генерація аналітики       │              │");
            Console.WriteLine("│   ┝━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━┥              │");
            Console.WriteLine("│   │ 2 │ Пошук виратрат            │              │");
            Console.WriteLine("│   ┝━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━┥              │");
            Console.WriteLine("│   │ 0 │ Повернутися в меню входу  │              │");
            Console.WriteLine("│   ┕━━━┷━━━━━━━━━━━━━━━━━━━━━━━━━━━┙              │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");
            char key = Console.ReadKey().KeyChar;
            List<Expense> expensesList = new List<Expense>();

            switch (key)
            {
                case '1':
                    GenerateUser();
                    break;
                case '2':
                    ChooseSearchCriteria();
                    break;
                case '0':
                    Main();
                    break;
                default:
                    UserFunctionality();
                    break;
            }
        }
        public class Expense
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
        }
        private static List<Expense> expenses = new List<Expense>();
        private static int GetNextExpenseId()
        {
            return expenses.Count > 0 ? expenses.Max(e => e.Id) + 1 : 1;
        }
        static void GenerateUser()
        {
            Console.Clear();
            UserAuthentication.WriteUserRole(user, "User");
            Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
            Console.WriteLine("│    Генерація аналітики      │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");

            decimal totalExpenses = expenses.Sum(e => e.Amount);

            Console.WriteLine($"Загальна сума витрат: {totalExpenses} грн");

            var expensesByCategory = expenses.GroupBy(e => e.Category)
                                             .Select(group => new
                                             {
                                                 Category = group.Key,
                                                 TotalAmount = group.Sum(e => e.Amount),
                                                 Count = group.Count()
                                             });

            Console.WriteLine("\nДеталізація витрат за категоріями:");
            foreach (var categoryInfo in expensesByCategory)
            {
                Console.WriteLine($"Категорія: {categoryInfo.Category}");
                Console.WriteLine($"Загальна сума: {categoryInfo.TotalAmount} грн");
                Console.WriteLine($"Кількість витрат: {categoryInfo.Count}\n");
            }

            Console.WriteLine("Натисніть будь-яку клавішу для повернення в меню користувача.");
            Console.ReadKey();
            UserFunctionality();
        }
        static void GenerateAnalytics()
        {
            Console.Clear();
            UserAuthentication.WriteUserRole(user, "Admin");
            Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
            Console.WriteLine("│    Генерація аналітики      │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");

            decimal totalExpenses = expenses.Sum(e => e.Amount);

            Console.WriteLine($"Загальна сума витрат: {totalExpenses} грн");
            var expensesByCategory = expenses.GroupBy(e => e.Category)
                                             .Select(group => new
                                             {
                                                 Category = group.Key,
                                                 TotalAmount = group.Sum(e => e.Amount),
                                                 Count = group.Count()
                                             });


            Console.WriteLine("\nДеталізація витрат за категоріями:");
            foreach (var categoryInfo in expensesByCategory)
            {
                Console.WriteLine($"Категорія: {categoryInfo.Category}");
                Console.WriteLine($"Загальна сума: {categoryInfo.TotalAmount} грн");
                Console.WriteLine($"Кількість витрат: {categoryInfo.Count}\n");
            }

            Console.WriteLine("Натисніть будь-яку клавішу для повернення в меню адміністратора.");
            Console.ReadKey();
            AdminFunctionality();
        }
        private static void SaveExpensesToFile(List<Expense> expenses)
        {
            string filePath = GetDailyExpensesFilePath();

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                if (new FileInfo(filePath).Length == 0)
                {
                    writer.WriteLine("┍━━━━━━━━┯━━━━━━━━━━━━━━━━━━━━━┯━━━━━━━━━━━━━━┯━━━━━━━━━━━━━━━━━━━━┯━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
                    writer.WriteLine("│   ID   │        Дата         │     Сума     │      Категорія     │            Опис витрат            │");
                    writer.WriteLine("┝━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┥");
                }
                foreach (var expense in expenses)
                {
                    writer.WriteLine($"│ {expense.Id,-6} │  {expense.Date,-17:yyyy-MM-dd HH:mm}  │ {expense.Amount,-11}  │ {expense.Category,-17}  │ {expense.Description,-33} │ ");
                    writer.WriteLine("┝━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┥");
                }
            }
        }

        static void AddExpense()
        {
            Console.Clear();
            UserAuthentication.WriteUserRole(user, "Admin");
            Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
            Console.WriteLine("│     Додавання витрати     │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");

            Console.Write("Введіть суму витрати: ");
            decimal expenseAmount;
            while (!decimal.TryParse(Console.ReadLine(), out expenseAmount) || expenseAmount <= 0)
            {
                Console.WriteLine("Невірний формат суми. Будь ласка, введіть додатнє число.");
                Console.Write("Введіть суму витрати: ");
            }

            Console.Write("Введіть опис витрати: ");
            string expenseDescription = Console.ReadLine();
            Console.WriteLine("Доступні категорії:");

            for (int i = 0; i < availableCategories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableCategories[i]}");
            }

            Console.WriteLine($"{availableCategories.Count + 1}. Створити нову категорію");

            Console.Write("Виберіть номер категорії або введіть номер для створення нової: ");
            int categoryChoice;

            while (!int.TryParse(Console.ReadLine(), out categoryChoice) || categoryChoice < 1 || categoryChoice > availableCategories.Count + 1)
            {
                Console.WriteLine("Невірний вибір. Будь ласка, введіть коректний номер категорії.");
                Console.Write("Виберіть номер категорії або введіть номер для створення нової: ");
            }

            string expenseCategory;

            if (categoryChoice <= availableCategories.Count)
            {
                expenseCategory = availableCategories[categoryChoice - 1];
            }
            else
            {
                Console.Write("Введіть назву нової категорії: ");
                expenseCategory = Console.ReadLine();
                availableCategories.Add(expenseCategory);
            }

            Expense newExpense = new Expense
            {
                Id = GetNextExpenseId(),
                Date = DateTime.Now,
                Amount = expenseAmount,
                Description = expenseDescription,
                Category = expenseCategory
            };

            expenses.Add(newExpense);
            Console.WriteLine($"\nВитрати {expenseAmount} грн додано успішно!");
            SaveExpensesToFile(expenses);
            Console.WriteLine("Натисніть будь-яку клавішу для повернення в меню адміністратора.");
            Console.ReadKey();
            AdminFunctionality();
        }

        private static void ChooseSearchCriteria()
        {
            Console.Clear();
            UserAuthentication.WriteUserRole(user, "User");
            Console.WriteLine("│           ┍━━━━━━━━━━━━━━━━━━━━━━━━━━━┑          │");
            Console.WriteLine("│           │       Пошук витрат        │          │");
            Console.WriteLine("│           ┕━━━━━━━━━━━━━━━━━━━━━━━━━━━┙          │");
            Console.WriteLine("│                                                  │");
            Console.WriteLine("│    Виберіть тип пошуку витрат:                   │");
            Console.WriteLine("│  ┍━━━┯━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑          │");
            Console.WriteLine("│  │ 1 │ Пошук за датою                 │          │");
            Console.WriteLine("│  ┝━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┥          │");
            Console.WriteLine("│  │ 2 │ Пошук за категорією            │          │");
            Console.WriteLine("│  ┝━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┥          │");
            Console.WriteLine("│  │ 3 │ Пошук за описом                │          │");
            Console.WriteLine("│  ┝━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┥          │");
            Console.WriteLine("│  │ 0 │ Повернутися в меню користувача │          │");
            Console.WriteLine("│  ┕━━━┷━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙          │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");
            char key = Console.ReadKey().KeyChar;

            switch (key)
            {
                case '1':
                    SearchByDate();
                    break;
                case '2':
                    SearchByCategory();
                    break;
                case '3':
                    SearchByDescription();
                    break;
                case '0':
                    UserFunctionality();
                    break;
                default:
                    ChooseSearchCriteria();
                    break;
            }
        }

        private static void SearchByDate()
        {
            Console.Clear();
            UserAuthentication.WriteUserRole(user, "User");
            Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
            Console.WriteLine("│       Пошук витрат за датою     │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");

            Console.Write("Введіть дату (рррр-мм-дд): ");
            string inputDate = Console.ReadLine();

            if (DateTime.TryParseExact(inputDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime searchDate))
            {
                var searchResults = expenses.Where(e => e.Date.Date == searchDate.Date).ToList();

                DisplaySearchResults(searchResults);
            }
            else
            {
                Console.WriteLine("Невірний формат дати.");
            }

            Console.WriteLine("Натисніть будь-яку клавішу для продовження.");
            Console.ReadKey();
            ChooseSearchCriteria();
        }
        private static void SearchByCategory()
        {
            Console.Clear();
            UserAuthentication.WriteUserRole(user, "User");
            Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
            Console.WriteLine("│    Пошук витрат за категорією    │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");

            Console.WriteLine("Доступні категорії:");

            for (int i = 0; i < availableCategories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableCategories[i]}");
            }

            Console.Write("Виберіть номер категорії: ");
            int categoryChoice;

            while (!int.TryParse(Console.ReadLine(), out categoryChoice) || categoryChoice < 1 || categoryChoice > availableCategories.Count)
            {
                Console.WriteLine("Невірний вибір. Будь ласка, введіть коректний номер категорії.");
                Console.Write("Виберіть номер категорії: ");
            }

            string selectedCategory = availableCategories[categoryChoice - 1];
            var searchResults = expenses.Where(e => e.Category.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase)).ToList();
            DisplaySearchResults(searchResults);

            Console.WriteLine("Натисніть будь-яку клавішу для продовження.");
            Console.ReadKey();
            ChooseSearchCriteria();
        }

        private static void SearchByDescription()
        {
            Console.Clear();
            UserAuthentication.WriteUserRole(user, "User");
            Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
            Console.WriteLine("│   Пошук витрат за описом витрати   │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");

            Console.Write("Введіть опис витрати: ");
            string searchDescription = Console.ReadLine();

            var searchResults = expenses.Where(e => e.Description.IndexOf(searchDescription, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            DisplaySearchResults(searchResults);
            Console.WriteLine("Натисніть будь-яку клавішу для продовження.");
            Console.ReadKey();
            ChooseSearchCriteria();
        }

        private static void DisplaySearchResults(List<Expense> matchingExpenses)
        {
            Console.Clear();
            UserAuthentication.WriteUserRole(user, "User");
            Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
            Console.WriteLine("│        Результати пошуку        │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");

            if (matchingExpenses.Count > 0)
            {
                DisplayExpenses(matchingExpenses);
            }
            else
            {
                Console.WriteLine("Немає витрат, що відповідають введеним критеріям.");
            }

            Console.WriteLine("Натисніть будь-яку клавішу для повернення в меню користувача.");
            Console.ReadKey();
            UserFunctionality();
        }

        private static void DisplayExpenses(List<Expense> expenses)
        {
            Console.WriteLine("┍━━━━━━━━┯━━━━━━━━━━━━━━━━━━━━━┯━━━━━━━━━━━━━━┯━━━━━━━━━━━━━━━━━━━━┯━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
            Console.WriteLine("│   ID   │        Дата         │     Сума     │      Категорія     │            Опис витрат            │");
            Console.WriteLine("┝━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┥");

            foreach (var expense in expenses)
            {
                Console.WriteLine($"│ {expense.Id,-6} │  {expense.Date,-17:yyyy-MM-dd HH:mm}  │ {expense.Amount,-11}  │ {expense.Category,-17}  │ {expense.Description,-33} │ ");
                Console.WriteLine("┝━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┥");
            }
        }

        private static void ChooseSearchCriteriaAdmin()
        {
            Console.Clear();
            UserAuthentication.WriteUserRole(user, "Admin");
            Console.WriteLine("│           ┍━━━━━━━━━━━━━━━━━━━━━━━━━━━┑          │");
            Console.WriteLine("│           │       Пошук витрат        │          │");
            Console.WriteLine("│           ┕━━━━━━━━━━━━━━━━━━━━━━━━━━━┙          │");
            Console.WriteLine("│                                                  │");
            Console.WriteLine("│    Виберіть тип пошуку витрат:                   │");
            Console.WriteLine("│  ┍━━━┯━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑       │");
            Console.WriteLine("│  │ 1 │ Пошук за датою                    │       │");
            Console.WriteLine("│  ┝━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┥       │");
            Console.WriteLine("│  │ 2 │ Пошук за категорією               │       │");
            Console.WriteLine("│  ┝━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┥       │");
            Console.WriteLine("│  │ 3 │ Пошук за описом                   │       │");
            Console.WriteLine("│  ┝━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┥       │");
            Console.WriteLine("│  │ 0 │ Повернутися в меню адміністратора │       │");
            Console.WriteLine("│  ┕━━━┷━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙       │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");
            char key = Console.ReadKey().KeyChar;

            switch (key)
            {
                case '1':
                    SearchByDateAdmin();
                    break;
                case '2':
                    SearchByCategoryAdmin();
                    break;
                case '3':
                    SearchByDescriptionAdmin();
                    break;
                case '0':
                    AdminFunctionality();
                    break;
                default:
                    ChooseSearchCriteria();
                    break;
            }
        }
        private static void DisplaySearchResultsAdmin(List<Expense> matchingExpenses)
        {
            Console.Clear();
            UserAuthentication.WriteUserRole(user, "Admin");
            Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
            Console.WriteLine("│        Результати пошуку        │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");

            if (matchingExpenses.Count > 0)
            {
                DisplayExpensesAdmin(matchingExpenses);
            }
            else
            {
                Console.WriteLine("Немає витрат, що відповідають введеним критеріям.");
            }

            Console.WriteLine("Натисніть будь-яку клавішу для повернення в меню адміністратора.");
            Console.ReadKey();
            AdminFunctionality();
        }

        private static void DisplayExpensesAdmin(List<Expense> expenses)
        {
            Console.WriteLine("┍━━━━━━━━┯━━━━━━━━━━━━━━━━━━━━━┯━━━━━━━━━━━━━━┯━━━━━━━━━━━━━━━━━━━━┯━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
            Console.WriteLine("│   ID   │        Дата         │     Сума     │      Категорія     │            Опис витрат            │");
            Console.WriteLine("┝━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┥");

            foreach (var expense in expenses)
            {
                Console.WriteLine($"│ {expense.Id,-6} │  {expense.Date,-17:yyyy-MM-dd HH:mm}  │ {expense.Amount,-11}  │ {expense.Category,-17}  │ {expense.Description,-33} │ ");
                Console.WriteLine("┝━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━┿━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┥");
            }
        }
        private static void SearchByDateAdmin()
        {
            Console.Clear();
            UserAuthentication.WriteUserRole(user, "Admin");
            Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
            Console.WriteLine("│       Пошук витрат за датою     │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");

            Console.Write("Введіть дату (рррр-мм-дд): ");
            string inputDate = Console.ReadLine();

            if (DateTime.TryParseExact(inputDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime searchDate))
            {
                var searchResults = expenses.Where(e => e.Date.Date == searchDate.Date).ToList();

                DisplaySearchResultsAdmin(searchResults);
            }
            else
            {
                Console.WriteLine("Невірний формат дати.");
            }

            Console.WriteLine("Натисніть будь-яку клавішу для продовження.");
            Console.ReadKey();
            ChooseSearchCriteriaAdmin();
        }

        private static void SearchByCategoryAdmin()
        {
            Console.Clear();
            UserAuthentication.WriteUserRole(user, "Admin");
            Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
            Console.WriteLine("│    Пошук витрат за категорією    │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");

            Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
            Console.WriteLine("│    Пошук витрат за категорією    │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");

            Console.WriteLine("Доступні категорії:");

            for (int i = 0; i < availableCategories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableCategories[i]}");
            }

            Console.Write("Виберіть номер категорії: ");
            int categoryChoice;

            while (!int.TryParse(Console.ReadLine(), out categoryChoice) || categoryChoice < 1 || categoryChoice > availableCategories.Count)
            {
                Console.WriteLine("Невірний вибір. Будь ласка, введіть коректний номер категорії.");
                Console.Write("Виберіть номер категорії: ");
            }

            string selectedCategory = availableCategories[categoryChoice - 1];
            var searchResults = expenses.Where(e => e.Category.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase)).ToList();
            DisplaySearchResults(searchResults);
            Console.WriteLine("Натисніть будь-яку клавішу для продовження.");
            Console.ReadKey();
            ChooseSearchCriteriaAdmin();
        }
        private static void SearchByDescriptionAdmin()
        {
            Console.Clear();
            UserAuthentication.WriteUserRole(user, "Admin");
            Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
            Console.WriteLine("│   Пошук витрат за описом витрати   │");
            Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");

            Console.Write("Введіть опис витрати: ");
            string searchDescription = Console.ReadLine();

            var searchResults = expenses.Where(e => e.Description.IndexOf(searchDescription, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            DisplaySearchResultsAdmin(searchResults);
            Console.WriteLine("Натисніть будь-яку клавішу для продовження.");
            Console.ReadKey();
            ChooseSearchCriteriaAdmin();
        }
    }
}


