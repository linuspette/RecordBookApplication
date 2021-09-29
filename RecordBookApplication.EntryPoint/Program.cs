﻿using System;

namespace RecordBookApplication.EntryPoint
{
    class Program
    {
        static void Main(string[] args)
        {
            var menu = new Menu();
            menu.LogIn();
            Console.Clear();
            Console.WriteLine("Press any key to close the program...");
            Console.ReadKey();
        }
    }
}
