using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AdaptiveProgrammingMEF;
using AdaptiveProgrammingModel;
using AdaptiveProgrammingTrace;
using AdaptiveProgrammingViewModel;

namespace AdaptiveProgrammingConsoleView
{
    class ConsoleMain
    {
        private static MainViewModel viewModel;
        private static bool exit;

        static void Main(string[] args)
        {
            exit = false;
            viewModel = new MainViewModel();
            viewModel.DLLFileBrowser = new ConsoleBrowse();
            Console.WriteLine("Commands:");
            Console.WriteLine("  browse");
            Console.WriteLine("  load");
            Console.WriteLine("  expand");
            Console.WriteLine("  fold");
            Console.WriteLine("  serialize");
            Console.WriteLine("  deserialize");
            Console.WriteLine("  exit");
            while (!exit)
            {
                string cmd = Console.ReadLine();
                switch (cmd)
                {
                    case "browse":
                        viewModel.BrowseDll.Execute(null);
                        break;
                    case "load":
                        if (viewModel.ChangeLoadButtonState)
                        {
                            try
                            {
                                viewModel.LoadDll.Execute(null);
                                Console.WriteLine(viewModel.TreeViewArea[0].Name);
                            }
                            catch (FileNotFoundException e)
                            {
                                Console.WriteLine("File not found");
                            }
                        }
                        else
                        {
                            Console.WriteLine("You have to select file - use \"browse\" ");
                        }

                        break;
                    case "expand":
                        Console.WriteLine("Get name to expand: ");
                        string nameToExpand = Console.ReadLine();
                        Expand(nameToExpand);
                        break;
                    case "fold":
                        Console.WriteLine("Get name to fold: ");
                        string nameToFold = Console.ReadLine();
                        Fold(nameToFold);
                        break;
                    case "serialize":
                        if (viewModel.ChangeSerializeButtonState)
                        {
                            viewModel.Serialize.Execute(null);
                        }
                        else
                        {
                            Console.WriteLine("You have to load file - use \"load\" ");
                        }

                        break;
                    case "deserialize":
                        viewModel.Deserialize.Execute(null);
                        Console.WriteLine(viewModel.TreeViewArea[0].Name);
                        break;
                    case "exit":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Commands not found!");
                        break;
                }
            }
        }

        private static void Fold(string nameToFold)
        {
            if (viewModel.TreeViewArea[0].Name == nameToFold || viewModel.TreeViewArea[0].IsExpanded)
            {
                int depth = 0;
                Console.WriteLine("_________________________________________________________________________");
                Console.WriteLine(viewModel.TreeViewArea[0].Name);
                if (viewModel.TreeViewArea[0].Name == nameToFold)
                    viewModel.TreeViewArea[0].IsExpanded = false;
                if (viewModel.TreeViewArea[0].IsExpanded)
                {
                    FoldR(nameToFold, viewModel.TreeViewArea[0].Children, depth);
                }
            }
        }

        private static void FoldR(string nameToFold, ObservableCollection<TreeViewItem> item, int depth)
        {
            string before = "";
            for (int j = 0; j < depth; j++)
            {
                before += "|  ";
            }

            before += "|__";
            foreach (TreeViewItem i in item)
            {
                Console.WriteLine(before + i.Name);
                if (i.Name == nameToFold || i.IsExpanded)
                {
                    if (i.Name == nameToFold)
                        i.IsExpanded = false;
                    if (i.IsExpanded)
                    {
                        depth++;
                        FoldR(nameToFold, i.Children, depth);
                    }
                }
            }
        }

        private static void Expand(string nameToExpand)
        {
            if (viewModel.TreeViewArea[0].Name == nameToExpand || viewModel.TreeViewArea[0].IsExpanded)
            {
                int depth = 0;
                Console.WriteLine("_________________________________________________________________________");
                Console.WriteLine(viewModel.TreeViewArea[0].Name);
                viewModel.TreeViewArea[0].IsExpanded = true;
                ExpandR(nameToExpand, viewModel.TreeViewArea[0].Children, depth);
                Console.WriteLine("_________________________________________________________________________");
            }

        }

        private static void ExpandR(string nameToExpand, ObservableCollection<TreeViewItem> item, int depth)
        {
            string before = "";
            for (int j = 0; j < depth; j++)
            {
                before += "|  ";
            }
            before += "|__";
            foreach (TreeViewItem i in item)
            {
                Console.WriteLine(before + i.Name);
                if (i.Name == nameToExpand || i.IsExpanded)
                {
                    i.IsExpanded = true;
                    depth++;
                    ExpandR(nameToExpand, i.Children, depth);
                }
            }
        }
    }
}
