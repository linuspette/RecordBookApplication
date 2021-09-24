using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordBookApplication.EntryPoint
{
    public class Student
    {
        private int ID;
        private string name;
        private string subject;
        private double grade;


        public Student(int _id, string _name, string _subject, double _grade)
        {
            ID = _id;
            name = _name;
            subject = _subject;
            grade = _grade;

        }

        public int GetID()
        {
            return ID;
        }

        public string GetName()
        {
            return name;
        }

        public string GetSubject()
        {
            return subject;
        }

        public double GetGrade()
        {
            return grade;
        }

        public override string ToString()
        {
            return string.Format($"ID: {ID} \nNamn: {name} \n{subject}: {grade} points\n");
        }
    }
}
