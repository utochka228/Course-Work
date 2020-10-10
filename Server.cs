using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Kyrsach
{
    //HUMANOID
    abstract class Humanoid
    {
        public int yearsOld { get; set; }

        public int mass { get; set; }

        public string nationality { get; set; }

        public string sex { get; set; }

        public virtual void GetInfo(int id)
        {
            Console.WriteLine($"------------------#{id}------------------");
            Console.WriteLine("Інформація про громадянина: ");
            Console.WriteLine("Вага: {0} кг",mass);
            Console.WriteLine("Вік: {0} год ",yearsOld);
            Console.WriteLine("Національність: " + nationality);
            Console.WriteLine("Стать: " + sex);
        }
    }
    //STUDENT
    class Student : Humanoid
    {
        public int course { get; set; }
        public int salary { get; set; }

        public string institute { get; set; }
        public string studentTicket { get; set; }

        public int rating { get; set; }
        public int roomNumber { get; set; } 
        public override void GetInfo(int id)
        {
            base.GetInfo(id);
            Console.WriteLine("Курс студента: " + course);
            Console.WriteLine("Стипендия студента: {0} грн", salary);
            Console.WriteLine("Інститут студента: " + institute);
            Console.WriteLine("Білет студента: " + studentTicket);
            Console.WriteLine("Рейтинг студента: " + rating);
            Console.WriteLine("Кімната студента: " + roomNumber);
        }

        public Student(int roomNumber)
        {
            
            this.roomNumber = roomNumber;
            course = Server.rand.Next(1, 6);
            studentTicket = "BK" + Server.rand.Next(100000, 150000);

            rating = Server.rand.Next(1, 1000);

            int s = Server.rand.Next(0, 2);
            if (s == 1) base.sex = "Чоловічий";
            else base.sex = "Жіночий";

            base.mass = Server.rand.Next(40, 130);
            base.yearsOld = Server.rand.Next(16, 23);
            Nation nat = (Nation)Server.rand.Next(0, 5);
            base.nationality = nat.ToString();

            int y = Server.rand.Next(0, 2);
            if (y == 1) salary = Server.rand.Next(900, 2000);
            else salary = 0;
            Institute inst = (Institute)Server.rand.Next(0, 3);
            institute = inst.ToString();

        }
        
    }
    

    class Personal : Humanoid
    {
        string proffesion;
        int salary;
    }
    //KOMNATA
    class Room
    {
        public int RoomNumber{ get; set; }
        public int maxCapacity { get; set; }
        public int currentCapacity { get; set; }
        public Room(int roomNumber, int maxCapacity)
        {
            RoomNumber = roomNumber;
            this.maxCapacity = maxCapacity;
            currentCapacity = 0;
        }

        public List<Student> students { get; set; }
        public int SetStudents(int count)
        {
            int countWasSetted = 0;
            students = new List<Student>();
            if (count > maxCapacity) count = maxCapacity;

            for (int i = 0; i < count; i++)
            {
                students.Add(new Student(RoomNumber));
                currentCapacity++;
                countWasSetted++;
            }
            return countWasSetted;
        }
        public void GetInfo()
        {
            Console.WriteLine("Номер кімнати: " + RoomNumber);
            Console.WriteLine("Максимальна вмістимість: " + maxCapacity);
            Console.WriteLine("Теперішня вмістимість: " + currentCapacity);
        }
        public void GetStudentsInfo()
        {
            if (currentCapacity == 0)
            {
                Console.WriteLine("Кімната пуста!");
                return;
            }
            for (int i = 0; i < maxCapacity; i++)
            {
                if(students[i] != null) students[i].GetInfo(i);
                
            }
        }
    }

    //OBSHAGA
    class Obshaga
    {
        public string obshagaName { get; set; }
        public string komendaName { get; set; }
        public int floors { get; set; }
        public int roomPerFloor { get; set; }
        public int roomSquare { get; set; }
        public int roomCapacity { get; set; }
        public int maxPeople { get; set; }

        public Room[,] rooms { get; set; }

        public Obshaga(int roomCapacity, int roomPerFloor, int floors, string name, string komendaName)
        {
            this.obshagaName = name;

            this.komendaName = komendaName;

            this.roomCapacity = roomCapacity;

            this.roomSquare = roomCapacity * 6;

            this.floors = floors;

            this.roomPerFloor = roomPerFloor;

            maxPeople = roomCapacity * roomPerFloor * floors;

            rooms = new Room[floors, roomPerFloor];

            int roomNumber = 0;

            for (int i = 0; i < floors; i++)
            {
                for (int j = 0; j < roomPerFloor; j++)
                {
                    rooms[i, j] = new Room(roomNumber + 1, roomCapacity);
                    roomNumber++;
                }
            }

        }
        public void Settle()
        {
            int countOfStudent;

            do
            {
                Console.Clear();
                Console.WriteLine("Введіть кількість людей на поселення (от 1 до {0}): ", maxPeople);
                countOfStudent = Convert.ToInt32(Console.ReadLine());
            }
            while (countOfStudent > maxPeople || countOfStudent <= 0);

            Console.WriteLine(countOfStudent + " будуть заселені...");
            for (int i = 0; i < floors; i++)
            {
                for (int j = 0; j < roomPerFloor; j++)
                {
                    if (countOfStudent == 0) return;
                    int countWasSetted = rooms[i, j].SetStudents(countOfStudent);
                    countOfStudent -= countWasSetted;
                }
            }
        }
        public void GetStudentsInfo(int floor, int room)
        {
            rooms[floor, room].GetStudentsInfo();

            Console.WriteLine("Нажміть 1, щоб повернутися назад..");
            Console.WriteLine("Нажміть 0 для вихода..");
        }
        public void FuckStudent(out int Xindex, out int Yindex)
        {
            GetRoomInfo(out Xindex, out Yindex);
            GetStudentsInfo(Xindex, Yindex);
            int idStudent;
            List<Student> students = rooms[Xindex, Yindex].students;
            if (students.Count > 0)
            {
                do
                {
                    Console.WriteLine("Виберіть студента на виселення:");
                    idStudent = int.Parse(Console.ReadLine());
                } while (idStudent >= students.Count || idStudent < 0);
                students.Remove(students[idStudent]);
                rooms[Xindex, Yindex].maxCapacity--;
                rooms[Xindex, Yindex].currentCapacity--;
            }
            else
            {
                Console.WriteLine("В цій кімнаті пусто!");
            }
        }
        public void GetObshagaInfo()
        {
            Console.WriteLine("Ім'я гуртожитку: " + obshagaName);
            Console.WriteLine("Ім'я коменданта: " + komendaName);
            Console.WriteLine("Поверх: " + floors);
            Console.WriteLine("Кількість кімнат на поверх: " + roomPerFloor);
            Console.WriteLine("Площа кімнати: {0} м2", roomSquare);
            Console.WriteLine("Вмістимість кімнати: " + roomCapacity);

            Console.WriteLine("Нажміть 1, щоб переглянути інформацію про кімнату..");
            Console.WriteLine("Нажміть 2, щоб повернутися назад..");
        }
        public void GetRoomInfo(out int Xindex, out int Yindex)
        {
            int floor;
            do
            {
                Console.WriteLine($"Введіть бажаний поверх(1-{floors}): ");

                floor = Convert.ToInt32(Console.ReadLine());
            } while (floor < 1 || floor > floors);

            int room;
            do
            {
                Console.WriteLine($"Введіть бажану кімнату(1-{roomPerFloor}): ");
                room = Convert.ToInt32(Console.ReadLine());

            } while (room < 1 || room > roomPerFloor);

            rooms[floor-1, room-1].GetInfo();
            Xindex = floor - 1;
            Yindex = room - 1;

            Console.WriteLine("Нажміть 1, щоб переглянути інформацію про жильців..");
            Console.WriteLine("Нажміть 2, щоб повернутися назад..");
        }
    }

    class Server
    {
        public static Random rand = new Random();

        public List<Obshaga> buildings;

        public Server()
        {
            buildings = new List<Obshaga>();
        }

        public void BuildObshaga()
        {
            Console.WriteLine("Створи власний гуртожиток..");

            string name;
            do
            {
                Console.WriteLine("Введіть ім'я гуртожитку: ");
                name = Console.ReadLine();
            } while (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name));

            string komendaName;
            do
            {
                Console.WriteLine("Введіть ім'я коменданта: ");
                komendaName = Console.ReadLine();

            } while (string.IsNullOrEmpty(komendaName) || string.IsNullOrWhiteSpace(komendaName));

            int floors = -1;
            string floorsStr;
            do
            {
                Console.WriteLine("Введіть кількість поверхів (1 - N): ");
                floorsStr = Console.ReadLine();
                if (string.IsNullOrEmpty(floorsStr) || string.IsNullOrWhiteSpace(floorsStr))
                    continue;
                floors = Convert.ToInt32(floorsStr);

            } while (floors <= 0);

            int roomPerFloor = -1;
            string roomPerFloorStr;
            do
            {
                Console.WriteLine("Введіть кількість кімнат на поверх (10 - N): ");
                roomPerFloorStr = Console.ReadLine();
                if (string.IsNullOrEmpty(roomPerFloorStr) || string.IsNullOrWhiteSpace(roomPerFloorStr))
                    continue;
                roomPerFloor = Convert.ToInt32(roomPerFloorStr);

            } while (roomPerFloor < 10);

            int roomCapacity = -1;
            string roomCapacityStr;
            do
            {
                Console.WriteLine("Введіть максимальну вмістимість кімнати (1 - N): ");
                roomCapacityStr = Console.ReadLine();
                if (string.IsNullOrEmpty(roomCapacityStr) || string.IsNullOrWhiteSpace(roomCapacityStr))
                    continue;
                roomCapacity = Convert.ToInt32(roomCapacityStr);

            } while (roomCapacity < 1);

            Obshaga build = new Obshaga(roomCapacity, roomPerFloor, floors, name, komendaName);
            buildings.Add(build);

            Console.Clear();

            Console.WriteLine("Поселення жильців...");
            build.Settle();

            InteractWithObshaga(build);
        }

        public void InteractWithObshaga(Obshaga build)
        {
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            int floor;
            int room;
            while (key.Key != ConsoleKey.D0)
            {
                Console.WriteLine("Нажміть 1 для перегляду загальної інформації гуртожитку..");
                Console.WriteLine("Нажміть 2 для перегляду інформації кімнати..");
                Console.WriteLine("Нажміть 3, щоб виселити студента..");
                Console.WriteLine("Нажміть 0 для виходу..");

                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        build.GetObshagaInfo();

                        key = Console.ReadKey(true);
                        Console.Clear();
                        switch (key.Key)
                        {
                            case ConsoleKey.D1:
                                Console.Clear();
                                build.GetRoomInfo(out floor, out room);

                                key = Console.ReadKey(true);
                                Console.Clear();
                                switch (key.Key)
                                {
                                    case ConsoleKey.D1:
                                        Console.Clear();
                                        build.GetStudentsInfo(floor, room);

                                        key = Console.ReadKey(true);
                                        Console.Clear();
                                        break;
                                }
                                break;

                            case ConsoleKey.D2:
                                break;
                        }
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        build.GetRoomInfo(out floor, out room);

                        key = Console.ReadKey(true);
                        Console.Clear();
                        switch (key.Key)
                        {
                            case ConsoleKey.D1:
                                Console.Clear();
                                build.GetStudentsInfo(floor, room);

                                key = Console.ReadKey(true);
                                Console.Clear();
                                break;
                        }
                        break;
                    case ConsoleKey.D3:
                        Console.Clear();
                        build.FuckStudent(out floor, out room);
                        break;
                }

            }
        }

        public void DestroyObshaga()
        {
            ShowNamesOfObshagas();
            int index = -1;
            if(buildings.Count > 0)
            {
                do
                {
                    Console.WriteLine("Оберіть номер гуртожитку, який необхідно знищити:");
                    index = int.Parse(Console.ReadLine());
                } while (index >= buildings.Count || index < 0);

                buildings.Remove(buildings[index]);
            }
            else
            {
                Console.WriteLine("Немає що руйнувати!");
            }
        }

        public void ShowNamesOfObshagas()
        {
            Console.WriteLine("Імена в списку гуртожитків: ");
            int i = 0;
            foreach (var build in buildings)
            {
                Console.WriteLine(i + "." + build.obshagaName);
                i++;
            }
        }

        public void SaveData()
        {
            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter sw = new StreamWriter(@"savedData.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, buildings);
            }

        }
        public void LoadData()
        {
            buildings = new List<Obshaga>();
            using (StreamReader file = File.OpenText(@"savedData.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                buildings = (List<Obshaga>)serializer.Deserialize(file, typeof(List<Obshaga>));
            }
        }
    }

    public enum Nation:int { Українець, Болгарин, Білорус, Росіянин, Азіат, Американець };
    public enum Institute : int { IКНI, IКТА, IГДГ, IТРЕ };
}
