using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyrsach
{
    static class Requests
    {
        public static void CreateObshaga(Server server)
        {
            Console.Clear();

            server.BuildObshaga();
        }

        public static void DestoryObshaga(Server server)
        {
            Console.Clear();

            server.DestroyObshaga();
        }
        public static void ShowBuildingsName(Server server)
        {
            Console.Clear();
            server.ShowNamesOfObshagas();
            Console.WriteLine("Нажміть будь-яку кнопку для продовження..");
            Console.ReadLine();
        }
        public static void Interact(Server server)
        {
            Console.Clear();
            server.ShowNamesOfObshagas();
            int index = -1;
            if (server.buildings.Count > 0)
            {
                do
                {
                    Console.WriteLine("Оберіть номер гуртожитку, з яким ви хочете взаємодіяти:");
                    index = int.Parse(Console.ReadLine());
                } while (index >= server.buildings.Count || index < 0);

                server.InteractWithObshaga(server.buildings[index]);
            }
            else
            {
                Console.WriteLine("Нема з чим взаємодіяти!");
            }
            
        }
        public static void ShowRequests()
        {
            Console.Clear();
            
            Console.WriteLine("\t\t--<ВІТАЮ В СИМУЛЯТОРІ ГУРТОЖИТКУ>--");
            Console.WriteLine("\t\tРозробив студент групи KI-13: Качур Олексій\n");

            Console.WriteLine($"1. Створити гуртожиток.");
            Console.WriteLine($"2. Зруйнувати гуртожиток(якщо він був до цього збудований).");
            Console.WriteLine($"3. Переглянути імена існуючих гуртожитків.");
            Console.WriteLine($"4. Взаємодіяти з гуртожитком.");
            Console.WriteLine($"ESC. Вихід(Зберегти дані).");
        }
        public static void SaveData(Server server)
        {
            Console.Clear();

            server.SaveData();
        }
    }

    class Client
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Default;
            Console.Title = "Симулятор гуртожитку";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Server server = new Server();
            StreamReader file = File.OpenText(@"savedData.json");
            if(file.Peek() != -1)
                server.LoadData();

            file.Close();
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            while (key.Key != ConsoleKey.Escape)
            {
                Requests.ShowRequests();

                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        Requests.CreateObshaga(server);
                        break;
                    case ConsoleKey.D2:
                        Requests.DestoryObshaga(server);
                        break;
                    case ConsoleKey.D3:
                        Requests.ShowBuildingsName(server);
                        break;
                    case ConsoleKey.D4:
                        Requests.Interact(server);
                        break;
                    case ConsoleKey.Escape:
                        Requests.SaveData(server);
                        return;
                    default:
                        break;
                }
            }

            Console.ReadKey();
        }
    }
}
