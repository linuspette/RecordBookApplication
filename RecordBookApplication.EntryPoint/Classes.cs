using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RecordBookApplication.EntryPoint.Menu;

namespace RecordBookApplication.EntryPoint
{
    public class Classes
    {
        public int classID { get; set; }
        public string className { get; set; }
        public static List<Student> studentData { get; private set; } = new List<Student>();
        public Classes(int _classID, string _className)
        {
            this.classID = _classID;
            this.className = _className;
        }
        public void AddStudentData(int id, string name, string command, List<Subjects> subjectData)
        {
            studentData.Add(new Student(id, name, className, command, subjectData));
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
    }
}
