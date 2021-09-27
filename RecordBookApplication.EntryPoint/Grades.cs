using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordBookApplication.EntryPoint
{
    public class Grades
    {
        private int gradeID;
        private string subject;
        private string grade;
        public Grades(int _gradeID, string _subject, string _grade)
        {
            gradeID = _gradeID;
            subject = _subject;
            grade = _grade;
        }
        public int GetGradeID()
        {
            return gradeID;
        }
        public string GetGradeSubject()
        {
            return subject;
        }
        public string GetGradeGrade()
        {
            return grade;
        }
        public string SendGrades()
        {
            string grades = $"{gradeID},{subject},{grade}";

            return grades;
        }
        public override string ToString() //Formats string
        {
            return string.Format($"{subject}: {grade}");
        }
    }
}