using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordBookApplication.EntryPoint
{
    public class Statistics
    {
        private Student GetLowest(List<Student> studentData)
        {
            double min = studentData[0].GetGrade();
            var lowestGrade = studentData[0];

            for (int i = 0; i < studentData.Count; i++)
            {
                if (min > studentData[i].GetGrade())
                {
                    min = studentData[i].GetGrade();
                    lowestGrade = studentData[i];
                }
            }

            return lowestGrade;
        }

        private Student GetHighest(List<Student> studentData)
        {
            double max = studentData[0].GetGrade();
            var highestGrade = studentData[0];

            for (int i = 0; i < studentData.Count; i++)
            {
                if (studentData[i].GetGrade() > max)
                {
                    max = studentData[i].GetGrade();
                    highestGrade = studentData[i];
                }
            }

            return highestGrade;
        }

        private double CalcAverage(List<Student> studentData)
        {
            double total = 0.0;

            for (int i = 0; i < studentData.Count; i++)
            {
                total += studentData[i].GetGrade();
            }

            return total / studentData.Count;
        }

        public void ComputeStatistics(List<Student> studentData)
        {
            Console.WriteLine($"The Average grade is {CalcAverage(studentData)}");
            Console.WriteLine($"The student with the highest grade: \n{GetHighest(studentData)}");
            Console.WriteLine($"The student with the highest grade: {GetLowest(studentData)}");
        }
    }
}
