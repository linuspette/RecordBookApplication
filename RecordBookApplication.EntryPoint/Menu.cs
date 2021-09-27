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
        string usersData = "users.txt";


        List<Student> studentData = new List<Student>();
        public List<Subjects> subjectData = new List<Subjects>();
        //List that stores statistical data
        private List<Statistics> statisticData = new List<Statistics>();

        public void Run() //Initiates program and loads data from databases
        {
            Console.WriteLine("Looking for existing databases...");
            FakeLoading();

            //Checks if users database exist.
            if (File.Exists(usersData))
            {
                Console.WriteLine("Database containing users exists. Loading data...");
                FakeLoading();
                int error = 0; //Calculates eventually errors
                using (StreamReader file = new StreamReader(usersData))
                {
                    int amountOfLines = File.ReadLines(usersData).Count(); //Calculates amount of lines in the txt-file

                    if (amountOfLines == 0)
                    {
                        file.Close();
                        Console.WriteLine("There's no users in database.");
                        CreateAdmin("Please create your first admin account");
                        Console.WriteLine("\nResuming to load data");
                        FakeLoading();
                    }

                    if (error != 0)
                    {
                        Console.WriteLine("There was a problem when reading the data from the database.");
                        Console.WriteLine($"Total errors found: {error}");
                    }
                }
            }
            //If users database doesn't exists,
            //a new database is created amd user will be prompted to create a new administrator
            else
            {
                using (File.Create(usersData)) { };
                Console.WriteLine("No users database found. Creating new database...");
                CreateAdmin("No administrator has been registered please do so now:");
                Console.WriteLine("\nResuming to load data");
                FakeLoading();
            }

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
                    Console.Clear();
                    Console.WriteLine("Initializing");
                    FakeLoading();

                }
                MainMenu();
            }

            //If student database doesn't exists, creates a new database
            else
            {
                using (File.Create(data)) { };
                Console.WriteLine("No students database found. Creating new database...");
                FakeLoading();
                Console.Clear();
                MainMenu();
            }
        }


        //Menu interactions
        public void MainMenu() //User UI for menu
        {
            string userinput = "";

            while (userinput != "0")
            {
                PrintMainMenu();

                userinput = Console.ReadLine();
                switch (userinput)
                {
                    case "1": StudentsMenu(); break;
                    case "2": SubjectsMenu(); break;
                    case "3": StatisticsMenu(); break;
                    case "4": SortMenu(); break;
                    case "5": AdminMenu(); break;
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
            Console.WriteLine($" - Amount of Subjects: {subjectData.Count} -\n\n");
            Console.WriteLine("1 - Run Students Manager");
            Console.WriteLine("2 - Subjects Manager");
            Console.WriteLine("3 - Open Statistics");
            Console.WriteLine("4 - Sort data");
            Console.WriteLine("5 - Administrate");
            Console.WriteLine("\n0 - Close program\n");
        }


        public void StudentsMenu()//User UI for stundents
        {
            string userinput = "";

            while (userinput != "0")
            {
                PrintStudentMenu();

                userinput = Console.ReadLine();

                switch (userinput)
                {
                    case "1": AddStudent(); break;
                    case "2": PrintStudents(); AwaitUserInput(); break;
                    case "3": AddAdditionalGrade(); AwaitUserInput(); break;
                    case "4": DeleteGrade(); AwaitUserInput(); break;
                    case "5": DeleteStudent(); AwaitUserInput(); break;
                    case "0": break;
                    default: Console.WriteLine("Not a valid option. Try again."); AwaitUserInput(); break;
                }
            }
        }
        public void PrintStudentMenu()
        {
            Console.Clear();
            Console.WriteLine(" ---- STUDENTS MANAGER ---- \n");
            Console.WriteLine($"\n - Amount of Students: {studentData.Count} -\n");
            Console.WriteLine("1 - Add new student");
            Console.WriteLine("2 - Show all students");
            Console.WriteLine("3 - Add additional grade to student");
            Console.WriteLine("4 - Delete grade from student");
            Console.WriteLine("5 - Delete student profile");
            Console.WriteLine("\n0 - Return to main menu\n");

        }
        public void PrintStudents()//Prints the list containing students in console
        {
            Console.Clear();
            foreach (var item in studentData)
            {
                Console.WriteLine(item);
            }
        }


        public void SubjectsMenu()//User UI for subjects
        {
            string userinput = "";

            while (userinput != "0")
            {
                PrintSubjectsMenu();

                userinput = Console.ReadLine();

                switch (userinput)
                {
                    case "1": AddSubject(); AwaitUserInput(); break;
                    case "2": DeleteSubject(); AwaitUserInput(); break;
                    case "3": PrintSubjects(); AwaitUserInput(); break;
                    case "0": break;
                    default: Console.WriteLine("Not a valid option. Try again."); AwaitUserInput(); break;
                }
            }
        }
        public void PrintSubjectsMenu()
        {
            Console.Clear();
            Console.WriteLine(" ---- SUBJECTS MANAGER ---- \n");
            Console.WriteLine($"\n - Amount of Subjects: {subjectData.Count} -\n");
            Console.WriteLine("1 - Add subject");
            Console.WriteLine("2 - Delete subject");
            Console.WriteLine("3 - Show all subjects");
            Console.WriteLine("\n0 - Return to main menu\n");
        }
        public void PrintSubjects()//Prints the list containing subjects in console
        {
            Console.Clear();
            foreach (var i in subjectData)
            {
                Console.WriteLine($"-{i}-");
            }
        }


        public void StatisticsMenu()//User UI for statistics
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
            Console.Clear();
            Console.WriteLine(" ---- STATISTICS ---- \n");
            Console.WriteLine("1 - Show all statistics");
            Console.WriteLine("2 - Show student with the lowest grade");
            Console.WriteLine("3 - Show student with the highest grade");
            Console.WriteLine("4 - Show the average grades");
            Console.WriteLine("\n0 - Return to main menu\n");
        }
        public void PrintStatistics()//Prints the list containing statistics in console
        {
            foreach (var i in statisticData)
            {
                Console.WriteLine(i);
            }
        }


        public void SortMenu() //UI that lets user choose which data to sort
        {
            string userinput = "";
            bool validSelection = false;

            while (!validSelection && userinput != "0") //Checks which data user wants to sort after
            {
                PrintSortMenu();
                userinput = Console.ReadLine();

                switch (userinput)
                {
                    case "1": SortStudentMenu(); validSelection = true; break;
                    case "2": SortGradesMenu(); validSelection = true; break;
                    case "0": break;
                    default: Console.Clear(); Console.WriteLine("Please select a valid option"); validSelection = false; break;
                }
            }
            Console.Clear();
        }
        public void PrintSortMenu()
        {
            Console.Clear();
            Console.WriteLine(" ---- SORTING ----- \n");
            Console.WriteLine("1 - Student data");
            Console.WriteLine("2 - Grades data");
            Console.WriteLine("\n0 - Return to main menu\n");
        }


        private void AdminMenu()
        {
            string userinput = "";
            bool credentialsAccepted = false;
            bool validSelection = false;

            while (!credentialsAccepted && userinput != "n")
            {
                string usernameInput = string.Empty;
                var passwordInput = string.Empty;


                Console.Clear();
                Console.WriteLine("Please enter administrator credentials:");
                Console.Write("Username: ");
                usernameInput = Console.ReadLine();
                Console.Write("Password: ");
                passwordInput = MaskedReadLine();
                Console.WriteLine();

                credentialsAccepted = ValidateCredentials(usernameInput, passwordInput);

                //If credentials are accepted, Admin panel will execute
                if (credentialsAccepted == true)
                {
                    Console.Clear();
                    Console.WriteLine("\nCredentials accepted");
                    Thread.Sleep(1500);
                    Console.WriteLine("\n ----WARNING---- ");
                    Console.WriteLine("The admin panel should only be used by certified staff" +
                        "\nas it can inflict strange behaviours and data losses.");
                    Thread.Sleep(3000);

                    while (userinput != "0")
                    {
                        PrintAdminMenu();

                        userinput = Console.ReadLine();

                        switch (userinput)
                        {
                            case "1": AddRandomStudent(); break;
                            case "2": AddRandomSubject(); break;
                            case "3": DeleteAllStudentData(); break;
                            case "4": DeleteAllSubjectData(); break;
                            case "5": DeleteAllStatisticsData(); break;
                            case "0": break;
                            default: Console.WriteLine("Not a valid option. Try again."); AwaitUserInput(); break;
                        }
                    }
                }
                //If credentials are invalid, asks user to try again, or exit to menu
                else
                {
                    Console.Clear();
                    Console.WriteLine("The credentials that you have entered is invalid.");
                    do
                    {
                        Console.WriteLine("Do you want to try again? y/n");
                        userinput = Console.ReadLine();
                        switch (userinput)
                        {
                            case "y": validSelection = true; Console.Clear(); break;
                            case "n": validSelection = true; Console.Clear(); break;
                            default: Console.Clear(); Console.WriteLine("Please enter a valid option."); validSelection = false; break;
                        }
                    } while (!validSelection);
                }
            }
        }
        private void PrintAdminMenu()
        {
            Console.Clear();
            Console.WriteLine(" ---- ADMIN PANEL ---- \n");
            Console.WriteLine($"\n - Amount of Students: {studentData.Count} -");
            Console.WriteLine($" - Amount of Subjects: {subjectData.Count} -\n");
            Console.WriteLine("1 - Add random student");
            Console.WriteLine("2 - Add random subject");
            Console.WriteLine("3 - Delete all student data");
            Console.WriteLine("4 - Delete all subject data");
            Console.WriteLine("5 - Delete all statistics data");
            Console.WriteLine("\n0 - Return to main menu\n");
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

            Console.Write($"Creating student");
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


        //Admin interactions
        private void DeleteAllStudentData()//Deletes all student data both from database and list
        {
            bool validSelection = false;
            do
            {
                Console.WriteLine("Are you sure you want to proceed?\nThe data will be lost forever. y/n");
                string userinput = Console.ReadLine();
                switch (userinput)
                {
                    case "y": validSelection = true; subjectData.Clear(); ClearFile(); Console.Clear(); break;
                    case "n": validSelection = true; Console.Clear(); break;
                    default: Console.Clear(); Console.WriteLine("Please enter a valid option."); validSelection = false; break;
                }
            } while (!validSelection);           
        }
        private void DeleteAllSubjectData()//Deletes all subject data both from database and list
        {
            bool validSelection = false;
            do
            {
                Console.WriteLine("Are you sure you want to proceed?\nThe data will be lost forever. y/n");
                string userinput = Console.ReadLine();
                switch (userinput)
                {
                    case "y": validSelection = true; studentData.Clear(); ClearSubjectFile(); Console.Clear(); break;
                    case "n": validSelection = true; Console.Clear(); break;
                    default: Console.Clear(); Console.WriteLine("Please enter a valid option."); validSelection = false; break;
                }
            } while (!validSelection);           
        }
        private void DeleteAllStatisticsData()//Deletes all subject data both from database and list
        {
            bool validSelection = false;
            do
            {
                Console.WriteLine("Are you sure you want to proceed?\nThe data will be lost forever. y/n");
                string userinput = Console.ReadLine();
                switch (userinput)
                {
                    case "y": validSelection = true; statisticData.Clear(); ClearStatisticsFile(); Console.Clear(); break;
                    case "n": validSelection = true; Console.Clear(); break;
                    default: Console.Clear(); Console.WriteLine("Please enter a valid option."); validSelection = false; break;
                }
            } while (!validSelection);     
        }
        private bool ValidateCredentials(string username, string password) //Validates user input credentials
        {
            string _username = string.Empty;
            string _password = string.Empty;

            bool validation = false;

            if (File.Exists(savedSubjects))
            {
                Console.WriteLine("Checking Credentials");
                FakeLoading();
                int error = 0; //Calculates eventually errors
                using (StreamReader file = new StreamReader(usersData))
                {
                    int rowsChecked = 0; //Keeps track of how many rows have been checked
                    int amountOfLines = File.ReadLines(usersData).Count(); //Calculates amount of lines in the txt-file

                    string line;
                    string[] dataText;

                    //Checks if there's any users saved in database
                    if (amountOfLines == 0)
                    {
                        Console.WriteLine("There's no users in database.");
                        Console.WriteLine("Please restart the application to resolve the issue");
                    }
                    else //Validates credentials
                    {
                        for (int Ind = 0; Ind < amountOfLines; Ind++)
                        {
                            line = File.ReadLines(usersData).Skip(rowsChecked).Take(1).First(); //Selects the row from txt-file that should be read
                            rowsChecked++;

                            if (line != null)
                            {
                                try
                                {
                                    dataText = line.Split(',');
                                    _username = dataText[1];
                                    _password = Encryption.Decrypt(dataText[2]);
                                }
                                catch
                                {
                                    error++;
                                }
                            }

                            if (username == _username && password == _password)
                            {
                                validation = true;
                                break;
                            }
                            else
                            {
                                validation = false;
                            }
                        }
                        if (error != 0)
                        {
                            Console.WriteLine("There was a problem when reading the data from the database.");
                            Console.WriteLine($"Total errors found: {error}");
                        }
                    }
                }
            }

            return validation;
        }
        private string MaskedReadLine()
        {
            var password = string.Empty;
            ConsoleKey key;
            do
            {

                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password = password[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    password += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            return password;
        }
        private void CreateAdmin(string header)
        {
            string userinput = string.Empty;
            string username = string.Empty;
            bool validInput = false;

            int ID = 0;

            Random rng = new Random();

            Console.Clear();
            Console.WriteLine(header);

            //Gives the user a unique ID
            using (StreamReader file = new StreamReader(usersData))
            {
                int rowsChecked = 0; //Keeps track of how many rows have been checked
                int amountOfLines = File.ReadLines(usersData).Count(); //Calculates amount of lines in the txt-file

                string line;
                string[] dataText;
                bool validID = false;

                //Checks the database if the ID is already taken
                while (!validID)
                {
                    ID = rng.Next(11111, 99999);
                    if (amountOfLines > 0)
                    {
                        for (int i = 0; i < amountOfLines; i++)
                        {
                            line = File.ReadLines(usersData).Skip(rowsChecked).Take(1).First(); //Selects the row from txt-file that should be read
                            rowsChecked++;

                            if (line != null)
                            {
                                try
                                {
                                    dataText = line.Split(',');
                                    if (ID != int.Parse(dataText[0]))
                                    {
                                        validID = true;
                                        break;
                                    }
                                    else
                                    {
                                        validID = false;
                                    }

                                }
                                catch
                                {
                                    validID = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        validID = true;
                    }
                }
            }

            //Lets user choose username
            while (userinput != "y")
            {
                Console.WriteLine("Please enter a username:");
                username = Console.ReadLine();

                do
                {
                    Console.WriteLine($"Do you want the username to be \"{username}\"? y/n");
                    userinput = Console.ReadLine().ToLower();
                    switch (userinput)
                    {
                        case "y": validInput = true; break;
                        case "n": validInput = true; break;
                        default: Console.Clear(); Console.WriteLine("Invalid input. Try again."); validInput = false; break;
                    }
                } while (!validInput);
            }

            //Lets user choose password
            string password1 = string.Empty;
            string password2 = string.Empty;

            do
            {
                Console.Clear();
                Console.Write("Please type in a password: ");
                password1 = MaskedReadLine();
                Console.Write("\nPlease enter your password again: ");
                password2 = MaskedReadLine();
                Console.WriteLine("\n");

                //If passwords doesn't match
                if (password1 != password2)
                {
                    do
                    {
                        Console.WriteLine("The passwords didn't match. " +
                            "\nDo you want to try again? y/n");
                        userinput = Console.ReadLine().ToLower();
                        switch (userinput)
                        {
                            case "y": validInput = true; break;
                            case "n": validInput = true; break;
                            default: Console.Clear(); Console.WriteLine("Invalid input. Try again."); validInput = false; break;
                        }
                    } while (!validInput);
                }
                //If passwords match - Writes to database
                else if (password1 == password2)
                {
                    Console.WriteLine("User has been created.");
                    Console.WriteLine();
                    using (StreamWriter sw = File.AppendText(usersData))
                    {
                            string userInput = $"{ID},{username},{Encryption.Encrypt(password1)}";
                            sw.WriteLine(userInput);
                    }
                }

            } while (password1 != password2 && userinput != "n");
        }
        private void ConfigureAdmins()
        {

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
                    if (userInput == "1")
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
                Thread.Sleep(75);
            }
            Console.WriteLine();
        }

        /* YET TO IMPLEMENT:
         * Search-function
         */
    }
}