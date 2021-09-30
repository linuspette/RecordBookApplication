using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RecordBookApplication.EntryPoint.Menu;
using static RecordBookApplication.EntryPoint.Classes;

namespace RecordBookApplication.EntryPoint
{
    public static class StatisticsMechanisms
    {
        private static readonly double dash = 0, F = 0, E = 10, D = 12.5, C = 15, B = 17.5, A = 20; //Grade values
        private static double dashCount = 0, fCount = 0, eCount = 0, dCount = 0, cCount = 0, bCount = 0, aCount = 0; //Counts how many grades of each type

        //Statistics menu
        public static void StatisticsMenu()//User UI for statistics
        {
            string userinput = "";

            while (userinput != "0")
            {
                PrintStatisticsMenu();

                userinput = Console.ReadLine();
                switch (userinput)
                {
                    case "1": PrintStatistics(); Menu.AwaitUserInput(); break;
                    case "2": GetLowest(); Menu.AwaitUserInput(); break;
                    case "3": GetHighest(); Menu.AwaitUserInput(); break;
                    case "4": CalcAverage(); Menu.AwaitUserInput(); break;
                    case "0": break;
                    default: Console.WriteLine("Not a valid option. Try again."); Menu.AwaitUserInput(); break;
                }
            }
        }
        private static void PrintStatisticsMenu()//Prints statistics menu
        {
            Console.Clear();
            Console.WriteLine(" ---- STATISTICS ---- \n");
            Console.WriteLine("1 - Show all statistics");
            Console.WriteLine("2 - Show student with the lowest grade");
            Console.WriteLine("3 - Show student with the highest grade");
            Console.WriteLine("4 - Show the average grades");
            Console.WriteLine("\n0 - Return to main menu\n");
        }
        public static void PrintStatistics()//Prints the list containing statistics in console
        {
            foreach (var i in statisticData)
            {
                Console.WriteLine(i);
            }
        }


