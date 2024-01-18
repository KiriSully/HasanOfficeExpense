using HasanOfficeExpense;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
internal static class ExpenseSearchManager
{
    public static List<string> availableCategories = new List<string> { "Офісні матеріали", "Послуги", "Їжа та напої", "Транспорт" };
    private static string user = "Guest";
    public static void SearchByDate(List<ClassExpense.Expense> expenses, List<string> availableCategories)
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
        Program.ChooseSearchCriteria();
    }


    public static void SearchByDateAdmin(List<ClassExpense.Expense> expenses, List<string> availableCategories)
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
        Program.ChooseSearchCriteriaAdmin();
    }

    public static void SearchByCategoryAdmin(List<ClassExpense.Expense> expenses, List<string> availableCategories)
    {
        Console.Clear();
        UserAuthentication.WriteUserRole(user, "Admin");

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
        var searchResults = expenses.Where(e => ((string)e.Category).Equals(selectedCategory, StringComparison.OrdinalIgnoreCase)).ToList();
        DisplaySearchResults(searchResults);
        Console.WriteLine("Натисніть будь-яку клавішу для продовження.");
        Console.ReadKey();
        Program.ChooseSearchCriteriaAdmin();
    }
    public static void SearchByCategory(List<ClassExpense.Expense> expenses, List<string> availableCategories)
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
        var searchResults = expenses.Where(e => string.Equals((string)e.Category, selectedCategory, StringComparison.OrdinalIgnoreCase))
                                    .ToList();
        DisplaySearchResults(searchResults);

        Console.WriteLine("Натисніть будь-яку клавішу для продовження.");
        Console.ReadKey();
        Program.ChooseSearchCriteria();
    }
    public static void SearchByDescription(List<ClassExpense.Expense> expenses, List<string> availableCategories)
    {
        Console.Clear();
        UserAuthentication.WriteUserRole(user, "User");
        Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
        Console.WriteLine("│   Пошук витрат за описом витрати   │");
        Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");

        Console.Write("Введіть опис витрати: ");
        string searchDescription = Console.ReadLine();
        var searchResults = expenses
            .Where(e => e.Description is string description && description.IndexOf(searchDescription, StringComparison.OrdinalIgnoreCase) >= 0)
            .ToList();
        DisplaySearchResultsAdmin(searchResults);
        Console.WriteLine("Натисніть будь-яку клавішу для продовження.");
        Console.ReadKey();
        Program.ChooseSearchCriteria();
    }
    public static void SearchByDescriptionAdmin(List<ClassExpense.Expense> expenses, List<string> availableCategories)
    {
        Console.Clear();
        UserAuthentication.WriteUserRole(user, "Admin");
        Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
        Console.WriteLine("│   Пошук витрат за описом витрати   │");
        Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");

        Console.Write("Введіть опис витрати: ");
        string searchDescription = Console.ReadLine();
        var searchResults = expenses
            .Where(e => e.Description is string description && description.IndexOf(searchDescription, StringComparison.OrdinalIgnoreCase) >= 0)
            .ToList();
        DisplaySearchResultsAdmin(searchResults);
        Console.WriteLine("Натисніть будь-яку клавішу для продовження.");
        Console.ReadKey();
        Program.ChooseSearchCriteriaAdmin();
    }
    public static void DisplaySearchResults(List<ClassExpense.Expense> matchingExpenses)
    {
        Console.Clear();
        UserAuthentication.WriteUserRole(user, "User");
        Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
        Console.WriteLine("│        Результати пошуку        │");
        Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");

        if (matchingExpenses.Count > 0)
        {
            ExpenseManager.DisplayExpenses(matchingExpenses);
        }
        else
        {
            Console.WriteLine("Немає витрат, що відповідають введеним критеріям.");
        }

        Console.WriteLine("Натисніть будь-яку клавішу для повернення в меню користувача.");
        Console.ReadKey();
        UserMainMenu.UserFunctionality();
    }
    private static void DisplaySearchResultsAdmin(List<ClassExpense.Expense> matchingExpenses)
    {
        Console.Clear();
        UserAuthentication.WriteUserRole(user, "Admin");
        Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
        Console.WriteLine("│        Результати пошуку        │");
        Console.WriteLine("┕━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┙");

        if (matchingExpenses.Count > 0)
        {
            ExpenseManager.DisplayExpensesAdmin(matchingExpenses);
        }
        else
        {
            Console.WriteLine("Немає витрат, що відповідають введеним критеріям.");
        }

        Console.WriteLine("Натисніть будь-яку клавішу для повернення в меню адміністратора.");
        Console.ReadKey();
        UserMainMenu.AdminFunctionality();
    }
}
