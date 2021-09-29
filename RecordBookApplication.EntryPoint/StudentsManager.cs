using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordBookApplication.EntryPoint
{
    public class StudentsManager
    {

        private static string studentsDatabase;

        public static void StudentsMenu(List<Student> studentData, List<Subjects> subjectData, string _studentsDatabase)//User UI for stundents
        {
            studentsDatabase = _studentsDatabase;
            string userinput = "";

            while (userinput != "0")
            {
                PrintStudentMenu(studentData);

                userinput = Console.ReadLine();

                switch (userinput)
                {
                    case "1": AddStudent(studentData, subjectData); break;
                    case "2": PrintStudents(studentData); Menu.AwaitUserInput(); break;
                    case "3": AddAdditionalGrade(studentData, subjectData); Menu.AwaitUserInput(); break;
                    case "4": DeleteGrade(studentData); Menu.AwaitUserInput(); break;
                    case "5": DeleteStudent(studentData); Menu.AwaitUserInput(); break;
                    case "0": break;
                    default: Console.WriteLine("Not a valid option. Try again."); Menu.AwaitUserInput(); break;
                }
            }
        }
        private static void PrintStudentMenu(List<Student> studentData)
        {
            Console.Clear();
            Console.WriteLine(" ---- STUDENTS MANAGER ---- \n");
            Console.WriteLine($"\n - Amount of Students: {studentData.Count} -\n");
            Console.WriteLine("1 - Add new student");
            Console.WriteLine("2 - Show all students");
            Console.WriteLine("3 - Add additional grade to student");
            Console.WriteLine("4 - Delete grade from student");
            Console.WriteLine("5 - Delete student profile");
            Console.WriteLine("\n0 - Return to main menu\n");

        }
        private static void PrintStudents(List<Student> studentData)//Prints the list containing students in console
        {
            Console.Clear();
            foreach (var item in studentData)
            {
                Console.WriteLine(item);
            }
        }


        //Methods for interacting with students
        private static void AddStudent(List<Student> studentData, List<Subjects> subjectData) //Function for adding a student
        {
            Random rng = new Random();
            bool validID = false;

            int ID = rng.Next(11111, 99999); //Randomizes userID. 

            //Makes sure that the ID ins't used by another user. 
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

            Console.WriteLine("Enter Name"); //Input for Username
            string name = Console.ReadLine();


            studentData.Add(new Student(ID, name, "notRandom", subjectData)); //Adds the data to the List
            int index = studentData.FindIndex(a => a.GetID() == ID);
            string userInput = $"{ID},{name},{string.Join(System.Environment.NewLine, studentData[index].GetGrades())}";

            using (StreamWriter sw = File.AppendText(studentsDatabase)) //Writes the data to database
            {
                sw.WriteLine(userInput);
            }
            Console.WriteLine("Student added. Press any key to continue...");
            Console.ReadKey();
        }
        public static void AddRandomStudent(List<Student> studentData, List<Subjects> subjectData, string _studentsDatabase) //Function for adding a random student.
        {
            studentsDatabase = _studentsDatabase;

            Random rng = new Random();
            string[] names = new string[] { "John Doe", "Ben Dover", "Mah Rajs", "Hanna Svensson" }; //Array containing names that can be randomized
            string[] subjects = new string[] { "Engelska", "Svenska", "Programmering", "Matematik" }; //Array that contains subjects that can be randomized
            bool validID = false;

            int ID = rng.Next(11111, 99999); //Randomizes UserID

            Console.Clear();
            if (subjectData.Count != 0)
            {
                if (studentData.Count != 0) //Checks so that UserID isn't used by another user
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

                string name = names[rng.Next(0, 4)]; //Randomizes name from array

                //Adds data to list
                studentData.Add(new Student(ID, name, "Random", subjectData));
                int index = studentData.FindIndex(a => a.GetID() == ID);
                string userInput = $"{ID},{name},{string.Join(System.Environment.NewLine, studentData[index].GetGrades())}";

                using (StreamWriter sw = File.AppendText(studentsDatabase)) //Adds data to datbase
                {
                    sw.WriteLine(userInput);
                }
                Console.Clear();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("No subjects exists. Please create a subject before adding a student.");
                Console.ReadKey();
            }
        }
        private static void DeleteStudent(List<Student> studentData) //Function for deleting a student
        {
            int ID = 0;
            bool validSelection = false;
            int index = -1;

            do
            {
                Console.WriteLine("Please choose which student you want to remove: ");
                for (int i = 0; i < studentData.Count; i++)
                {
                    Console.WriteLine($"{studentData[i].GetName()}: {studentData[i].GetID()}");
                }

                try
                {
                    ID = int.Parse(Console.ReadLine());
                    index = studentData.FindIndex(a => a.GetID() == ID);
                    validSelection = true;
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Please choose a valid ID.");
                    Menu.AwaitUserInput();
                }
                if (index == -1)
                {
                    validSelection = false;
                    Console.WriteLine("Please choose a valid ID.");
                    Menu.AwaitUserInput();
                }
            } while (!validSelection);

            studentData.RemoveAt(index);
            WriteToStudentsFile(studentData);
        }


        //Methods for interacting with grades
        private static void AddAdditionalGrade(List<Student> studentData, List<Subjects> subjectData) //Adds more grades to an existing student
        {
            int ID = 0;
            bool validSelection = false;
            int index = -1;

            do
            {
                Console.Clear();
                Console.WriteLine("Please choose which student you want to add another grade to:");
                for (int i = 0; i < studentData.Count; i++)
                {
                    Console.WriteLine($"{studentData[i].GetName()}: {studentData[i].GetID()}");
                }
                try
                {
                    ID = int.Parse(Console.ReadLine());
                    if (ID <= 0)
                    {
                        validSelection = false;
                        Console.Clear();
                        Console.WriteLine("Please choose a valid ID.");
                        Menu.AwaitUserInput();
                    }
                    else
                    {
                        for (int i = 0; i < studentData.Count; i++)
                        {
                            if (ID == studentData[i].GetID())
                            {
                                validSelection = true;
                                index = studentData.FindIndex(a => a.GetID() == ID);
                                break;
                            }
                            else
                            {
                                validSelection = false;
                            }
                        }
                    }
                    if (!validSelection)
                    {
                        Console.Clear();
                        Console.WriteLine("Please choose a valid ID.");
                        Menu.AwaitUserInput();
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Please choose a valid ID.");
                    Menu.AwaitUserInput();
                }
            } while (!validSelection);

            studentData[index].AddGrades(subjectData);
            WriteToStudentsFile(studentData);
        }
        private static void DeleteGrade(List<Student> studentData) //Deletes grade from specific student
        {
            int ID = 0;
            bool validSelection = false;
            int index = -1;

            Console.WriteLine("Please choose which student you want to remove a grade from: ");
            for (int i = 0; i < studentData.Count; i++)
            {
                Console.WriteLine($"{studentData[i].GetName()}: {studentData[i].GetID()}");
            }

            do
            {
                try
                {
                    ID = int.Parse(Console.ReadLine());
                    index = studentData.FindIndex(a => a.GetID() == ID);
                    validSelection = true;
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Please choose a valid ID.");
                    Menu.AwaitUserInput();

                }
                if (index == -1)
                {
                    validSelection = false;
                    Console.WriteLine("Please choose a valid ID.");
                    Menu.AwaitUserInput();
                }
            } while (!validSelection);

            studentData[index].RemoveGrades();
            WriteToStudentsFile(studentData);
        }


        //File I/O
        public static void ClearStudentsFile(string _studentsDatabase) //Clears student database
        {
            using (TextWriter tw = new StreamWriter(_studentsDatabase, false))
            {
                tw.Write(string.Empty);
            }
        }
        private static void WriteToStudentsFile(List<Student> studentData) //Writes to student Database
        {
            ClearStudentsFile(studentsDatabase);
            using (StreamWriter sw = File.AppendText(studentsDatabase))
            {
                string convertedGrades;

                for (int i = 0; i < studentData.Count; i++)
                {
                    convertedGrades = Menu.ConvertGradesToString(studentData[i].GetGrades());
                    string userInput = $"{studentData[i].GetID()},{studentData[i].GetName()},{convertedGrades}";
                    sw.WriteLine(userInput);
                }
            }
        }
    }
}
