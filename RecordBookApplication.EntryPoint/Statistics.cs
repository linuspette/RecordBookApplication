namespace RecordBookApplication.EntryPoint
{
    public class Statistics
    {
        //Values for constructor
        private int statisticID;
        private string typeOfStatistic;
        private double points;



        public Statistics(int _statisticID, string _typeOfStatistics, double _points)
        {
            statisticID = _statisticID;
            typeOfStatistic = _typeOfStatistics;
            points = _points;
        }

        //Data I/O
        public int GetStatisticsID()
        {
            return statisticID;
        }
        public string GetStatisticsType()
        {
            return typeOfStatistic;
        }
        public double GetPoints()
        {
            return points;
        }
        public override string ToString() //Formats string
        {
            return string.Format($"{typeOfStatistic}: {points} points");
        }
    }
}
