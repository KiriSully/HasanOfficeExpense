using HasanOfficeExpense;
using System;
using System.Collections.Generic;
using System.Linq;

internal static class AnalyticsManagerHelpers
{
    public static void GenerateAnalytics(List<ClassExpense.Expense> expenses)
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

        Console.WriteLine("Натисніть будь-яку клавішу для повернення в меню адміністратора.");
        Console.ReadKey();
        UserMainMenu.AdminFunctionality();
    }
}