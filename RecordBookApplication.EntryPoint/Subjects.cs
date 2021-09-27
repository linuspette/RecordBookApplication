namespace RecordBookApplication.EntryPoint
{
    public class Subjects
    {
        private string name;
        private int subjectID;
        public Subjects(int _subjectID, string _name)
        {
            name = _name;
            subjectID = _subjectID;
        }
        public string GetSubjectName()
        {
            return name;
        }
        public int GetSubjectID()
        {
            return subjectID;
        }
        public override string ToString() //Formats string
        {
            return string.Format($"{name}");
        }
    }
}