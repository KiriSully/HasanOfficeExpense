using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasanOfficeExpense
{
    internal class UserAuthentication
    {
        public static void WriteMenu(string user, string state, int quantity, string listChoise)
        {
            string[] list = listChoise.Split(',');

            Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
            Console.WriteLine($"   Ви увійшли як: {user}                            ");
            Console.WriteLine("┝━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┥");
            Console.WriteLine("│                                                  │");


        }
        public static void WriteUserRole(string userName, string role)
        {
            string filePath = Path.Combine(@"Data\Users", $"{userName}.role.txt");

            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                sw.WriteLine(role);
            }
            Console.WriteLine("┍━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┑");
            Console.WriteLine($"   Ви увійшли як: {userName}                            ");
            Console.WriteLine("┝━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┥");
            Console.WriteLine("│                                                  │");
        }
        public static string ReadUserRole(string userName)
        {
            string filePath = Path.Combine(@"Data\Users", $"{userName}.role.txt");

            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath, Encoding.UTF8).Trim();
            }

            return string.Empty;
        }
        public static bool CheckFileExist(string path)
        {
            return File.Exists(path);
        }
        public static bool CheckDirectoryeExist(string path)
        {
            return Directory.Exists(path);
        }
        public static bool CheckNotDirectoryFile(string path)
        {
            string[] list = Directory.GetFiles(path);
            return !(list.Length == 0);
        }
        public static bool CheckNotEmptyFile(string path)
        {
            var file = new FileInfo(path);
            return !(file.Length == 0);
        }
        public static string[] GettingAllUsers(string path)
        {
            string[] list = Directory.GetFiles(path);
            for (int i = 0; i < list.Length; i++)
            {
                int len = list[i].Split('\\', '.').Length;
                list[i] = list[i].Split('\\', '.')[len - 2];
            }
            return list;
        }
        public static string SecretPassword()
        {
            string pasword = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pasword += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && pasword.Length > 0)
                {
                    pasword = pasword.Substring(0, (pasword.Length - 1));
                    Console.Write("\b \b");
                }
            }
            while (key.Key != ConsoleKey.Enter);
            return pasword;
        }
    }
}
