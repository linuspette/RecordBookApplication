﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static RecordBookApplication.EntryPoint.Menu;
using static RecordBookApplication.EntryPoint.Classes;

namespace RecordBookApplication.EntryPoint
{
    public class SubjectsManager
    {
        private static string subjectsDatabase;

        public static void SubjectsMenu(List<Subjects> subjectData, List<Classes> classData, string _subjectsDatabase)//User UI for subjects
        {
            subjectsDatabase = _subjectsDatabase;
            string userinput = "";

            while (userinput != "0")
            {
                PrintSubjectsMenu(subjectData);

                userinput = Console.ReadLine();

                switch (userinput)
                {
                    case "1": AddSubject(subjectData); Menu.AwaitUserInput(); break;
                    case "2": DeleteSubject(subjectData, studentData); Menu.AwaitUserInput(); break;
                    case "3": PrintSubjects(subjectData); Menu.AwaitUserInput(); break;
                    case "0": break;
                    default: Console.WriteLine("Not a valid option. Try again."); Menu.AwaitUserInput(); break;
                }
            }
        }
        private static void PrintSubjectsMenu(List<Subjects> subjectData)
        {
            Console.Clear();
            Console.WriteLine(" ---- SUBJECTS MANAGER ---- \n");
            Console.WriteLine($"\n - Amount of Subjects: {subjectData.Count} -\n");
            Console.WriteLine("1 - Add subject");
            Console.WriteLine("2 - Delete subject");
            Console.WriteLine("3 - Show all subjects");
            Console.WriteLine("\n0 - Return to main menu\n");
        }
        private static void PrintSubjects(List<Subjects> subjectData)//Prints the list containing subjects in console
        {
            Console.Clear();
            foreach (var i in subjectData)
            {
                Console.WriteLine($"-{i}-");
            }
        }
        private static void AddSubject(List<Subjects> subjectData)//Adds a subject
        {
            Random rng = new Random();
            Console.WriteLine("Enter the name of the Subject:");
            string addSubject = Console.ReadLine();
            bool subjectExists = false;
            bool validID = false;

            int ID = rng.Next(11111, 99999);

            if (subjectData.Count != 0)
            {
                do
                {
                    for (int i = 0; i < subjectData.Count; i++)
                    {

                        if (ID == subjectData[i].GetSubjectID())
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

            for (int i = 0; i < subjectData.Count; i++)
            {
                if (addSubject == subjectData[i].GetSubjectName())
                {
                    subjectExists = true;
                    break;
                }
                else
                {
                    subjectExists = false;
                }
            }
            if (!subjectExists)
            {
                subjectData.Add(new Subjects(ID, addSubject));
                WriteToSubjectFile(subjectData);
            }
            else
            {
                Console.WriteLine("Subject already exists.");
            }
        }
        public static void AddRandomSubject(List<Subjects> subjectData, string _subjectsDatabase)//Adds a subject
        {
            subjectsDatabase = _subjectsDatabase;
            Console.Clear();
            Random rng = new Random();
            string addSubject = "";
            bool subjectExists = false;
            bool validID = false;
            string[] subjectNames = new string[] { "Svenska", "Engelska", "Matte", "Naturvetenskap", "Samhälllskunskap", "Fysik", "Biologi" };
            string[] subjectDifficulties = new string[] { "1a", "1b", "1c", "2a", "2b", "2c", "3a", "3b", "3c" };

            int error = 0;

            Console.Write($"Creating subject");
            Student.FakeLoading();
            int ID = rng.Next(11111, 99999);

            if (subjectData.Count != 0)
            {
                do
                {
                    for (int i = 0; i < subjectData.Count; i++)
                    {
                        if (ID == subjectData[i].GetSubjectID())
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


            do //Makes sure the subject already isn't added
            {
                addSubject = $"{subjectNames[rng.Next(0, subjectNames.Length)]}{subjectDifficulties[rng.Next(0, subjectDifficulties.Length)]}";

                for (int i = 0; i < subjectData.Count; i++)
                {
                    if (addSubject == subjectData[i].GetSubjectName())
                    {
                        subjectExists = true;
                        error++;
                        break;
                    }
                    else
                    {
                        subjectExists = false;
                    }
                }
            } while (subjectExists && error != 10);

            if (!subjectExists)
            {
                subjectData.Add(new Subjects(ID, addSubject));
                WriteToSubjectFile(subjectData);
            }
            else
            {
                Console.WriteLine("Subject already exists.");
            }
            if (error == 10)
            {
                Console.WriteLine("There was an error when trying to add a random subject. Try again.");
                Thread.Sleep(2000);
            }
        }
        private static void DeleteSubject(List<Subjects> subjectData, List<Student> studentData) //Deletes a subject
        {
            int ID = 0;
            bool validSelection = false;
            int index = -1;

            do
            {
                Console.WriteLine("Please choose which subject you want to remove: ");
                for (int i = 0; i < subjectData.Count; i++)
                {
                    Console.WriteLine($"{subjectData[i].GetSubjectName()}: {subjectData[i].GetSubjectID()}");
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

            subjectData.RemoveAt(index);
            WriteToSubjectFile(subjectData);
        }
        public static int GetSubjectListLenght(List<Subjects> subjectData)
        {
            return subjectData.Count;
        }


        //File I/O

        public static void ClearSubjectFile(string _subjectDatabase) //Clears subject database
        {
            using (TextWriter tw = new StreamWriter(_subjectDatabase, false))
            {
                tw.Write(string.Empty);
            }
        }
        private static void WriteToSubjectFile(List<Subjects> subjectData) //Writes to subject Database
        {
            ClearSubjectFile(subjectsDatabase);
            using (StreamWriter sw = File.AppendText(subjectsDatabase))
            {
                for (int i = 0; i < subjectData.Count; i++)
                {
                    string userInput = $"{subjectData[i].GetSubjectID()},{subjectData[i].GetSubjectName()}";
                    sw.WriteLine(userInput);
                }
            }
        }
    }
}
