using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Text;
using static RecordBookApplication.EntryPoint.Classes;


namespace RecordBookApplication.EntryPoint
{
    public class Menu
    {
        public static string[] databases { get; private set; } = new string[] {"subjectDatabase.xml",
                                                                               "studentDatabase.xml",
                                                                               "statisticsDatabase.xml",
                                                                               "classesDatabase.xml",
                                                                               "usersDatabase.xml"};
        public static readonly string subjectsDatabase = databases[0];
        public static readonly string studentDatabase = databases[1];
        public static readonly string statisticsDatabase = databases[2];
        public static readonly string classesDatabase = databases[3];
        public static readonly string usersDatabase = databases[4];

        private static bool userIsAdmin { get; set; }
        bool userIsAuthenticated = false;

        //List that stores logged in users access codes
        public static List<string> userAccessCodes { get; private set; } = new List<string>();
        //List that stores class data
        public static List<Classes> classData { get; set; } = new List<Classes>();
        
        //List that stores subject data
        public static List<Subjects> subjectData { get; set; } = new List<Subjects>();        
        
        //List that stores statistical data
        public static List<Statistics> statisticData { get; set; } = new List<Statistics>();

        
        public void LogIn() //User log in
        {
            string userinput = "";
            bool credentialsAccepted = false;
            bool validSelection = false;
            bool anotherLogIn = true;

            FileEncryption.DeCryptUsers();
            //Checks if users database exist.
            if (File.Exists(usersDatabase))
            {
                int error = 0; //Calculates eventually errors
                using (StreamReader file = new StreamReader(usersDatabase))
                {
                    int amountOfLines = File.ReadLines(usersDatabase).Count(); //Calculates amount of lines in the txt-file

                    if (amountOfLines == 0)
                    {
                        file.Close();
                        Console.WriteLine("There's no users in database.");
                        CreateUser("Please create your first admin account");
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
                using (File.Create(usersDatabase)) { };
                Console.WriteLine("No users database found. Creating new database...");
                CreateUser("No administrator has been registered please do so now:");
                FakeLoading();
            }

            do
            {
                //Decrypts file if its encrypted
                if (File.Exists($"{usersDatabase}.aes"))
                {
                    FileEncryption.DeCryptUsers();
                }

                while (!credentialsAccepted && userinput != "n" && anotherLogIn == true)
                {
                    string usernameInput = string.Empty;
                    var passwordInput = string.Empty;
                    userIsAdmin = false;


                    Console.Clear();
                    Console.WriteLine(" -- Type \"0\" if you want to close the application --  ");
                    Console.WriteLine("\tPlease enter your credentials:");
                    Console.Write("\tUsername: ");
                    usernameInput = Console.ReadLine();

                    if (usernameInput != "0")
                    {

                        Console.Write("\tPassword: ");
                        passwordInput = MaskedReadLine();
                        Console.WriteLine();

                        credentialsAccepted = ValidateCredentials(usernameInput, passwordInput);

                        //If credentials are accepted, Admin panel will execute
                        if (credentialsAccepted == true)
                        {
                            Run();
                            ClearLists();

                            do
                            {
                                Console.Clear();
                                Console.WriteLine("Do you want to log in to another account? y/n");
                                userinput = Console.ReadLine();
                                switch (userinput)
                                {
                                    case "y": validSelection = true; credentialsAccepted = false; Console.Clear(); anotherLogIn = true; break;
                                    case "n": validSelection = true; Console.Clear(); anotherLogIn = false; break;
                                    default: Console.Clear(); Console.WriteLine("Please enter a valid option."); validSelection = false; break;
                                }
                            } while (!validSelection);
                        }
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
                                    case "n": validSelection = true; credentialsAccepted = false; anotherLogIn = false; Console.Clear(); break;
                                    default: Console.Clear(); Console.WriteLine("Please enter a valid option."); validSelection = false; break;
                                }
                            } while (!validSelection);
                        }
                    }
                    else
                    {
                        Console.Clear();
                        FileEncryption.EncryptUsers();
                        anotherLogIn = false;
                    }
                    
                }

            } while (anotherLogIn);
        }

