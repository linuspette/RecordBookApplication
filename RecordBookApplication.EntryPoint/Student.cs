using System;
using System.Collections.Generic;
using System.Threading;

namespace RecordBookApplication.EntryPoint
{
    public class Student
    {
        private int ID;
        private string name;
        private List<Grades> grades = new List<Grades>();

        public Student(int _id, string _name, string command, List<Subjects> subjectData)
        {
            ID = _id;
            name = _name;
            if (command == "notRandom")
            {
                AddGrades(subjectData);
            }            
            if (command == "Random")
            {
                RandomizeGrades(subjectData);
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
        public void AddGrades(List<Subjects> subjectData) //Lets user add grades to the specific ID
        {
            string[] acceptedGrades = new string[] { "A", "B", "C", "D", "E", "F", "-" };
            bool validSelection = false;
            int subjectSelection = 0;
            string subject = "";
            string grade = "";
            Random rng = new Random();

            string tryAgain = "";

            bool validID = false;
            bool gradeExists = true;

            int gradeID = rng.Next(11111, 99999); //Randomizes userID. 

            if (subjectData.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("There's no subjects available.\nAdd a subject via the main menu first.");
                Thread.Sleep(2000);
                tryAgain = "n";
            }

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
            }//Makes sure that the ID isn't used by another grade. 

            while (gradeExists && tryAgain != "n" && subjectData.Count != 0)
            {
                do //Checks which subject that should be graded
                {
                    Console.Clear();
                    Console.WriteLine("Please select the subject to be graded:");
                    for (int i = 0; i < subjectData.Count; i++)
                    {
                        Console.WriteLine($"{i+1} - {subjectData[i]}");
                    }

                    do
                    {
                        try
                        {
                            subjectSelection = int.Parse(Console.ReadLine());
                            if(subjectSelection < 1)
                            {
                                validSelection = false;
                                Console.WriteLine("Please choose a valid option.");
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                validSelection = true;
                            }
                        }
                        catch
                        {
                            validSelection = false;
                            Console.WriteLine("Please choose a valid option.");
                            Thread.Sleep(1000);
                        }
                    } while (!validSelection);

                    subject = subjectData[subjectSelection-1].GetSubjectName();

                    for (int i = 0; i < subjectData.Count; i++)
                    {
                        if (subject == subjectData[i].GetSubjectName())
                        {
                            validSelection = true;
                            break;
                        }
                        else
                        {
                            validSelection = false;
                        }
                    }

                } while (!validSelection);

                if (grades.Count != 0)
                {
                    for (int i = 0; i < grades.Count; i++)
                    {
                        if (subject == grades[i].GetGradeSubject())
                        {
                            Console.Clear();
                            Console.WriteLine("This subject is already graded. Please delete the existing grade first.");
                            Thread.Sleep(2000);
                            gradeExists = true;
                            do
                            {
                                Console.Clear();
                                Console.WriteLine("Do you want to try adding a grade again? y/n");

                                switch (tryAgain = Console.ReadLine().ToLower())
                                {
                                    case "y":
                                        Console.Clear();
                                        validSelection = true;
                                        break;
                                    case "n": validSelection = true; break;
                                    default: Console.Clear(); Console.WriteLine("Please choose a valid alternative.\nPress any key to continue..."); Console.ReadKey(); validSelection = false; break;
                                }
                            } while (!validSelection);
                        }
                        else
                        {
                            gradeExists = false;
                        }
                        if (gradeExists == true)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    gradeExists = false;
                }
            }

            if (gradeExists == false && subjectData.Count != 0)
            {
                do //Checks so that user did input a valid grade
                {
                    Console.Clear();
                    Console.WriteLine("Set the grade");
                    Console.WriteLine("\nGrades that you can set:");
                    Console.WriteLine(" | A | | B | | C | | D | | E | | F | | - | ");

                    grade = Console.ReadLine().ToUpper();
                    for (int i = 0; i < acceptedGrades.Length; i++)
                    {
                        if (grade == acceptedGrades[i])
                        {
                            validSelection = true;
                            break;
                        }
                        else
                        {
                            validSelection = false;
                        }
                    }

                    if (validSelection)
                    {
                        grades.Add(new Grades(gradeID, subject, grade));
                    }
                    else
                    {
                        Console.WriteLine("Please choose a valid alternative.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!validSelection);
            }
        }
        public void RemoveGrades()
        {
            int ID = 0;
            bool validSelection = false;
            int index = -1;

            if (grades.Count != 0)
            {
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
            else
            {
                Console.Clear();
                Console.WriteLine("There's no grades to be removed.");
                Thread.Sleep(2000);
            }
        }
        public void RandomizeGrades(List<Subjects> subjectData) //Adds a randomized grade to specific ID
        {

            string[] _grades = new string[] { "A", "B", "C", "D", "E", "F", "-" }; //Array that contains valid grades

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
            int index = rng.Next(0, subjectData.Count);
            string subject = subjectData[index].GetSubjectName();
            string grade = _grades[rng.Next(0,7)];

            grades.Add(new Grades(gradeID, subject, grade));
        }
        public List<String> GetGrades()
        {
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
        public void SortGradesBySubject()
        {
            grades.Sort((x, y) => string.Compare(x.GetGradeSubject(), y.GetGradeSubject()));
        }        
        public void SortGradesByGrade()
        {
            grades.Sort((x, y) => string.Compare(x.GetGradeGrade(), y.GetGradeGrade()));
        }
        public void ReverseSortedGrades()
        {
            grades.Reverse();
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