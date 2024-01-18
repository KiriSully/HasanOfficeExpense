using System;
using System.Collections.Generic;

namespace HasanOfficeExpense
{
    internal static class UserMainMenu
    {

        private static string user = "Guest";

        public static void AdminFunctionality()
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
            List<ClassExpense.Expense> expensesList = new List<ClassExpense.Expense>();
            switch (key)
            {
                case '1':
                    Program.AddExpense();
                    break;
                case '2':
                    AnalyticsManager.GenerateAnalytics(expensesList);
                    break;
                case '3':
                    Program.ChooseSearchCriteriaAdmin();
                    break;
                case '0':
                    Program.Main();
                    break;
                default:
                    AdminFunctionality();
                    break;
            }
        }
        public static void UserFunctionality()
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
            List<ClassExpense.Expense> expensesList = new List<ClassExpense.Expense>();

            switch (key)
            {
                case '1':
                    AnalyticsManager.GenerateUser(expensesList);
                    break;
                case '2':
                    Program.ChooseSearchCriteria();
                    break;
                case '0':
                    Program.Main();
                    break;
                default:
                    UserFunctionality();
                    break;
            }
        }
    }
}