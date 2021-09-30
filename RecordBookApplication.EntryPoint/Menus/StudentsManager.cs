using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RecordBookApplication.EntryPoint.Menu;
using static RecordBookApplication.EntryPoint.Classes;
using System.Threading;

namespace RecordBookApplication.EntryPoint
{
    public class StudentsManager
    {

        public static void StudentsMenu()//User UI for stundents
        { 
            string userinput = "";

            while (userinput != "0")
            {
                PrintStudentMenu();

                userinput = Console.ReadLine();

                switch (userinput)
                {
                    case "1": AddStudent(); break;
                    case "2": PrintStudents(); Menu.AwaitUserInput(); break;
                    case "3": AddAdditionalGrade(); Menu.AwaitUserInput(); break;
                    case "4": DeleteGrade(); Menu.AwaitUserInput(); break;
                    case "5": DeleteStudent(); Menu.AwaitUserInput(); break;
                    case "0": break;
                    default: Console.WriteLine("Not a valid option. Try again."); Menu.AwaitUserInput(); break;
                }
            }
        }
        private static void PrintStudentMenu()
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
        private static void PrintStudents()//Prints the list containing students in console
        {
            Console.Clear();
            foreach (var item in studentData)
            {
                Console.WriteLine(item);
            }
        }


        //Methods for interacting with students
        private static void AddStudent() //Function for adding a student
        {
            Random rng = new Random();
            bool validID = false;
            bool validSelection = false;
            string className = string.Empty;

            int ID = rng.Next(11111, 99999); //Randomizes userID. 

            //Makes sure that the ID ins't used by another user. 
            if (classData.Count != 0 && subjectData.Count != 0)
            {
                if (studentData.Count != 0)
                {
                    do
                    {
                        for (int i = 0; i < studentData.Count; i++)
                        {

                            if (ID == studentData[i].ID)
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

                do
                {
                    Console.WriteLine("Please select the class the student belongs to:");
                    for (int i = 0; i < classData.Count; i++)
                    {
                        Console.WriteLine(classData[i].className);
                    }
                    className = Console.ReadLine();
                    for (int i = 0; i < classData.Count; i++)
                    {
                        if (className == classData[i].className)
                        {
                            validSelection = true;
                            break;
                        }
                        else
                        {
                            validSelection = false;
                        }
                    }
                    if (!validSelection)
                    {
                        Console.WriteLine("Please choose a valid alternative.");
                    }
                } while (!validSelection);

                studentData.Add(new Student(ID, name, className, "notRandom")); //Adds the data to the List
                int index = studentData.FindIndex(a => a.ID == ID);
                string userInput = $"{ID},{name},{string.Join(System.Environment.NewLine, studentData[index].GetGrades())}";

                using (StreamWriter sw = File.AppendText(studentDatabase)) //Writes the data to database
                {
                    sw.WriteLine(userInput);
                }
                Console.WriteLine("Student added. Press any key to continue...");
                Console.ReadKey();
            }
            //If no subject or class exists
            else
            {
                if (classData.Count == 0)
                {
                    Console.WriteLine("You need to add a class before you add a student");
                    AwaitUserInput();
                }
                else if (subjectData.Count == 0)
                {
                    Console.WriteLine("You need to add a subject before you add a student");
                    AwaitUserInput();
                }
            }

        }
        public static void AddRandomStudent() //Function for adding a random student.
        {
            Random rng = new Random();
            string[] names = new string[] { "John Doe", "Ben Dover", "Mah Rajs", "Hanna Svensson" }; //Array containing names that can be randomized
            string[] subjects = new string[] { "Engelska", "Svenska", "Programmering", "Matematik" }; //Array that contains subjects that can be randomized
            bool validID = false;
            string className = string.Empty;

            int ID = rng.Next(11111, 99999); //Randomizes UserID

            Console.Clear();
            //If a class and a subject exists, adds random student
            if (classData.Count != 0 && subjectData.Count != 0)
            {
                if (subjectData.Count != 0)
                {
                    if (studentData.Count != 0) //Checks so that UserID isn't used by another user
                    {
                        do
                        {
                            for (int i = 0; i < studentData.Count; i++)
                            {

                                if (ID == studentData[i].ID)
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

                    int classDataIndex = rng.Next(0, classData.Count);


                    //Adds data to list
                    classData[rng.Next(0, classData.Count)].AddStudentData(ID, name, classData[classDataIndex].className, "Random");

                    int studentDataIndex = studentData.FindIndex(a => a.ID == ID);
                    string userInput = $"{ID},{name},{classData[classDataIndex].className},{string.Join(System.Environment.NewLine, studentData[studentDataIndex].GetGrades())}";

                    using (StreamWriter sw = File.AppendText(studentDatabase)) //Adds data to datbase
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
            //If no subject or class exists
            else
            {
                if (classData.Count == 0)
                {
                    Console.WriteLine("You need to add a class before you add a student");
                    AwaitUserInput();
                }
                else if (subjectData.Count == 0)
                {
                    Console.WriteLine("You need to add a subject before you add a student");
                    AwaitUserInput();
                }
            }
        }
        private static void DeleteStudent() //Function for deleting a student
        {
            int ID = 0;
            bool validSelection = false;
            int index = -1;

            do
            {
                Console.WriteLine("Please choose which student you want to remove: ");
                for (int i = 0; i < studentData.Count; i++)
                {
                    Console.WriteLine($"{studentData[i].name}: {studentData[i].ID}");
                }

                try
                {
                    ID = int.Parse(Console.ReadLine());
                    index = studentData.FindIndex(a => a.ID == ID);
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
            WriteToStudentsFile();
        }


        //Methods for interacting with grades
        private static void AddAdditionalGrade() //Adds more grades to an existing student
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
                    Console.WriteLine($"{studentData[i].name}: {studentData[i].ID}");
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
                            if (ID == studentData[i].ID)
                            {
                                validSelection = true;
                                index = studentData.FindIndex(a => a.ID == ID);
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

            studentData[index].AddGrades();
            WriteToStudentsFile();
        }
        private static void DeleteGrade() //Deletes grade from specific student
        {
            int ID = 0;
            bool validSelection = false;
            int index = -1;

            Console.WriteLine("Please choose which student you want to remove a grade from: ");
            for (int i = 0; i < studentData.Count; i++)
            {
                Console.WriteLine($"{studentData[i].name}: {studentData[i].ID}");
            }

            do
            {
                try
                {
                    ID = int.Parse(Console.ReadLine());
                    index = studentData.FindIndex(a => a.ID == ID);
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
            WriteToStudentsFile();
        }


        //File I/O
        public static void ClearStudentsFile() //Clears student database
        {
            using (TextWriter tw = new StreamWriter(statisticsDatabase, false))
            {
                tw.Write(string.Empty);
            }
        }
        private static void WriteToStudentsFile() //Writes to student Database
        {
            ClearStudentsFile();
            using (StreamWriter sw = File.AppendText(studentDatabase))
            {
                string convertedGrades;

                for (int i = 0; i < studentData.Count; i++)
                {
                    convertedGrades = Menu.ConvertGradesToString(studentData[i].GetGrades());
                    string userInput = $"{studentData[i].ID},{studentData[i].name},{studentData[i].studentsClass},{convertedGrades}";
                    sw.WriteLine(userInput);
                }
            }
        }
    }
}