        //Interacting with statistics
        private static void GetLowest() //Gets the student with the lowest grades
        {
            double studentTotalPoints = 0;
            double savedStudentPoints = 0;
            int savedStudentIndex = 0;
            bool statisticsExists = false;

            List<string> temp;

            if (studentData.Count != 0)
            {
                for (int i = 0; i < studentData.Count; i++)
                {
                    studentTotalPoints = 0;
                    for (int j = 0; j < studentData[i].GetGradesForStatistics().Count; j++)
                    {
                        temp = studentData[i].GetGradesForStatistics();
                        foreach (string index in temp)
                        {
                            switch (index)
                            {
                                case "-": dashCount++; break;
                                case "F": fCount++; break;
                                case "E": eCount++; break;
                                case "D": dCount++; break;
                                case "C": cCount++; break;
                                case "B": bCount++; break;
                                case "A": aCount++; break;
                            }
                        }
                        studentTotalPoints = ((dash * dashCount) + (F * fCount) + (E * eCount) + (D * dCount) + (C * cCount) + (B * bCount) + (A * aCount));
                        CounterReset();
                        if (i == 0)
                        {
                            savedStudentPoints = studentTotalPoints;
                            savedStudentIndex = i;
                        }
                        else
                        {
                            if (studentTotalPoints < savedStudentPoints)
                            {
                                savedStudentPoints = studentTotalPoints;
                                savedStudentIndex = i;
                            }
                        }
                    }
                }
                if (statisticData.Count == 0)
                {
                    statisticData.Add(new Statistics(1, "LowestGrade", savedStudentPoints));
                }
                else
                {
                    for (int i = 0; i < statisticData.Count; i++)
                    {
                        //Checks if statistics already exists. If so, the statistics will be overwritten.
                        if (1 == statisticData[i].GetStatisticsID())
                        {
                            statisticData.RemoveAt(i);
                            statisticData.Add(new Statistics(1, "LowestGrade", savedStudentPoints));
                            statisticsExists = true;
                        }
                        //If statistics doesn't exists: writes data to list
                        else
                        {
                            statisticsExists = false;
                        }
                        if (!statisticsExists)
                        {
                            statisticData.Add(new Statistics(1, "LowestGrade", savedStudentPoints));
                        }
                    }
                }
                Console.Clear();
                Console.WriteLine("The student with the lowest grades are:");
                Console.WriteLine($"{studentData[savedStudentIndex]}\nTotal points: {savedStudentPoints}");
                WriteToStatisticsFile();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Theres no students saved in database.");
            }
        }
        private static void GetHighest() //Gets the student with the highest grades
        {
            double studentTotalPoints = 0;
            double savedStudentPoints = 0;
            int savedStudentIndex = 0;
            int ID = 2;
            List<string> temp;
            bool statisticsExists = false;

            if (studentData.Count != 0)
            {
                for (int i = 0; i < studentData.Count; i++)
                {
                    studentTotalPoints = 0;
                    for (int j = 0; j < studentData[i].GetGradesForStatistics().Count; j++)
                    {
                        temp = studentData[i].GetGradesForStatistics();
                        foreach (string index in temp)
                        {
                            switch (index)
                            {
                                case "-": dashCount++; break;
                                case "F": fCount++; break;
                                case "E": eCount++; break;
                                case "D": dCount++; break;
                                case "C": cCount++; break;
                                case "B": bCount++; break;
                                case "A": aCount++; break;
                            }
                        }
                        studentTotalPoints = ((dash * dashCount) + (F * fCount) + (E * eCount) + (D * dCount) + (C * cCount) + (B * bCount) + (A * aCount));
                        CounterReset();
                        if (i == 0)
                        {
                            savedStudentPoints = studentTotalPoints;
                            savedStudentIndex = i;
                        }
                        else
                        {
                            if (studentTotalPoints > savedStudentPoints)
                            {
                                savedStudentPoints = studentTotalPoints;
                                savedStudentIndex = i;
                            }
                        }
                    }
                }
                if (statisticData.Count == 0)
                {
                    statisticData.Add(new Statistics(2, "HighestGrade", savedStudentPoints));
                }
                else
                {
                    for (int i = 0; i < statisticData.Count; i++)
                    {
                        //Checks if statistics already exists. If so, the statistics will be overwritten.
                        if (ID == statisticData[i].GetStatisticsID())
                        {
                            statisticData.RemoveAt(i);
                            statisticData.Add(new Statistics(ID, "HighestGrade", savedStudentPoints));
                            statisticsExists = true;
                            break;
                        }
                        //If statistics doesn't exists: writes data to list
                        else
                        {
                            statisticsExists = false;
                        }
                        if (!statisticsExists)
                        {
                            statisticData.Add(new Statistics(ID, "HighestGRrade", savedStudentPoints));
                        }
                    }
                }
                Console.Clear();
                Console.WriteLine("The student with the highest grades are:");
                Console.WriteLine($"{studentData[savedStudentIndex]}\nTotal points: {savedStudentPoints}");
                WriteToStatisticsFile();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Theres no students saved in database.");
            }
        }
        private static void CalcAverage() //Gets the average grades of all students
        {
            double studentTotalPoints = 0;
            bool statisticsExists = false;

            List<string> temp;

            if (studentData.Count != 0)
            {
                for (int i = 0; i < studentData.Count; i++)
                {
                    studentTotalPoints = 0;
                    for (int j = 0; j < studentData[i].GetGradesForStatistics().Count; j++)
                    {
                        temp = studentData[i].GetGradesForStatistics();
                        foreach (string index in temp)
                        {
                            switch (index)
                            {
                                case "-": dashCount++; break;
                                case "F": fCount++; break;
                                case "E": eCount++; break;
                                case "D": dCount++; break;
                                case "C": cCount++; break;
                                case "B": bCount++; break;
                                case "A": aCount++; break;
                            }
                        }
                        studentTotalPoints = ((dash * dashCount) + (F * fCount) + (E * eCount) + (D * dCount) + (C * cCount) + (B * bCount) + (A * aCount));
                    }
                }
                if (statisticData.Count == 0)
                {
                    statisticData.Add(new Statistics(3, "AverageGrade", Math.Round(studentTotalPoints / studentData.Count, 2)));
                }
                else
                {
                    for (int i = 0; i < statisticData.Count; i++)
                    {
                        //Checks if statistics already exists. If so, the statistics will be overwritten.
                        if (3 == statisticData[i].GetStatisticsID())
                        {
                            statisticData.RemoveAt(i);
                            statisticData.Add(new Statistics(3, "AverageGrade", Math.Round(studentTotalPoints / studentData.Count, 2)));
                            statisticsExists = true;
                            break;
                        }
                        //If statistics doesn't exists: writes data to list
                        else
                        {
                            statisticsExists = false;
                        }
                    }
                    if (!statisticsExists)
                    {
                        statisticData.Add(new Statistics(3, "AverageGrade", Math.Round(studentTotalPoints / studentData.Count, 2)));
                    }
                }
                Console.Clear();
                Console.WriteLine("The average grades are:");
                Console.WriteLine($"Total A: {aCount}\n" +
                                  $"Total B: {bCount}\n" +
                                  $"Total C: {cCount}\n" +
                                  $"Total D: {dCount}\n" +
                                  $"Total E: {eCount}\n" +
                                  $"Total F: {fCount}\n" +
                                  $"Total -: {dashCount}\n" +
                                  $"Average points: {Math.Round(studentTotalPoints / studentData.Count, 2)}");
                WriteToStatisticsFile();
                CounterReset();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Theres no students saved in database.");
            }
        }
        private static void CounterReset() //Resets the counter
        {
            dashCount = 0;
            fCount = 0;
            eCount = 0;
            dCount = 0;
            cCount = 0;
            bCount = 0;
            aCount = 0;
        }

        //File I/O
        public static void ClearStatisticsFile() //Clears statistics database
        {
            using (TextWriter tw = new StreamWriter(statisticsDatabase, false))
            {
                tw.Write(string.Empty);
            }
        }
        private static void WriteToStatisticsFile() //Writes to student Database
        {
            ClearStatisticsFile();
            using (StreamWriter sw = File.AppendText(statisticsDatabase))
            {
                for (int i = 0; i < statisticData.Count; i++)
                {
                    string points = string.Join(System.Environment.NewLine, statisticData[i].GetPoints());
                    string userInput = $"{statisticData[i].GetStatisticsID()},{statisticData[i].GetStatisticsType()},{points.Replace(',', '.')}";
                    sw.WriteLine(userInput);
                }
            }
        }
    }
}
