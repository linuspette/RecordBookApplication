using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordBookApplication.EntryPoint
{
    public class Menu
    {
        string data = "database.txt";
        Statistics statistics = new Statistics();
        List<Student> studentData = new List<Student>();
        public void Run()
        {
            if (File.Exists(data))
            {
                using (StreamReader file = new StreamReader(data))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] dataText = line.Split(',');
                        studentData.Add(new Student(int.Parse(dataText[0]), dataText[1], dataText[2], double.Parse(dataText[3])));
                    }
                }
                    UI();
            }
            else
            {
                File.Create(data);
                UI();
            }


        }

        public void UI()
        {
            bool validSelection = false;
            string userinput = "";

            while (userinput != "0")
            {

            }
        }
        public void AddStudent()
        {
            Random rng = new Random();

            bool validSelection = false;
            bool validID = false;
            double grade = 0;

            int ID = rng.Next(11111, 99999);

            if (studentData.Count != 0)
            {
                do
                {
                    for (int i = 0; i < studentData.Count; i++)
                    {

                        if (ID == studentData[i].GetID())
                        {
                            ID = rng.Next(11111, 99999);
                        }
                        else
                        {
                            validID = true;
                        }
                    }
                } while (!validID);
            }

            Console.WriteLine("Enter Name");
            string name = Console.ReadLine();

            Console.WriteLine("Enter subject");
            string subject = Console.ReadLine();

            do
            {
                try
                {
                    grade = double.Parse(Console.ReadLine());
                    validSelection = true;
                }
                catch
                {
                    validSelection = false;
                }
            } while (!validSelection);


            studentData.Add(new Student(ID, name, subject, grade));
            string userInput = $"{ID},{name},{subject},{grade}";

            using (StreamWriter sw = File.AppendText(data))
            {
                sw.WriteLine(userInput);
            }
            Console.WriteLine("Student added. Press any key to continue...");
            Console.ReadKey();
        }

        public void AddRandomStudent()
        {
            Random rng = new Random();
            string[] names = new string[] { "John Doe", "Ben Dover", "Mah Rajs", "Hanna Svensson" };
            string[] subjects = new string[] { "Engelska", "Svenska", "Programmering", "Matematik" };
            bool validID = false;

            int ID = rng.Next(11111,99999);


            if (studentData.Count != 0)
            {
                do
                {
                    for (int i = 0; i < studentData.Count; i++)
                    {

                        if (ID == studentData[i].GetID())
                        {

                        }
                        else
                        {
                            validID = true;
                        }
                    }
                } while (!validID);
            }

            double grade = rng.Next(0, 100);
            string name = names[rng.Next(0, 3)];
            string subject = subjects[rng.Next(0, 3)];


            studentData.Add(new Student(ID, name, subject, grade));
            string userInput = $"{ID},{name},{subject},{grade}";

            using (StreamWriter sw = File.AppendText(data))
            {
                sw.WriteLine(userInput);
            }
        }

        public void PrintStudents()
        {
            foreach (var item in studentData)
            {
                Console.WriteLine(item);
            }
        }

    }
}

