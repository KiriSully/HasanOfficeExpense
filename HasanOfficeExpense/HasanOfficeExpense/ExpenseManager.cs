using ClassExpense;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace HasanOfficeExpense
{
    public class ExpenseManager
    {
        internal static void SaveExpensesToFile(List<Expense> expenses)
        {
            string filePath = Program.GetDailyExpensesFilePath();

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

        internal static void DisplayExpenses(List<ClassExpense.Expense> expenses)
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
        internal static void DisplayExpensesAdmin(List<ClassExpense.Expense> expenses)
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
    }
}