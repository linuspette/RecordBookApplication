using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace RecordBookApplication.EntryPoint
{
    public class Menu
    {
        string savedSubjects = "subjectDatabase.txt";
        string data = "studentDatabase.txt";
        string statisticsData = "statisticsDatabase.txt";


        List<Student> studentData = new List<Student>();
        public List<Subjects> subjectData = new List<Subjects>();
        //List that stores statistical data
        private List<Statistics> statisticData = new List<Statistics>();

        public void Run() //Initiates program and loads data from databases
        {
            Console.WriteLine("Looking for existing databases...");
            FakeLoading();

            //Checks if subjects database exist.
            //If it exists, data will be written to corresponding lists
            if (File.Exists(savedSubjects))
            {
                Console.WriteLine("Database containing subjects exists. Loading data...");
                FakeLoading();
                int error = 0; //Calculates eventually errors
                using (StreamReader file = new StreamReader(savedSubjects))
                {
                    int rowsChecked = 0; //Keeps track of how many rows have been checked
                    int amountOfLines = File.ReadLines(savedSubjects).Count(); //Calculates amount of lines in the txt-file

                    string line;
                    string[] dataText;

                    for (int Ind = 0; Ind < amountOfLines; Ind++)
                    {
                        line = File.ReadLines(savedSubjects).Skip(rowsChecked).Take(1).First(); //Selects the row from txt-file that should be read
                        rowsChecked++;

                        if (line != null)
                        {
                            try
                            {
                                dataText = line.Split(',');
                                subjectData.Add(new Subjects(int.Parse(dataText[0]), dataText[1]));
                            }
                            catch
                            {
                                error++;
                            }
                        }
                    }

                    if (error != 0)
                    {
                        Console.WriteLine("There was a problem when reading the data from the database.");
                        Console.WriteLine($"Total errors found: {error}");
                    }
                }
            }
            //If subject database doesn't exists, a new database is created
            else
            {
                using (File.Create(savedSubjects)) { };
                Console.WriteLine("No subjects database found. Creating new database...");
                FakeLoading();
            }


            //Checks if statistics database exist.
            //If it exists, data will be written to corresponding lists
            if (File.Exists(statisticsData))
            {
                Console.WriteLine("Database containing statistics exists. Loading data...");
                FakeLoading();
                int error = 0; //Calculates eventually errors
                using (StreamReader file = new StreamReader(statisticsData))
                {
                    int rowsChecked = 0; //Keeps track of how many rows have been checked
                    int amountOfLines = File.ReadLines(statisticsData).Count(); //Calculates amount of lines in the txt-file

                    string line;
                    string[] dataText;

                    for (int Ind = 0; Ind < amountOfLines; Ind++)
                    {
                        line = File.ReadLines(statisticsData).Skip(rowsChecked).Take(1).First(); //Selects the row from txt-file that should be read
                        rowsChecked++;

                        if (line != null)
                        {
                            try
                            {
                                dataText = line.Split(',');
                                double points = double.Parse(dataText[2].ToString().Replace('.', ','));
                                statisticData.Add(new Statistics(int.Parse(dataText[0]), dataText[1], points));
                            }
                            catch
                            {
                                error++;
                            }
                        }
                    }

                    if (error != 0)
                    {
                        Console.WriteLine("There was a problem when reading the data from the database.");
                        Console.WriteLine($"Total errors found: {error}");
                    }
                }
            }
            //If subject database doesn't exists, a new database is created
            else
            {
                using (File.Create(statisticsData)) { };
                Console.WriteLine("No statistics database found. Creating new database...");
                FakeLoading();
            }


            //Checks if students database exist.
            //If it exists, data will be written to corresponding lists
            if (File.Exists(data))
            {
                Console.WriteLine("Database containing students exists. Loading data...");
                int error = 0; //Calculates eventually errors

                //Reads the txt-file
                using (StreamReader file = new StreamReader(data))
                {
                    int rowsChecked = 0; //Keeps track of how many rows have been checked
                    int amountOfLines = File.ReadLines(data).Count(); //Calculates amount of lines in the txt-file

                    string line;
                    string[] dataText;

                    FakeLoading();
                    for (int Ind = 0; Ind < amountOfLines; Ind++)
                    {
                        line = File.ReadLines(data).Skip(rowsChecked).Take(1).First(); //Selects the row from txt-file that should be read
                        rowsChecked++;

                        if (line != null)
                        {
                            try
                            {
                                dataText = line.Split(','); //Takes in data from txt-file and splits it into an array
                                studentData.Add(new Student(int.Parse(dataText[0]), dataText[1], "initiating", subjectData)); //Adds the studentinformation to student-list
                                if (dataText.Length < 6)//Checks lenght of array. If amount of indexes is less than fixe, the student only has one grade registered
                                {
                                    string temp = $"{dataText[2]},{dataText[3]},{dataText[4]}";
                                    string[] tempData = temp.Split(',');
                                    SendGradeData(int.Parse(dataText[0]), tempData);
                                }
                                else//If multiple grades are registered on the student
                                {
                                    string temp = "";
                                    int counter = 0;
                                    int index = 2; //Keeps track of the index-placement in the array
                                    string[] tempData;

                                    for (int i = 0; i < dataText.Length; i++) //Counts amount of strings stored in the array
                                    {
                                        counter++;
                                    }
                                    int amountOfGrades = (counter - 2) / 3; //Calculates how many grades the student has grades in

                                    for (int i = 0; i < amountOfGrades; i++) //Loop, according to how many grades that is registered
                                    {
                                        temp = "";
                                        for (int j = 0; j < 3; j++) //Loops 3 times, which is the amount of information every grade contains
                                        {
                                            temp += $"{dataText[index]},";
                                            index++;
                                        }
                                        tempData = temp.Split(',');
                                        SendGradeData(int.Parse(dataText[0]), tempData); //Sends data to grades-list with the correct information.
                                    }
                                }
                            }
                            catch
                            {
                                error++;
                            }
                        }
                    }

                    if (error != 0)//Notifys user if there was an error loading in the data
                    {
                        Console.WriteLine("There was a problem when reading the data from the database.");
                        Console.WriteLine($"Total errors found: {error}");
                    }
                    FakeLoading();
                }
                UI();
            }

            //If student database doesn't exists, creates a new database
            else
            {
                using (File.Create(data)) { };
                Console.WriteLine("No students database found. Creating new database...");
                FakeLoading();
                Console.Clear();
                UI();
            }
        }


        //Menu interactions
        public void UI() //User UI for menu
        {
            string userinput = "";

            while (userinput != "0")
            {
                PrintMainMenu();

                userinput = Console.ReadLine();
                switch (userinput)
                {
                    case "1": AddStudent(); break;
                    case "2": AddRandomStudent(); break;
                    case "3": PrintStudents(); AwaitUserInput(); break;
                    case "4": SortMenu(); AwaitUserInput(); break;
                    case "5": AddAdditionalGrade(); AwaitUserInput(); break;
                    case "6": DeleteGrade(); AwaitUserInput(); break;
                    case "7": DeleteStudent(); AwaitUserInput(); break;
                    case "8": AddSubject(); AwaitUserInput(); break;
                    case "9": DeleteSubject(); AwaitUserInput(); break;
                    case "10": PrintSubjects(); AwaitUserInput(); break;
                    case "11": AddRandomSubject(); AwaitUserInput(); break;
                    case "12": Console.Clear(); StatisticsMenu(); AwaitUserInput(); break;
                    case "0": break;
                    default: Console.WriteLine("Not a valid option. Try again."); AwaitUserInput(); break;
                }
            }
        }
        private void PrintMainMenu() //Prints menu
        {
            Console.Clear();
            Console.WriteLine(" ---- MENU ---- ");
            Console.WriteLine($"\n - Amount of Students: {studentData.Count} -");
            Console.WriteLine($" - Amount of Subjects: {subjectData.Count} -\n");
            Console.WriteLine("1 - Add new student");
            Console.WriteLine("2 - Add new randomized student");
            Console.WriteLine("3 - Show all students");
            Console.WriteLine("4 - Sort data");
            Console.WriteLine("5 - Add additional grade to student");
            Console.WriteLine("6 - Delete grade from student");
            Console.WriteLine("7 - Delete student profile");
            Console.WriteLine("8 - Add subject");
            Console.WriteLine("9 - Delete subject");
            Console.WriteLine("10 - Show all subjects");
            Console.WriteLine("11 - Add randomized subject");
            Console.WriteLine("12 - Open Statistics");
            Console.WriteLine("0 - Close program");
        }
        public void StatisticsMenu()
        {
            string userinput = "";

            while (userinput != "0")
            {
                PrintStatisticsMenu();

                userinput = Console.ReadLine();
                switch (userinput)
                {
                    case "1": PrintStatistics(); AwaitUserInput(); break;
                    case "2": GetLowest(); AwaitUserInput(); break;
                    case "3": GetHighest(); AwaitUserInput(); break;
                    case "4": CalcAverage(); AwaitUserInput(); break;
                    case "0": break;
                    default: Console.WriteLine("Not a valid option. Try again."); AwaitUserInput(); break;
                }
            }
        }
        private void PrintStatisticsMenu()//Prints statistics menu
        {
            Console.WriteLine(" ---- STATISTICS ---- ");
            Console.WriteLine("1 - Show all statistics");
            Console.WriteLine("2 - Show student with the lowest grade");
            Console.WriteLine("3 - Show student with the highest grade");
            Console.WriteLine("4 - Show the average grades");
            Console.WriteLine("\n0 - Return to menu");
        }
        public void PrintStudents()//Prints the list containing students in console
        {
            Console.Clear();
            foreach (var item in studentData)
            {
                Console.WriteLine(item);
            }
        }
        public void PrintSubjects()//Prints the list containing subjects in console
        {
            Console.Clear();
            foreach (var i in subjectData)
            {
                Console.WriteLine($"-{i}-");
            }
        }
        public void PrintStatistics()//Prints the list containing statistics in console
        {
            foreach (var i in statisticData)
            {
                Console.WriteLine(i);
            }
        }
        public void AwaitUserInput() //Awaits user input before continuing. Clears console
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }



        //Subject interations
        public void AddSubject()//Adds a subject
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
                WriteToSubjectFile();
            }
            else
            {
                Console.WriteLine("Subject already exists.");
            }
        }
        public void AddRandomSubject()//Adds a subject
        {
            Console.Clear();
            Random rng = new Random();
            string addSubject = "";
            bool subjectExists = false;
            bool validID = false;
            string[] subjectNames = new string[] { "Svenska", "Engelska", "Matte", "Naturvetenskap", "Samhälllskunskap", "Fysik", "Biologi" };
            string[] subjectDifficulties = new string[] { "1a", "1b", "1c", "2a", "2b", "2c", "3a", "3b", "3c" };

            int error = 0;

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
                WriteToSubjectFile();
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
        public void DeleteSubject() //Deletes a subject
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
                    AwaitUserInput();
                }
                if (index == -1)
                {
                    validSelection = false;
                    Console.WriteLine("Please choose a valid ID.");
                    AwaitUserInput();
                }
            } while (!validSelection);

            subjectData.RemoveAt(index);
            WriteToSubjectFile();
        }
        public int GetSubjectListLenght()
        {
            return subjectData.Count;
        }


        //Methods for interacting with students
        public void AddStudent() //Function for adding a student
        {
            Random rng = new Random();
            bool validID = false;

            int ID = rng.Next(11111, 99999); //Randomizes userID. 

            //Makes sure that the ID ins't used by another user. 
            if (studentData.Count != 0)
            {
                do
                {
                    for (int i = 0; i < studentData.Count; i++)
                    {

                        if (ID == studentData[i].GetID())
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

            Console.WriteLine("Enter Name"); //Input for Username
            string name = Console.ReadLine();


            studentData.Add(new Student(ID, name, "notRandom", subjectData)); //Adds the data to the List
            int index = studentData.FindIndex(a => a.GetID() == ID);
            string userInput = $"{ID},{name},{string.Join(System.Environment.NewLine, studentData[index].GetGrades())}";

            using (StreamWriter sw = File.AppendText(data)) //Writes the data to database
            {
                sw.WriteLine(userInput);
            }
            Console.WriteLine("Student added. Press any key to continue...");
            Console.ReadKey();
        }
        public void AddRandomStudent() //Function for adding a random student.
        {
            Random rng = new Random();
            string[] names = new string[] { "John Doe", "Ben Dover", "Mah Rajs", "Hanna Svensson" }; //Array containing names that can be randomized
            string[] subjects = new string[] { "Engelska", "Svenska", "Programmering", "Matematik" }; //Array that contains subjects that can be randomized
            bool validID = false;

            int ID = rng.Next(11111, 99999); //Randomizes UserID

            Console.Clear();
            if (subjectData.Count != 0)
            {
                if (studentData.Count != 0) //Checks so that UserID isn't used by another user
                {
                    do
                    {
                        for (int i = 0; i < studentData.Count; i++)
                        {

                            if (ID == studentData[i].GetID())
                            {

                            }
                            else
                            {
                                validID = true;
                            }
                        }
                    } while (!validID);
                }

                string name = names[rng.Next(0, 4)]; //Randomizes name from array

                //Adds data to list
                studentData.Add(new Student(ID, name, "Random", subjectData)); 
                int index = studentData.FindIndex(a => a.GetID() == ID);
                string userInput = $"{ID},{name},{string.Join(System.Environment.NewLine, studentData[index].GetGrades())}";

                using (StreamWriter sw = File.AppendText(data)) //Adds data to datbase
                {
                    sw.WriteLine(userInput);
                }
                Console.Clear();
                Console.WriteLine("Random student added. Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("No subjects exists. Please create a subject before adding a student.");
                Console.ReadKey();
            }
        }
        private void DeleteStudent() //Function for deleting a student
        {
            int ID = 0;
            bool validSelection = false;
            int index = -1;

            do
            {
                Console.WriteLine("Please choose which student you want to remove: ");
                for (int i = 0; i < studentData.Count; i++)
                {
                    Console.WriteLine($"{studentData[i].GetName()}: {studentData[i].GetID()}");
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
                    AwaitUserInput();
                }
                if (index == -1)
                {
                    validSelection = false;
                    Console.WriteLine("Please choose a valid ID.");
                    AwaitUserInput();
                }
            } while (!validSelection);

            studentData.RemoveAt(index);
            WriteToFile();
        }


        //Methods for interacting with grades
        public string ConvertGradesToString(List<string> grades)//Prints grades
        {
            string text = "";
            for (int i = 0; i < grades.Count; i++)
            {
                if (i > 0)
                {
                    text += $",{grades[i]}";
                }
                else
                {
                    text += $"{grades[i]}";
                }
            }
            return text;
        }
        private void SendGradeData(int ID, string[] gradeData) //Sends stored Gradedata to the corresponding list
        {
            int index = studentData.FindIndex(a => a.GetID() == ID);

            studentData[index].AddGradesOnStartUp(int.Parse(gradeData[0]), gradeData[1], gradeData[2]);
        }
        private void AddAdditionalGrade() //Adds more grades to an existing student
        {
            int ID = 0;
            bool validSelection = false;
            int index = -1;

            do
            {
                Console.Clear();
                Console.WriteLine("Please choose which student you want to add another grade to:");
                for (int i = 0; i < studentData.Count; i++)
                {
                    Console.WriteLine($"{studentData[i].GetName()}: {studentData[i].GetID()}");
                }
                try
                {
                    ID = int.Parse(Console.ReadLine());
                    if (ID <= 0)
                    {
                        validSelection = false;
                        Console.Clear();
                        Console.WriteLine("Please choose a valid ID.");
                        AwaitUserInput();
                    }
                    else
                    {
                        for (int i = 0; i < studentData.Count; i++)
                        {
                            if (ID == studentData[i].GetID())
                            {
                                validSelection = true;
                                index = studentData.FindIndex(a => a.GetID() == ID);
                                break;
                            }
                            else
                            {
                                validSelection = false;
                            }
                        }
                    }
                    if (!validSelection)
                    {
                        Console.Clear();
                        Console.WriteLine("Please choose a valid ID.");
                        AwaitUserInput();
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Please choose a valid ID.");
                    AwaitUserInput();
                }
            } while (!validSelection);

            studentData[index].AddGrades(subjectData);
            WriteToFile();
        }
        private void DeleteGrade() //Deletes grade from specific student
        {
            int ID = 0;
            bool validSelection = false;
            int index = -1;

            Console.WriteLine("Please choose which student you want to remove a grade from: ");
            for (int i = 0; i < studentData.Count; i++)
            {
                Console.WriteLine($"{studentData[i].GetName()}: {studentData[i].GetID()}");
            }

            do
            {
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
                    AwaitUserInput();

                }
                if (index == -1)
                {
                    validSelection = false;
                    Console.WriteLine("Please choose a valid ID.");
                    AwaitUserInput();
                }
            } while (!validSelection);

            studentData[index].RemoveGrades();
            WriteToFile();
        }


        //File I/O
        private void ClearFile() //Clears student database
        {
            using (TextWriter tw = new StreamWriter(data, false))
            {
                tw.Write(string.Empty);
            }
        }
        private void ClearSubjectFile() //Clears subject database
        {
            using (TextWriter tw = new StreamWriter(savedSubjects, false))
            {
                tw.Write(string.Empty);
            }
        }
        private void ClearStatisticsFile() //Clears statistics database
        {
            using (TextWriter tw = new StreamWriter(statisticsData, false))
            {
                tw.Write(string.Empty);
            }
        }
        private void WriteToFile() //Writes to student Database
        {
            ClearFile();
            using (StreamWriter sw = File.AppendText(data))
            {
                string convertedGrades;

                for (int i = 0; i < studentData.Count; i++)
                {
                    convertedGrades = ConvertGradesToString(studentData[i].GetGrades());
                    string userInput = $"{studentData[i].GetID()},{studentData[i].GetName()},{convertedGrades}";
                    sw.WriteLine(userInput);
                }
            }
        }
        private void WriteToSubjectFile() //Writes to subject Database
        {
            ClearSubjectFile();
            using (StreamWriter sw = File.AppendText(savedSubjects))
            {
                for (int i = 0; i < subjectData.Count; i++)
                {
                    string userInput = $"{subjectData[i].GetSubjectID()},{subjectData[i].GetSubjectName()}";
                    sw.WriteLine(userInput);
                }
            }
        }
        private void WriteToStatisticsFile() //Writes to student Database
        {
            ClearStatisticsFile();
            using (StreamWriter sw = File.AppendText(statisticsData))
            {
                for (int i = 0; i < statisticData.Count; i++)
                {
                    string points = string.Join(System.Environment.NewLine, statisticData[i].GetPoints());
                    string userInput = $"{statisticData[i].GetStatisticsID()},{statisticData[i].GetStatisticsType()},{points.Replace(',', '.')}";
                    sw.WriteLine(userInput);
                }
            }
        }


        //Sorting mechanisms
        public void SortMenu() //UI that lets user choose which data to sort
        {
            string userinput = "";
            bool validSelection = false;

            while (!validSelection) //Checks which data user wants to sort after
            {
                Console.Clear();
                Console.WriteLine("Which data do you want to sort?");
                Console.WriteLine("1 - Student data");
                Console.WriteLine("2 - Grades data");

                userinput = Console.ReadLine();

                switch (userinput)
                {
                    case "1": SortStudentMenu(); validSelection = true; break;
                    case "2": SortGradesMenu(); validSelection = true; break;
                    default: Console.Clear(); Console.WriteLine("Please select a valid option"); validSelection = false; break;
                }
            }
            Console.Clear();
        }
        public void SortStudentMenu() //Menu that let's user choose how they want to sort the data
        {
            string userinput = "";
            bool validSelection = false;

            while (!validSelection) //Checks which data user wants to sort after
            {
                Console.Clear();
                Console.WriteLine("How do you want to sort the data?");
                Console.WriteLine("1 - By ID");
                Console.WriteLine("2 - By name");

                userinput = Console.ReadLine();

                switch (userinput)
                {
                    case "1": studentData = SortStudent(studentData, "ID"); validSelection = true; break;
                    case "2": studentData = SortStudent(studentData, "Name"); validSelection = true; break;
                    default: Console.Clear(); Console.WriteLine("Please select a valid option"); validSelection = false; break;
                }
            }
            Console.Clear();
            WriteToFile();
        }
        public void SortGradesMenu() //Menu that let's user choose how they want to sort the data
        {
            string userinput = "";
            bool validSelection = false;

            while (!validSelection) //Checks which data user wants to sort after
            {
                Console.Clear();
                Console.WriteLine("How do you want to sort the data?");
                Console.WriteLine("1 - By subject");
                Console.WriteLine("2 - By grades");

                userinput = Console.ReadLine();

                switch (userinput)
                {
                    case "1": studentData = SortGrades(studentData, "Subject"); validSelection = true; break;
                    case "2": studentData = SortGrades(studentData, "Grade"); validSelection = true; break;
                    default: Console.Clear(); Console.WriteLine("Please select a valid option"); validSelection = false; break;
                }
            }
            Console.Clear();
            WriteToFile();
        }
        public List<Student> SortStudent(List<Student> _studentData, string getInfo) //Sorts the list after choosen type
        {
            int i, j;

            switch (getInfo)
            {
                case "ID": //Sorts list after ID using insertion sort
                    for (i = 1; i < _studentData.Count; i++)
                    {
                        for (j = i; j > 0; j--)
                        {
                            if (_studentData[j].GetID() < _studentData[j - 1].GetID())
                            {
                                var tmp = _studentData[j - 1];
                                _studentData[j - 1] = _studentData[j];
                                _studentData[j] = tmp;
                            }
                        }
                    }
                    break;
                case "Name": //Sorts list after names
                    _studentData.Sort((x, y) => string.Compare(x.GetName(), y.GetName()));
                    break;
            }
            return _studentData;
        }
        public List<Student> SortGrades(List<Student> _studentData, string getInfo)
        {
            int i;
            string userInput = "";
            bool validSelection = false;

            switch (getInfo)
            {
                case "Subject": //Sorts list after subjects. 
                    for (i = 0; i < studentData.Count; i++)
                    {
                        studentData[i].SortGradesBySubject();
                    }
                    break;
                case "Grade": //Sorts list after grades using InsertionSort. User can choose if the order should ascend/descend
                    for (i = 0; i < studentData.Count; i++)
                    {
                        studentData[i].SortGradesByGrade();
                    }

                    do //Checks which order user wants the grades sorted
                    {
                        Console.Clear();
                        Console.WriteLine("Should the order be ascending or descending?");
                        Console.WriteLine("1 - Ascending");
                        Console.WriteLine("2 - Descending");
                        switch (userInput = Console.ReadLine())
                        {
                            case "1": validSelection = true; break;
                            case "2": validSelection = true; break;
                            default: Console.WriteLine("Please choose a valid option."); validSelection = false; break;
                        }
                    } while (!validSelection);
                    if (userInput == "2")
                    {
                        for (i = 0; i < studentData.Count; i++)
                        {
                            studentData[i].ReverseSortedGrades();
                        }
                    }
                    break;
            }
            return studentData;
        }


        //Interacting with statistics

        //Values used by methods
        private readonly double dash = 0, F = 0, E = 10, D = 12.5, C = 15, B = 17.5, A = 20; //Grade values
        private double dashCount = 0, fCount = 0, eCount = 0, dCount = 0, cCount = 0, bCount = 0, aCount = 0; //Counts how many grades of each type
        private void GetLowest()
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
        private void GetHighest()
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
        private void CalcAverage()
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
        private void CounterReset()
        {
            dashCount = 0;
            fCount = 0;
            eCount = 0;
            dCount = 0;
            cCount = 0;
            bCount = 0;
            aCount = 0;
        }


        //Just for fun
        private void FakeLoading()
        {
            for (int i = 0; i < 20; i++)
            {
                Console.Write('.');
                Thread.Sleep(100);
            }
            Console.WriteLine();
        }

        /* YET TO IMPLEMENT:
         * Search-function
         */
    }
}