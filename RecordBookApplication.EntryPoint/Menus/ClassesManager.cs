using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using static RecordBookApplication.EntryPoint.Menu;
using static RecordBookApplication.EntryPoint.Classes;

namespace RecordBookApplication.EntryPoint
{
    public class ClassesManager
    {
        public static void ClassesManagerMenu()
        {
            string userinput = "";

            while (userinput != "0")
            {
                PrintClassesManagerMenu();

                userinput = Console.ReadLine();

                switch (userinput)
                {
                    case "1": AddClass(); AwaitUserInput(); break;
                    case "2": DeleteClass(); AwaitUserInput(); break;
                    case "3": PrintClasses(); AwaitUserInput(); break;
                    case "0": break;
                    default: Console.WriteLine("Not a valid option. Try again."); AwaitUserInput(); break;
                }
            }
        }
        private static void PrintClassesManagerMenu()
        {
            Console.Clear();
            Console.WriteLine(" ---- CLASS MANAGER ---- \n");
            Console.WriteLine($"\n - Amount of classes: {classData.Count} -\n");
            Console.WriteLine("1 - Add class");
            Console.WriteLine("2 - Delete class");
            Console.WriteLine("3 - Show all classes");
            Console.WriteLine("\n0 - Return to main menu\n");
        }
        private static void PrintClasses()
        {
            Console.Clear();
            int i, j, k;
            for (i = 0; i < classData.Count; i++)
            {
                if (IsUserAdmin())
                {
                    Console.WriteLine("_________________________________________");
                    Console.WriteLine(classData[i]);
                    for (j = 0; j < studentData.Count; j++)
                    {
                        if (classData[i].className == studentData[j].studentsClass)
                        {
                            Console.WriteLine($"Student: {studentData[j].name}");
                            
                        }
                    }
                    Console.WriteLine("_________________________________________");
                }
                else
                {
                    if (userAccessCodes.Count != 0)
                    {
                        for (j = 0; j < userAccessCodes.Count; j++)
                        {
                            if (classData[i].accesCode == userAccessCodes[j])
                            {
                                Console.WriteLine(classData[i]);
                                for (k = 0; k < studentData.Count; k++)
                                {
                                    if (classData[i].className == studentData[j].studentsClass)
                                    {
                                        Console.WriteLine($"Student: {studentData[j].name}");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("You have no access codes associated with your account.");
                        Console.WriteLine("Please contact an administrator.");
                        AwaitUserInput();
                    }
                }
            }
        }
        private static void AddClass()
        {
            Random rng = new Random();
            bool validInput = false;

            //Generates random ID
            int ID = rng.Next(11111, 99999);
            if (classData.Count != 0)
            {
                do
                {
                    for (int i = 0; i < classData.Count; i++)
                    {

                        if (ID == classData[i].classID)
                        {
                            ID = rng.Next(11111, 99999);
                        }
                        else
                        {
                            validInput = true;
                        }
                    }
                } while (!validInput);
            }

            //UserInput for class name
            Console.WriteLine("Enter Class name");
            string className = Console.ReadLine();
            if (classData.Count != 0)
            {
                do
                {
                    for (int i = 0; i < classData.Count; i++)
                    {

                        if (className == classData[i].className)
                        {
                            className = Console.ReadLine();
                            validInput = false;
                            break;
                        }
                        else
                        {
                            validInput = true;
                        }
                    }
                } while (!validInput);
            }

            //Generates random access code
            string accesCode = GenerateAccessCode();
            if (classData.Count != 0)
            {
                do
                {
                    for (int i = 0; i < classData.Count; i++)
                    {

                        if (accesCode == classData[i].accesCode)
                        {
                            accesCode = GenerateAccessCode();
                        }
                        else
                        {
                            validInput = true;
                        }
                    }
                } while (!validInput);
            }

            //Adds data to list
            classData.Add(new Classes(ID, className, accesCode));
            string userInput = $"{ID},{className},{accesCode}";
            SortingMechanisms.SortClasses();
            using (StreamWriter sw = File.AppendText(classesDatabase)) //Writes the data to database
            {
                sw.WriteLine(userInput);
            }
            Console.WriteLine("Class added. Press any key to continue...");
            Console.ReadKey();

        }
        public static void AddRandomClass()
        {
            Random rng = new Random();
            bool validInput = false;

            string[] prefix = new string[] {"1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string[] sufix = new string[] { "a", "b", "c" };

            //Generates a random ID
            int ID = rng.Next(11111, 99999); //Randomizes userID. 
            if (classData.Count != 0)
            {
                do
                {
                    for (int i = 0; i < classData.Count; i++)
                    {

                        if (ID == classData[i].classID)
                        {
                            ID = rng.Next(11111, 99999);
                            validInput = false;
                        }
                        else
                        {
                            validInput = true;
                        }
                    }
                } while (!validInput);
            }

            //Generates a class name
            string className = $"{prefix[rng.Next(0, 8)]}{sufix[rng.Next(0, 3)]}";
            if (classData.Count != 0)
            {
                do
                {
                    for (int i = 0; i < classData.Count; i++)
                    {

                        if (className == classData[i].className)
                        {
                            className = $"{prefix[rng.Next(0, 8)]}{sufix[rng.Next(0, 3)]}";
                            validInput = false;
                        }
                        else
                        {
                            validInput = true;
                        }
                    }
                } while (!validInput);
            }         

           
            //Generates an accescode
            string accesCode = GenerateAccessCode();
            if (classData.Count != 0)
            {
                do
                {
                    for (int i = 0; i < classData.Count; i++)
                    {

                        if (accesCode == classData[i].accesCode)
                        {
                            accesCode = GenerateAccessCode();
                            validInput = false;
                        }
                        else
                        {
                            validInput = true;
                        }
                    }
                } while (!validInput);
            }


            classData.Add(new Classes(ID, className, accesCode));
            string userInput = $"{ID},{className},{accesCode}";

            using (StreamWriter sw = File.AppendText(classesDatabase)) //Writes the data to database
            {
                sw.WriteLine(userInput);
            }
            Console.Write($"Creating class");
            FakeLoading();

        }
        private static void DeleteClass()
        {

        }
        private static string GenerateAccessCode()
        {
            Random rng = new Random();
            const string chars = "QWERTYUIOPASDFGHJKLZXCVBNM" +
                                 "qwertyuiopasdfghjklzxcvbnm" +
                                 "123456789";
            return new string(Enumerable.Repeat(chars, 10)
                             .Select(s => s[rng.Next(s.Length)]).ToArray());
        }
    }
}
