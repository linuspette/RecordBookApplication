using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using static RecordBookApplication.EntryPoint.Menu;

namespace RecordBookApplication.EntryPoint
{
    public class Classes
    {
        public int classID { get; set; }
        public string className { get; set; }
        public string accesCode { get; private set; }
        public static List<Student> studentData { get; set; } = new List<Student>();
        public Classes(int _classID, string _className, string _accesCode)
        {
            this.classID = _classID;
            this.className = _className;
            this.accesCode = _accesCode;
        }
        public void AddStudentData(int id, string name, string _className, string command)
        {
            studentData.Add(new Student(id, name, _className, command));
        }
        public List<Student> GetStudentData()
        {
            return studentData;
        }
        public static string AddStudentToRandomClass()
        {
            Random rng = new Random();
            return classData[rng.Next(0, classData.Count)].className;
        }


        //File I/O
        public static void ClearClassesFile() //Clears student database
        {
            using (TextWriter tw = new StreamWriter(classesDatabase, false))
            {
                tw.Write(string.Empty);
            }
        }
        private static void WriteToClassesFile() //Writes to student Database
        {
            FileEncryption.DeCrypt(classesDatabase);
            ClearClassesFile();
            SortingMechanisms.SortClasses();
            using (StreamWriter sw = File.AppendText(classesDatabase))
            {

                for (int i = 0; i < classData.Count; i++)
                {
                    string userInput = $"{classData[i].classID},{classData[i].className}";
                    sw.WriteLine(userInput);
                }
            }
        }


        public string PrintStudents()//Prints grades
        {
            string text = "";
            foreach (var i in studentData)
            {
                text += $"{i}\n";
            }

            return text;
        }
        public override string ToString() //Formats string
        {
            return string.Format($"\nKlass namn: {className} \n\n");
        }

    }
}
