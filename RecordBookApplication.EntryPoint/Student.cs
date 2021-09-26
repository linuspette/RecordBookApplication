using System;
using System.Collections.Generic;

namespace RecordBookApplication.EntryPoint
{
    public class Student
    {
        private int ID;
        private string name;
        private List<Grades> grades = new List<Grades>();

        public Student(int _id, string _name, string command)
        {
            ID = _id;
            name = _name;
            if (command == "notRandom")
            {
                AddGrades();
            }            
            if (command == "Random")
            {
                RandomizeGrades();
            }
            if (command == "initiating")
            {

            }

        }
        public int GetID() //Gives UserID
        {
            return ID;
        }
        public string GetName() //Gives user Name
        {
            return name;
        }
        public void ReceiveGradeData(List<Grades> _grades)
        {
            grades = _grades;
        }
        public void AddGradesOnStartUp(int gradeID, string subject, string grade)
        {
            grades.Add(new Grades(gradeID, subject, grade));
        }
        public void AddGrades() //Lets user add grades to the specific ID
        {
            string[] acceptedGrades = new string[] { "A", "B", "C", "D", "E", "F", "-" };
            bool validSelection = false;
            string subject = "";
            string grade = "";
            Random rng = new Random();

            bool validID = false;

            int gradeID = rng.Next(11111, 99999); //Randomizes userID. 

            if (grades.Count != 0)
            {
                do
                {
                    for (int i = 0; i < grades.Count; i++)
                    {

                        if (ID == grades[i].GetGradeID())
                        {
                            ID = rng.Next(11111, 99999);
                        }
                        else
                        {
                            validID = true;
                        }
                    }
                } while (!validID);
            }//Makes sure that the ID ins't used by another grade. 

            do //Checks which subject that should be graded
            {
                Console.Clear();
                Console.WriteLine("Choose a subject to grade:");
                Console.WriteLine("1 - Swedish");
                Console.WriteLine("2 - English");
                Console.WriteLine("3 - Math");
                Console.WriteLine("4 - Science");
                Console.WriteLine("5 - Religion");
                switch (subject = Console.ReadLine())
                {
                    case "1": subject = "Swedish"; validSelection = true; break;
                    case "2": subject = "English"; validSelection = true; break;
                    case "3": subject = "Math"; validSelection = true; break;
                    case "4": subject = "Science"; validSelection = true; break;
                    case "5": subject = "Religion"; validSelection = true; break;
                    default: Console.WriteLine("Please choose a valid alternative."); validSelection = false; break;
                }
            } while (!validSelection);

            do //Checks so that user did input a valid grade
            {
                Console.Clear();
                Console.WriteLine("Set the grade");
                Console.WriteLine("\nGrades that you can set:");
                Console.WriteLine(" | A | | B | | C | | D | | E | | F | | - | ");

                grade = Console.ReadLine().ToUpper();
                for (int i = 0; i < acceptedGrades.Length; i++)
                {
                    if (grade == acceptedGrades[i].ToString())
                    {
                        validSelection = true;
                    }
                }

                if (validSelection)
                {
                    grades.Add(new Grades(gradeID, subject, grade));
                }
                else
                {
                    validSelection = false;
                    Console.WriteLine("Please choose a valid alternative.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!validSelection);
        }
        public void RemoveGrades()
        {
            int ID = 0;
            bool validSelection = false;
            int index = -1;

            while (!validSelection)
            {
                Console.WriteLine($"Available grades for {name}: ");
                for (int i = 0; i < grades.Count; i++)
                {
                    Console.WriteLine($"{grades[i].GetGradeID()} - {grades[i].GetGradeSubject()}: {grades[i].GetGradeGrade()}");
                }
                Console.WriteLine("\nPlease enter the ID of the grade you wish to remove: ");
                try
                {
                    ID = int.Parse(Console.ReadLine());
                    index = grades.FindIndex(a => a.GetGradeID() == ID);
                    validSelection = true;
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Please choose a valid ID.");

                }
                if (index == -1)
                {
                    validSelection = false;
                    Console.Clear();
                    Console.WriteLine("Please choose a valid ID.");
                }
            }
            grades.RemoveAt(index);
        }
        public void RandomizeGrades() //Adds a randomized grade to specific ID
        {

            string[] _grades = new string[] { "A", "B", "C", "D", "E", "F", "-" }; //Array that contains valid grades
            string[] _subjects = new string[] { "Swedish", "English", "Math", "Science", "Religion" }; //Array that contains valid subjects

            Random rng = new Random();

            bool validID = false;

            int gradeID = rng.Next(11111, 99999); //Randomizes userID. 

            if (grades.Count != 0)
            {
                do
                {
                    for (int i = 0; i < grades.Count; i++)
                    {

                        if (ID == grades[i].GetGradeID())
                        {
                            ID = rng.Next(11111, 99999);
                        }
                        else
                        {
                            validID = true;
                        }
                    }
                } while (!validID);
            }//Makes sure that the ID ins't used by another grade. 

            string subject = _subjects[rng.Next(0,5)];
            string grade = _grades[rng.Next(0,7)];

            grades.Add(new Grades(gradeID, subject, grade));
        }
        public List<String> GetGrades()
        {
            string s = "";
            string d = "";
            string gradeInfo = "";

            List<string> receivedInfo = new List<string>();

            for (int i = 0; i < grades.Count; i++)
            {
                var tmp = grades[i].SendGrades().Split(',');
                gradeInfo = $"{tmp[0]},{tmp[1]},{tmp[2]}";
                receivedInfo.Add(new string(gradeInfo));
            }

            return receivedInfo;
        }
        public string PrintGrades()//Prints grades
        {
            string text = "";
            foreach (var i in grades)
            {
                text += $"{i}\n";
            }

            return text;
        }
        public override string ToString() //Formats string
        {
            return string.Format($"ID: {ID} \nNamn: {name} \n{PrintGrades()}");
        }
    }
}
