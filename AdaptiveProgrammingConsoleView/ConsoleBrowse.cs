using System;
using AdaptiveProgrammingViewModel;

namespace AdaptiveProgrammingConsoleView
{
    public class ConsoleBrowse : IBrowse
    {
        public string Browse()
        {
            string path;
            Console.WriteLine("Get path to DLL/XML file: ");
            path = Console.ReadLine();
            return path;
        }
    }
}