using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;
using ClassExpense;

namespace HasanOfficeExpense
{
    internal class Program
    {
        private static readonly string userPath = @"Data\Users\";
        private static readonly string reportsFolderPath = @"Data\Reports\";
        private static string user = "Guest";
        public static List<string> availableCategories = new List<string> { "Офісні матеріали", "Послуги", "Їжа та напої", "Транспорт" };
        private static int quantityUsers = 0;
        private static RegistrationManager registrationManager = new RegistrationManager(userPath);
        public static string GetDailyExpensesFilePath()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            return Path.Combine(reportsFolderPath, $"expenses_{currentDate}.txt");
        }
        public static void Main()
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
                UserMainMenu.AdminFunctionality();
            }
            else if (role.Equals("User", StringComparison.OrdinalIgnoreCase))
            {
                UserMainMenu.UserFunctionality();
            }
            else
            {
                Mainmenu();
            }
        }
        public static void Registration(bool clear)
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
            public static string HashPassword(string password)
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
                string hashedPassword = RegistrationManager.HashPassword(password);

                using (StreamWriter sw = new StreamWriter(userPath + name + ".txt", false, Encoding.UTF8))
                {
                    sw.WriteLine($"{name};{hashedPassword};{(quantityUsers == 0 ? 1 : 0)}", false, Encoding.UTF8);
                    sw.Close();
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
        public static void Authorization(bool clear)
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
                        string hashedEnteredPassword = RegistrationManager.HashPassword(enteredPassword);

                        if (hashedEnteredPassword == storedHashedPassword)
                        {
                            user = login;
                            string role = UserAuthentication.ReadUserRole(user);

                            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                            {
                                UserMainMenu.AdminFunctionality();
                            }
                            else if (role.Equals("User", StringComparison.OrdinalIgnoreCase))
                            {
                                UserMainMenu.UserFunctionality();
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

        public static void Mainmenu()
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
                    UserMainMenu.AdminFunctionality();
                    break;
                case '2':
                    UserAuthentication.WriteUserRole(user, "User");
                    UserMainMenu.UserFunctionality();
                    break;
                case '0':
                    Environment.Exit(0);
                    break;
                default:
                    Mainmenu();
                    break;
            }
        }
        
        private static List<ClassExpense.Expense> expenses = new List<ClassExpense.Expense>();
        private static int GetNextExpenseId()
        {
            return expenses.Count > 0 ? expenses.Max(e => e.Id) + 1 : 1;
        }
        public static void AddExpense()
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

            ClassExpense.Expense newExpense = new ClassExpense.Expense
            {
                Id = GetNextExpenseId(),
                Date = DateTime.Now,
                Amount = (int)expenseAmount,
                Description = expenseDescription,
                Category = expenseCategory
            };

            expenses.Add(newExpense);
            Console.WriteLine($"\nВитрати {expenseAmount} грн додано успішно!");
            ExpenseManager.SaveExpensesToFile(expenses);
            Console.WriteLine("Натисніть будь-яку клавішу для повернення в меню адміністратора.");
            Console.ReadKey();
            UserMainMenu.AdminFunctionality();
        }

        public static void ChooseSearchCriteria()
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
                    ExpenseSearchManager.SearchByDate(expenses, availableCategories);
                    break;
                case '2':
                    ExpenseSearchManager.SearchByCategory(expenses, availableCategories);
                    break;
                case '3':
                    ExpenseSearchManager.SearchByDescription(expenses, availableCategories);
                    break;
                case '0':
                    UserMainMenu.UserFunctionality();
                    break;
                default:
                    ChooseSearchCriteria();
                    break;
            }
        }
        public static void ChooseSearchCriteriaAdmin()
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
                    ExpenseSearchManager.SearchByDateAdmin(expenses, availableCategories);
                    break;
                case '2':
                    ExpenseSearchManager.SearchByCategoryAdmin(expenses, availableCategories);
                    break;
                case '3':
                    ExpenseSearchManager.SearchByDescriptionAdmin(expenses, availableCategories);
                    break;
                case '0':
                    UserMainMenu.AdminFunctionality();
                    break;
                default:
                    ChooseSearchCriteria();
                    break;
            }
        }
        
    }
}


