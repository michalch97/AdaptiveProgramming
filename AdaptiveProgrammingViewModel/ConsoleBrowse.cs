﻿using System;

namespace AdaptiveProgrammingViewModel
{
    public class ConsoleBrowse : IBrowse
    {
        public string Browse()
        {
            string path;
            Console.WriteLine("Get path to DLL/JSON file: ");
            path = Console.ReadLine();
            return path;
        }
    }
}