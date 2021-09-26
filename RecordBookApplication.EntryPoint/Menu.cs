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
            bool validInput = false;
            int userinput = 99;

            while (userinput != 0)
            {
                do
                {
                    try
                    {
                        userinput = int.Parse(Console.ReadLine());
                        validInput = true;
                    }
                    catch
                    {
                        Console.WriteLine("Please enter a valid input.");
                        validInput = false;
                    }
                } while (!validInput);
                switch (userinput)
                {
                    case 1: AddStudent(); break;
                    case 2: AddRandomStudent(); break;
                    case 3: PrintStudents(); AwaitUserInput(); break;
                    case 4: InitiateSort(); AwaitUserInput(); break;
                    case 0: break;
                    default: Console.WriteLine("Not a valid option. Try again."); AwaitUserInput(); break;
                }
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
            Console.WriteLine("Random student added. Press any key to continue...");
            Console.ReadKey();
        }
        public void InitiateSort()
        {
            InsertionSort(studentData, "Name");
            ClearFile();
            WriteToFile();
        }
        public List<Student> InsertionSort(List<Student> _studentData, string _getInfo)
        {
            int i, j;
            string getInfo = _getInfo;
            switch (getInfo)
            {
                case "ID":
                    for (i = 1; i < _studentData.Count; i++)
                    {
                        for (j = i; j > 0; j--)
                        {
                            if (_studentData[j].GetID() < _studentData[j -1].GetID())
                            {
                                var tmp = _studentData[j - 1];
                                _studentData[j-1] = _studentData[j];
                                _studentData[j] = tmp;
                            }
                        }
                    }
                    break;                
                case "Name":
                    _studentData.Sort((x, y) => string.Compare(x.GetName(), y.GetName()));
                    break;                
                
                case "Subject":
                    _studentData.Sort((x, y) => string.Compare(x.GetSubject(), y.GetSubject()));
                    break;
                case "Grade":
                    for (i = 1; i < _studentData.Count; i++)
                    {
                        for (j = i; j > 0; j--)
                        {
                            if (_studentData[j].GetGrade() < _studentData[j - 1].GetGrade())
                            {
                                var tmp = _studentData[j - 1];
                                _studentData[j - 1] = _studentData[j];
                                _studentData[j] = tmp;
                            }
                        }
                    }
                    break;

            }
            return _studentData;
        }
        public void PrintStudents()
        {
            foreach (var item in studentData)
            {
                Console.WriteLine(item);
            }
        }
        public void AwaitUserInput()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
        private void ClearFile()
        {
            using (TextWriter tw = new StreamWriter(data, false))
            {
                tw.Write(string.Empty);
            }
        }
        private void WriteToFile()
        {
            using (StreamWriter sw = File.AppendText(data))
            {
                for (int i = 0; i < studentData.Count; i++)
                {
                    string userInput = $"{studentData[i].GetID()},{studentData[i].GetName()},{studentData[i].GetSubject()},{studentData[i].GetGrade()}";
                    sw.WriteLine(userInput);
                }
            }
        }

    }
}

