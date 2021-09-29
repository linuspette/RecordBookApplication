using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordBookApplication.EntryPoint
{
    public class SortingMechanisms
    {
        static string data = "studentDatabase.xml";


        public static void SortMenu(List<Student> studentData) //UI that lets user choose which data to sort
        {

            string userinput = "";
            bool validSelection = false;

            while (!validSelection && userinput != "0") //Checks which data user wants to sort after
            {
                PrintSortMenu();
                userinput = Console.ReadLine();

                switch (userinput)
                {
                    case "1": SortStudentMenu(studentData); validSelection = true; break;
                    case "2": SortGradesMenu(studentData); validSelection = true; break;
                    case "0": break;
                    default: Console.Clear(); Console.WriteLine("Please select a valid option"); validSelection = false; break;
                }
            }
            Console.Clear();
        }
        private static void PrintSortMenu()
        {
            Console.Clear();
            Console.WriteLine(" ---- SORTING ----- \n");
            Console.WriteLine("1 - Student data");
            Console.WriteLine("2 - Grades data");
            Console.WriteLine("\n0 - Return to main menu\n");
        }

        //Sorting mechanisms
        private static void SortStudentMenu(List<Student> studentData) //Menu that let's user choose how they want to sort the data
        {
            string userinput = "";
            bool validSelection = false;

            while (!validSelection) //Checks which data user wants to sort after
            {
                Console.Clear();
                Console.WriteLine("How do you want to sort the data?");
                Console.WriteLine("1 - By ID");
                Console.WriteLine("2 - By name");

                userinput = Console.ReadLine();

                switch (userinput)
                {
                    case "1": studentData = SortStudent(studentData, "ID"); validSelection = true; break;
                    case "2": studentData = SortStudent(studentData, "Name"); validSelection = true; break;
                    default: Console.Clear(); Console.WriteLine("Please select a valid option"); validSelection = false; break;
                }
            }
            Console.Clear();
            WriteToFile(studentData);
        }
        private static void SortGradesMenu(List<Student> studentData) //Menu that let's user choose how they want to sort the data
        {
            string userinput = "";
            bool validSelection = false;

            while (!validSelection) //Checks which data user wants to sort after
            {
                Console.Clear();
                Console.WriteLine("How do you want to sort the data?");
                Console.WriteLine("1 - By subject");
                Console.WriteLine("2 - By grades");

                userinput = Console.ReadLine();

                switch (userinput)
                {
                    case "1": studentData = SortGrades(studentData, "Subject"); validSelection = true; break;
                    case "2": studentData = SortGrades(studentData, "Grade"); validSelection = true; break;
                    default: Console.Clear(); Console.WriteLine("Please select a valid option"); validSelection = false; break;
                }
            }
            Console.Clear();
            WriteToFile(studentData);
        }
        private static List<Student> SortStudent(List<Student> studentData, string getInfo) //Sorts the list after choosen type
        {
            int i, j;

            switch (getInfo)
            {
                case "ID": //Sorts list after ID using insertion sort
                    for (i = 1; i < studentData.Count; i++)
                    {
                        for (j = i; j > 0; j--)
                        {
                            if (studentData[j].GetID() < studentData[j - 1].GetID())
                            {
                                var tmp = studentData[j - 1];
                                studentData[j - 1] = studentData[j];
                                studentData[j] = tmp;
                            }
                        }
                    }
                    break;
                case "Name": //Sorts list after names
                    studentData.Sort((x, y) => string.Compare(x.GetName(), y.GetName()));
                    break;
            }
            return studentData;
        }
        private static List<Student> SortGrades(List<Student> studentData, string getInfo)
        {
            int i;
            string userInput = "";
            bool validSelection = false;

            switch (getInfo)
            {
                case "Subject": //Sorts list after subjects. 
                    for (i = 0; i < studentData.Count; i++)
                    {
                        studentData[i].SortGradesBySubject();
                    }
                    break;
                case "Grade": //Sorts list after grades using InsertionSort. User can choose if the order should ascend/descend
                    for (i = 0; i < studentData.Count; i++)
                    {
                        studentData[i].SortGradesByGrade();
                    }

                    do //Checks which order user wants the grades sorted
                    {
                        Console.Clear();
                        Console.WriteLine("Should the order be ascending or descending?");
                        Console.WriteLine("1 - Ascending");
                        Console.WriteLine("2 - Descending");
                        switch (userInput = Console.ReadLine())
                        {
                            case "1": validSelection = true; break;
                            case "2": validSelection = true; break;
                            default: Console.WriteLine("Please choose a valid option."); validSelection = false; break;
                        }
                    } while (!validSelection);
                    if (userInput == "1")
                    {
                        for (i = 0; i < studentData.Count; i++)
                        {
                            studentData[i].ReverseSortedGrades();
                        }
                    }
                    break;
            }
            return studentData;
        }
        public static string ConvertGradesToString(List<string> grades)//Prints grades
        {
            string text = "";
            for (int i = 0; i < grades.Count; i++)
            {
                if (i > 0)
                {
                    text += $",{grades[i]}";
                }
                else
                {
                    text += $"{grades[i]}";
                }
            }
            return text;
        }


        //File I/O
        private static void ClearFile() //Clears student database
        {
            using (TextWriter tw = new StreamWriter(data, false))
            {
                tw.Write(string.Empty);
            }
        }
        private static void WriteToFile(List<Student> studentData) //Writes to student Database
        {
            ClearFile();
            using (StreamWriter sw = File.AppendText(data))
            {
                string convertedGrades;

                for (int i = 0; i < studentData.Count; i++)
                {
                    convertedGrades = ConvertGradesToString(studentData[i].GetGrades());
                    string userInput = $"{studentData[i].GetID()},{studentData[i].GetName()},{convertedGrades}";
                    sw.WriteLine(userInput);
                }
            }
        }
    }
}
