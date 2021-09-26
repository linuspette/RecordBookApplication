using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RecordBookApplication.EntryPoint
{
    public class Menu
    {
        string data = "database.txt";
        Statistics statistics = new Statistics();
        List<Student> studentData = new List<Student>();
        public void Run() //Initiates program
        {
            //List<Grades> temp = new List<Grades>();

            Console.WriteLine("Looking for existing database...");
            //Thread.Sleep(3500);

            if (File.Exists(data)) //Checks if database exist.
                                   //If it exists, data will be written to list
            {
                Console.WriteLine("Database exists. Loading data...");
                int error = 0;


                using (StreamReader file = new StreamReader(data))
                {
                    int skip = 0;
                    int amountOfLines = File.ReadLines(data).Count();

                    string line;
                    string[] dataText;


                    for (int Ind = 0; Ind < amountOfLines; Ind++)
                    {
                        line = File.ReadLines(data).Skip(skip).Take(1).First();
                        skip++;
                        if (line != null)
                        {
                            try
                            {
                                dataText = line.Split(',');
                                studentData.Add(new Student(int.Parse(dataText[0]), dataText[1], "initiating"));
                                if (dataText.Length < 6)
                                {
                                    string temp = $"{dataText[2]},{dataText[3]},{dataText[4]}";
                                    string[] tempData = temp.Split(',');
                                    SendGradeData(int.Parse(dataText[0]), tempData);
                                }
                                else
                                {
                                    string temp = "";
                                    int counter = 0;
                                    int index = 2;
                                    string[] tempData;

                                    for (int i = 0; i < dataText.Length; i++)
                                    {
                                        counter++;
                                    }
                                    int amountOfGrades = (counter - 2) / 3;
                                    for (int i = 0; i < amountOfGrades; i++)
                                    {
                                        temp = "";
                                        for (int j = 0; j < 3; j++)
                                        {
                                            temp += $"{dataText[index]},";
                                            index++;
                                        }
                                        tempData = temp.Split(',');
                                        SendGradeData(int.Parse(dataText[0]), tempData);
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
                    //Thread.Sleep(3500);

                }
                UI();
            }
            else //If it doesn't exists, creates a new database
            {
                using (File.Create(data)) { };
                Console.WriteLine("No database found. Creating new database...");
                //Thread.Sleep(2000);
                Console.Clear();
                UI();
            }


        }
        public void UI() //User UI for menu
        {
            string userinput = "";

            while (userinput != "0")
            {
                PrintMenu();

                userinput = Console.ReadLine();
                switch (userinput)
                {
                    case "1": AddStudent(); break;
                    case "2": AddRandomStudent(); break;
                    case "3": PrintStudents(); AwaitUserInput(); break;
                    case "4": InitiateSort(); AwaitUserInput(); break;
                    case "5": AddAdditionalGrade(); AwaitUserInput(); break;
                    case "0": break;
                    default: Console.WriteLine("Not a valid option. Try again."); AwaitUserInput(); break;
                }
            }
        }
        public void AddStudent() //Function for adding a student
        {
            Random rng = new Random();


            bool validID = false;

            int ID = rng.Next(11111, 99999); //Randomizes userID. 

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
            }//Makes sure that the ID ins't used by another user. 

            Console.WriteLine("Enter Name"); //Input for Username
            string name = Console.ReadLine();


            studentData.Add(new Student(ID, name, "notRandom")); //Adds the data to the List
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



            studentData.Add(new Student(ID, name, "Random")); //Adds data to list

            int index = studentData.FindIndex(a => a.GetID() == ID);
            string userInput = $"{ID},{name},{string.Join(System.Environment.NewLine, studentData[index].GetGrades())}";

            using (StreamWriter sw = File.AppendText(data)) //Adds data to databse
            {
                sw.WriteLine(userInput);
            }
            Console.WriteLine("Random student added. Press any key to continue...");
            Console.ReadKey();
        }
        public void InitiateSort() //Initiates the sorting. Also lets user to choose which type to sort
        {
            string userinput = "";
            bool validSelection = false;
            while (!validSelection) //Checks which data user wants to sort after
            {
                Console.Clear();
                Console.WriteLine("How do you want to sort the data?");
                Console.WriteLine("1 - By ID");
                Console.WriteLine("2 - By name");
                //Console.WriteLine("3 - By subject");
                //Console.WriteLine("4 - By grades");

                userinput = Console.ReadLine();

                switch (userinput)
                {
                    case "1": studentData = Sort(studentData, "ID"); validSelection = true; break;
                    case "2": studentData = Sort(studentData, "Name"); validSelection = true; break;
                    //case "3": studentData = Sort(studentData, "Subject"); validSelection = true; break;
                    //case "4": studentData = Sort(studentData, "Grade"); validSelection = true; break;
                    default: Console.Clear(); Console.WriteLine("Please select a valid option"); validSelection = false; break;
                }
            }
            Console.Clear();
            WriteToFile();
        }
        public List<Student> Sort(List<Student> _studentData, string _getInfo) //Sorts the list after choosen type
        {
            int i, j;
            string getInfo = _getInfo;
            string userInput = "";
            bool validSelection = false;
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

                    /*case "Subject": //Sorts list after subjects. 
                        _studentData.Sort((x, y) => string.Compare(x.GetSubject(), y.GetSubject()));
                        break;*/
                    /*case "Grade": //Sorts list after grades using InsertionSort. User can choose if the order should ascend/descend
                        for (i = 1; i < _studentData.Count; i++)
                        {
                            for (j = i; j > 0; j--)
                            {
                                if (_studentData[j].GetGrade() < _studentData[j - 1].GetGrade())
                                {
                                    var tmp = _studentData[j - 1];
                                    _studentData[j - 1] = _studentData[j];
                                    _studentData[j] = tmp;
                                }
                            }
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
                            _studentData.Reverse();
                        }
                        break;*/

            }
            return _studentData;
        }
        public void PrintStudents()//Prints the list in console
        {
            foreach (var item in studentData)
            {
                Console.WriteLine(item);
            }
        }
        public void AwaitUserInput() //Awaits user input before continuing. Clears console
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
        private void ClearFile() //Clears Database
        {
            using (TextWriter tw = new StreamWriter(data, false))
            {
                tw.Write(string.Empty);
            }
        }
        private void WriteToFile() //Writes to Database
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
        private void PrintMenu() //Prints menu
        {
            Console.Clear();
            Console.WriteLine(" ---- MENU ---- ");
            Console.WriteLine("1 - Add new student");
            Console.WriteLine("2 - Add new randomized student");
            Console.WriteLine("3 - Show all strudents");
            Console.WriteLine("4 - Sort data");
            Console.WriteLine("0 - Close program");
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

            Console.WriteLine("Please choose which student you want to addd another grade to:");
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

            studentData[index].AddGrades();
            WriteToFile();

        }
        private void DeleteGrade()
        {
            int ID = 0;
            bool validSelection = false;
            int index = -1;

            Console.WriteLine("Please choose which student you want to remove a from: ");
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
            ClearFile();
        }

        /* YET TO IMPLEMENT:
         * Sorting function for grades
         * Search-function
         * Delete student data-function
         * 
         * Be able to choose the amount of subjects
         */
    }
}