        private void Run() //Initiates program and loads data from databases
        {
            if (userIsAuthenticated)
            {
                Console.Clear();
                Console.WriteLine(" --USER AUTHENTICATED-- \n");
                Thread.Sleep(1500);
                Console.WriteLine("Looking for existing databases...");
                FakeLoading();
                FileEncryption.DeCryptAllFiles();

                //Checks if subjects database exist.
                //If it exists, data will be written to corresponding lists
                if (File.Exists(subjectsDatabase))
                {
                    Console.WriteLine("Database containing subjects exists. Loading data...");
                    FakeLoading();
                    int error = 0; //Calculates eventually errors
                    using (StreamReader file = new StreamReader(subjectsDatabase))
                    {
                        int rowsChecked = 0; //Keeps track of how many rows have been checked
                        int amountOfLines = File.ReadLines(subjectsDatabase).Count(); //Calculates amount of lines in the txt-file

                        string line;
                        string[] dataText;

                        for (int Ind = 0; Ind < amountOfLines; Ind++)
                        {
                            line = File.ReadLines(subjectsDatabase).Skip(rowsChecked).Take(1).First(); //Selects the row from txt-file that should be read
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
                    using (File.Create(subjectsDatabase)) { };
                    Console.WriteLine("No subjects database found. Creating new database...");
                    FakeLoading();
                }


                //Checks if statistics database exist.
                //If it exists, data will be written to corresponding lists
                if (File.Exists(statisticsDatabase))
                {
                    Console.WriteLine("Database containing statistics exists. Loading data...");
                    FakeLoading();
                    int error = 0; //Calculates eventually errors
                    using (StreamReader file = new StreamReader(statisticsDatabase))
                    {
                        int rowsChecked = 0; //Keeps track of how many rows have been checked
                        int amountOfLines = File.ReadLines(statisticsDatabase).Count(); //Calculates amount of lines in the txt-file

                        string line;
                        string[] dataText;

                        for (int Ind = 0; Ind < amountOfLines; Ind++)
                        {
                            line = File.ReadLines(statisticsDatabase).Skip(rowsChecked).Take(1).First(); //Selects the row from txt-file that should be read
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
                    using (File.Create(statisticsDatabase)) { };
                    Console.WriteLine("No statistics database found. Creating new database...");
                    FakeLoading();
                }

                //Checks if classes database exists
                //If it exists, data will be written to corresponding lists
                if (File.Exists(classesDatabase))
                {
                    Console.WriteLine("Database containing subjects exists. Loading data...");
                    FakeLoading();
                    int error = 0; //Calculates eventually errors
                    using (StreamReader file = new StreamReader(classesDatabase))
                    {
                        int rowsChecked = 0; //Keeps track of how many rows have been checked
                        int amountOfLines = File.ReadLines(classesDatabase).Count(); //Calculates amount of lines in the txt-file

                        string line;
                        string[] dataText;

                        for (int Ind = 0; Ind < amountOfLines; Ind++)
                        {
                            line = File.ReadLines(classesDatabase).Skip(rowsChecked).Take(1).First(); //Selects the row from txt-file that should be read
                            rowsChecked++;

                            if (line != null)
                            {
                                try
                                {
                                    dataText = line.Split(',');
                                    classData.Add(new Classes(int.Parse(dataText[0]), dataText[1], dataText[2]));
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
                //If classes database doesn't exists, a new database is created
                else
                {
                    using (File.Create(classesDatabase)) { };
                    Console.WriteLine("No subjects database found. Creating new database...");
                    FakeLoading();
                }


                //Checks if students database exist.
                //If it exists, data will be written to corresponding lists
                if (File.Exists(studentDatabase))

                {
                    Console.WriteLine("Database containing students exists. Loading data...");
                    int error = 0; //Calculates eventually errors

                    //Reads the txt-file
                    using (StreamReader file = new StreamReader(studentDatabase))
                    {
                        int rowsChecked = 0; //Keeps track of how many rows have been checked
                        int amountOfLines = File.ReadLines(studentDatabase).Count(); //Calculates amount of lines in the txt-file

                        string line;
                        string[] dataText;

                        FakeLoading();
                        for (int Ind = 0; Ind < amountOfLines; Ind++)
                        {
                            line = File.ReadLines(studentDatabase).Skip(rowsChecked).Take(1).First(); //Selects the row from txt-file that should be read
                            rowsChecked++;

                            if (line != null)
                            {
                                try
                                {
                                    dataText = line.Split(','); //Takes in data from txt-file and splits it into an array
                                    for (int i = 0; i < classData.Count; i++)
                                    {
                                        if (dataText[2] == classData[i].className)
                                        {
                                            classData[i].AddStudentData(int.Parse(dataText[0]), dataText[1], dataText[2], "initiating"); //Adds the studentinformation to student-list
                                            if (dataText.Length < 7)//Checks lenght of array. If amount of indexes is less than fixe, the student only has one grade registered
                                            {
                                                string temp = $"{dataText[3]},{dataText[4]},{dataText[5]}";
                                                string[] tempData = temp.Split(',');
                                                SendGradeData(int.Parse(dataText[0]), tempData);
                                            }
                                            else//If multiple grades are registered on the student
                                            {
                                                string temp = "";
                                                int counter = 0;
                                                int index = 2; //Keeps track of the index-placement in the array
                                                string[] tempData;

                                                for (int j = 0; j < dataText.Length; j++) //Counts amount of strings stored in the array
                                                {
                                                    counter++;
                                                }
                                                int amountOfGrades = (counter - 2) / 3; //Calculates how many grades the student has grades in

                                                for (int j = 0; j < amountOfGrades; j++) //Loop, according to how many grades that is registered
                                                {
                                                    temp = "";
                                                    for (int k = 0; k < 3; k++) //Loops 3 times, which is the amount of information every grade contains
                                                    {
                                                        temp += $"{dataText[index]},";
                                                        index++;
                                                    }
                                                    tempData = temp.Split(',');
                                                    SendGradeData(int.Parse(dataText[0]), tempData); //Sends data to grades-list with the correct information.
                                                }
                                            }

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
                }
                //If student database doesn't exists, creates a new database
                else
                {
                    using (File.Create(studentDatabase)) { };
                    Console.WriteLine("No students database found. Creating new database...");
                    FakeLoading();
                    Console.Clear();
                }

                //Starts menu associated to the user
                if (userIsAdmin)
                {
                    AdminMenu();
                }
                else
                {
                    MainMenu();
                }
            }
            //If user fails to authenticate
            else
            {
                Console.WriteLine("Please contact an administrator");
                Console.WriteLine("The program will now shut down.");
                FakeLoading();
            }
        }

        //Menu interactions
        private void MainMenu() //User UI for menu
        {
            string userinput = "";
            Console.Clear();

            while (userinput != "0")
            {
                PrintMainMenu();

                userinput = Console.ReadLine();
                switch (userinput)
                {
                    case "1": ClassesManager.ClassesManagerMenu(); break;
                    case "2": StudentsManager.StudentsMenu(); break;
                    case "3": SubjectsManager.SubjectsMenu(); break;
                    case "4": StatisticsMechanisms.StatisticsMenu(); break;
                    case "5": SortingMechanisms.SortMenu(); break;
                    case "0": FileEncryption.EncryptAllFiles(); break; //Encrypts all files before user logs out
                    default: Console.WriteLine("Not a valid option. Try again."); AwaitUserInput(); break;
                }
            }
        }
        private void PrintMainMenu() //Prints menu
        {
            Console.Clear();
            Console.WriteLine(" ---- MENU ---- ");
            Console.WriteLine($"\n - Amount of classes: {classData.Count} -\n");
            Console.WriteLine($" - Amount of Students: {studentData.Count} -");
            Console.WriteLine($" - Amount of Subjects: {subjectData.Count} -\n\n");
            Console.WriteLine("1 - Run Students Manager");
            Console.WriteLine("2 - Subjects Manager");
            Console.WriteLine("3 - Open Statistics");
            Console.WriteLine("4 - Sort data");
            Console.WriteLine("\n0 - Log out\n");
        }

        private void AdminMenu()
        {
            string userInput = string.Empty;

            Console.Clear();

            while (userInput != "0")
            {
                PrintAdminMenu();

                userInput = Console.ReadLine();


                switch (userInput)
                {
                    case "1": ClassesManager.ClassesManagerMenu(); break;
                    case "2": StudentsManager.StudentsMenu(); break;
                    case "3": SubjectsManager.SubjectsMenu(); break;
                    case "4": StatisticsMechanisms.StatisticsMenu(); break;
                    case "5": SortingMechanisms.SortMenu(); break;
                    case "6": ClassesManager.AddRandomClass(); break;
                    case "7": StudentsManager.AddRandomStudent(); break;
                    case "8": SubjectsManager.AddRandomSubject(); break;
                    case "9": CreateUser("Create a new user:"); break;
                    case "10": DeleteAllStudentData(); break;
                    case "11": DeleteAllSubjectData(); break;
                    case "12": DeleteAllStatisticsData(); break;
                    case "13": FileEncryption.EncryptionMenu(); break;
                    case "14": SearchEngine.SearchMenu(studentData); break;
                    case "15": AddAccesToUser(); break;
                    case "0": FileEncryption.EncryptAllFiles(); break; //Encrypts all files before user logs out
                    default: Console.WriteLine("Not a valid option. Try again."); AwaitUserInput(); break;
                }
            }
        }
        private void PrintAdminMenu()
        {
            Console.Clear();
            Console.WriteLine(" ---- ADMIN PANEL ---- \n");
            Console.WriteLine($"\n - Amount of classes: {classData.Count} -\n");
            Console.WriteLine($" - Amount of Students: {studentData.Count} -");
            Console.WriteLine($" - Amount of Subjects: {subjectData.Count} -\n");
            Console.WriteLine("1 - Run Classes Manager");
            Console.WriteLine("2 - Run Students Manager");
            Console.WriteLine("3 - Run Subjects Manager");
            Console.WriteLine("4 - Open Statistics");
            Console.WriteLine("5 - Sort data");
            Console.WriteLine("6 - Add random class");
            Console.WriteLine("7 - Add random student");
            Console.WriteLine("8 - Add random subject");
            Console.WriteLine("9 - Create new user");
            Console.WriteLine("10 - Delete all student data");
            Console.WriteLine("11 - Delete all subject data");
            Console.WriteLine("12 - Delete all statistics data");
            Console.WriteLine("13 - Encrypt database");
            Console.WriteLine("14 - Decrypt database");
            Console.WriteLine("15 - Run Search Engine");
            Console.WriteLine("\n0 - Log out\n");
        }


        //Methods for interacting with students
        public static string ConvertGradesToString(List<string> grades)//Prints grades
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
        private static void SendGradeData(int ID, string[] gradeData) //Sends stored Gradedata to the corresponding list
        {
            int index = studentData.FindIndex(a => a.ID == ID);

            studentData[index].AddGradesOnStartUp(int.Parse(gradeData[0]), gradeData[1], gradeData[2]);
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
                    case "y": validSelection = true; subjectData.Clear(); StudentsManager.ClearStudentsFile(); Console.Clear(); break;
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
                    case "y": validSelection = true; studentData.Clear(); SubjectsManager.ClearSubjectFile(); Console.Clear(); break;
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
                    case "y": validSelection = true; statisticData.Clear(); StatisticsMechanisms.ClearStatisticsFile(); Console.Clear(); break;
                    case "n": validSelection = true; Console.Clear(); break;
                    default: Console.Clear(); Console.WriteLine("Please enter a valid option."); validSelection = false; break;
                }
            } while (!validSelection);     
        }
        private void CreateUser(string header) //Creates a new administrator. Header is for the message that swhows on screen.
        {
            string userinput = string.Empty;
            string username = string.Empty;
            string userType = string.Empty;
            bool validInput = false;

            int ID = 0;

            Random rng = new Random();

            Console.Clear();
            Console.WriteLine(header);

            //Gives the user a unique ID
            using (StreamReader file = new StreamReader(usersDatabase))
            {
                int rowsChecked = 0; //Keeps track of how many rows have been checked
                int amountOfLines = File.ReadLines(usersDatabase).Count(); //Calculates amount of lines in the txt-file

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
                            line = File.ReadLines(usersDatabase).Skip(rowsChecked).Take(1).First(); //Selects the row from txt-file that should be read
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

            //Lets user choose user type
            using (StreamReader file = new StreamReader(usersDatabase))
            {
                if (File.ReadLines(usersDatabase).Count() == 0)
                {
                    userType = "admin";
                }
                else
                {
                    do
                    {
                        Console.WriteLine($"Should the user have:");
                        Console.WriteLine("1 - Normal access");
                        Console.WriteLine("2 - Administrator access");
                        userinput = Console.ReadLine().ToLower();
                        switch (userinput)
                        {
                            case "1": userType = "normal"; validInput = true; break;
                            case "2": userType = "admin"; validInput = true; break;
                            default: Console.Clear(); Console.WriteLine("Invalid input. Try again."); validInput = false; break;
                        }
                    } while (!validInput);
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
                    using (StreamWriter sw = File.AppendText(usersDatabase))
                    {
                            string userInput = $"{ID},{userType},{username},{Convert.ToBase64String(Hash.GetSHA1(username, password1))}";
                            sw.WriteLine(userInput);
                    }
                }

            } while (password1 != password2 && userinput != "n");
        }
        private void ConfigureUsers()
        {

        }
        private void AddAccesToUser()
        {

            bool validSelection = false;

            string userName = string.Empty;
            string accesCode = string.Empty;

            List<string> tempData = new List<string>();

            //Reads userdatabase and writes it into memory
            using (StreamReader sw = new StreamReader(usersDatabase))
            {
                int rowsChecked = 0; //Keeps track of how many rows have been checked
                int amountOfLines = File.ReadLines(usersDatabase).Count(); //Calculates amount of lines in the txt-file

                string line;

                for (int Ind = 0; Ind < amountOfLines; Ind++)
                {
                    line = File.ReadLines(usersDatabase).Skip(rowsChecked).Take(1).First(); //Selects the row from txt-file that should be read
                    rowsChecked++;

                    if (line != null)
                    {
                        try
                        {
                            tempData.Add(new string(line));

                        }
                        catch
                        {
                            Console.WriteLine("Error");
                        }
                    }
                }

            }
            string[] dataText = null;

            do
            {
                //Let's user select which user to add info to
                do
                {
                    Console.Clear();
                    Console.WriteLine("Choose which user you want to add class access to: ");
                    Console.WriteLine("You can choose by typing either username or ID.");

                for (int i = 0; i < tempData.Count; i++)
                {
                    string line = tempData[i];
                    dataText = line.Split(',');
                    if (dataText[1] != "admin")
                    {
                        Console.WriteLine($"{i+1} - User:{dataText[2]} ID: {dataText[0]}");
                    }
                }
                    string userInput = Console.ReadLine();
                    for (int i = 0; i < tempData.Count; i++)
                    {
                        if (tempData[i].Contains(userInput))
                        {
                            userName = userInput;
                            validSelection = true;
                            break;
                        }
                        else
                        {
                            validSelection = false;
                            Console.WriteLine("Please choose a option.");
                        }
                    }
                } while (!validSelection);

                //Let's user select which access code to add to the user
                do
                {
                    Console.Clear();
                    Console.WriteLine("Please enter the aproriate access code:");
                    for (int i = 0; i < classData.Count; i++)
                    {
                        Console.WriteLine($"{classData[i].className} - Access Code:{classData[i].accesCode}");
                    }
                    string userInput = Console.ReadLine();
                    for (int i = 0; i < classData.Count; i++)
                    {
                        if (userInput == classData[i].accesCode)
                        {
                            accesCode = userInput;
                            validSelection = true;
                            break;
                        }
                        else
                        {
                            validSelection = false;
                            Console.WriteLine("Please choose a option.");
                        }
                    }
                } while (!validSelection);

            } while (!validSelection);
            int index = -1;
            try
            {
                index = tempData.FindIndex(a => a.Contains(userName));
                if (dataText.Length > 5)
                {
                    for (int i = 5; i < dataText.Length; i++)
                    {
                        accesCode += $",{dataText[i]}";
                    }
                }
            }
            catch
            {

            }


            if (index != -1)
            {
                dataText = tempData[index].Split(',');
                tempData.RemoveAt(index);
                tempData.Add(new string($"{dataText[0]},{dataText[1]},{dataText[2]},{dataText[3]}, {accesCode}"));
            }

            using (StreamWriter sw = File.AppendText(usersDatabase))
            {
                sw.Write(string.Empty);
                foreach (var i in tempData)
                {
                    sw.WriteLine(i);
                }
                tempData.Clear();
            }
        }


        //Users interactions
        private bool ValidateCredentials(string username, string password) //Validates user input credentials
        {
            byte[] hashedCredentials = new byte[20];
            var userInput = Hash.GetSHA1(username, password);

            string[] dataText;
            string userType = string.Empty;

            bool validation = false;

            if (File.Exists(usersDatabase))
            {
                Console.WriteLine("\nChecking Credentials");
                FakeLoading();
                int error = 0; //Calculates eventually errors
                using (StreamReader file = new StreamReader(usersDatabase))
                {
                    int rowsChecked = 0; //Keeps track of how many rows have been checked
                    int amountOfLines = File.ReadLines(usersDatabase).Count(); //Calculates amount of lines in the txt-file

                    string line;

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
                            line = File.ReadLines(usersDatabase).Skip(rowsChecked).Take(1).First(); //Selects the row from txt-file that should be read
                            rowsChecked++;

                            if (line != null)
                            {
                                try
                                {
                                    dataText = line.Split(',');
                                    string temp = dataText[3];
                                    userType = dataText[1];
                                    hashedCredentials = Convert.FromBase64String(temp);
                                }
                                catch
                                {
                                    error++;
                                }
                            }

                            if (hashedCredentials != null)
                            {
                                if (Hash.MatchSHA1(hashedCredentials, userInput) && userType == "admin")
                                {
                                    validation = true;
                                    userIsAdmin = true;
                                    userIsAuthenticated = true;
                                    break;
                                }
                                else if (Hash.MatchSHA1(hashedCredentials, userInput) && userType != "admin")
                                {
                                    validation = true;
                                    userIsAdmin = false;
                                    userIsAuthenticated = true;
                                    break;
                                }
                                else
                                {
                                    validation = false;
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
            }

            return validation;
        }
        public static string MaskedReadLine() //Masks user input
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
        public static bool IsUserAdmin()
        {
            return userIsAdmin;
        }


        //Extra code that makes the work easier
        public static void FakeLoading()
        {
           /*for (int i = 0; i < 20; i++)
            {
                Console.Write('.');
                Thread.Sleep(75);
            }*/
            Console.WriteLine();
        }
        public static void AwaitUserInput() //Awaits user input before continuing. Clears console
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
        public void EncryptAllFiles() //Encrypts all files
        {
            FileEncryption.EncryptAllFiles();
            FileEncryption.EncryptUsers();
        }
        private void ClearLists()
        {
            studentData.Clear();
            subjectData.Clear();
            statisticData.Clear();
        }

        /* YET TO IMPLEMENT:
         * Permissions for teachers, so they can only access specific classes FIX THIS
         * SQL-Implementation
         * GUI
         */
    }
}