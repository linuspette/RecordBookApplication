using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordBookApplication.EntryPoint
{
    public class SearchEngine
    {
        public static void SearchMenu(List<Student> studentData)
        {
            string userInput = string.Empty;
            string findClass = string.Empty;

            do
            {
                Console.Clear();
                Console.WriteLine("Please select your search criteria:");
                Console.WriteLine("1 - By name" +
                                  "\n2 - By ID" +
                                  "\n3 - By class(WIP)" +
                                  "\n\n0 - Main menu");
                userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1": Console.Clear(); FindStudentByName(studentData); Menu.AwaitUserInput(); break;
                    case "2": Console.Clear(); FindStudentByID(studentData); Menu.AwaitUserInput(); break;
                    case "0": break;
                    default: break;
                }

            } while (userInput != "0");
        }
        private static void FindStudentByID(List<Student> studentData)
        {
            string findID = string.Empty;
            List<Student> searchResult = new List<Student>();

            findID = Console.ReadLine();

            for (int i = 0; i < studentData.Count; i++)
            {
                if (studentData[i].ID.ToString().Contains(findID))
                {
                    searchResult.Add(studentData[i]);
                }
            }
            if (searchResult.Count != 0)
            {
                foreach (var i in searchResult)
                {
                    Console.WriteLine(i);
                }
            }
            else
            {
                Console.WriteLine("No studentmatches your criteria.");
            }
            searchResult.Clear();
        }
        private static void FindStudentByName(List<Student> studentData)
        {
            string findName = string.Empty;
            List<Student> searchResult = new List<Student>();

            findName = Console.ReadLine().ToLower();

            for (int i = 0; i < studentData.Count; i++)
            {
                if (studentData[i].name.ToLower().Contains(findName))
                {
                    searchResult.Add(studentData[i]);
                }
            }
            if (searchResult.Count != 0)
            {
                foreach (var i in searchResult)
                {
                    Console.WriteLine(i);
                }
            }
            else
            {
                Console.WriteLine("No studentmatches your criteria.");
            }
            searchResult.Clear();
        }
    }
}
