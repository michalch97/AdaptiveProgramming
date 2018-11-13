using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaptiveProgrammingModel;
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
                        viewModel.LoadDll.Execute(null);
                        Console.WriteLine(viewModel.TreeViewArea[0].Name);
                        break;
                    case "expand":
                        Console.WriteLine("Get name to expand: ");
                        string nameToExpand = Console.ReadLine();
                        Expand(nameToExpand);
                        break;
                    case "fold":Console.WriteLine("Get name to fold: ");
                        string nameToFold = Console.ReadLine();
                        Fold(nameToFold);
                        break;
                    case "serialize":
                        viewModel.Serialize.Execute(null);
                        break;
                    case "deserialize":
                        viewModel.Deserialize.Execute(null);
                        Console.WriteLine(viewModel.TreeViewArea[0].Name);
                        break;
                    case "exit":
                        exit = true;
                        break;
                }
            }

        }

        private static void Fold(string nameToFold)
        {
            if (viewModel.TreeViewArea[0].Name == nameToFold || viewModel.TreeViewArea[0].IsExpanded)
            {
                Console.WriteLine("_________________________________________________________________________");
                Console.WriteLine(viewModel.TreeViewArea[0].Name);
                if (viewModel.TreeViewArea[0].Name == nameToFold)
                    viewModel.TreeViewArea[0].IsExpanded = false;
                if (viewModel.TreeViewArea[0].IsExpanded)
                {
                    foreach (TreeViewItem item in viewModel.TreeViewArea[0].Children)
                    {
                        Console.WriteLine("|__" + item.Name);
                        if (item.Name == nameToFold || item.IsExpanded)
                        {
                            if (item.Name == nameToFold)
                                item.IsExpanded = false;
                            if (item.IsExpanded)
                            {
                                foreach (TreeViewItem childrenItem in item.Children)
                                {
                                    Console.WriteLine("|  |__" + childrenItem.Name);
                                    if (childrenItem.Name == nameToFold || childrenItem.IsExpanded)
                                    {
                                        if (childrenItem.Name == nameToFold)
                                            childrenItem.IsExpanded = false;
                                        if (childrenItem.IsExpanded)
                                        {
                                            foreach (TreeViewItem childrenChildrenItem in childrenItem.Children)
                                            {
                                                Console.WriteLine("|  |  |__" + childrenChildrenItem.Name);
                                                if (childrenChildrenItem.Name == nameToFold || childrenChildrenItem.IsExpanded)
                                                {
                                                    if (childrenChildrenItem.Name == nameToFold)
                                                        childrenChildrenItem.IsExpanded = false;
                                                    if (childrenChildrenItem.IsExpanded)
                                                    {
                                                        foreach (TreeViewItem childrenChildrenChildrenItem in childrenChildrenItem.Children)
                                                        {
                                                            Console.WriteLine("|  |  |  |__" + childrenChildrenChildrenItem.Name);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("_________________________________________________________________________");
            }
        }

        private static void Expand(string nameToExpand)
        {
            if (viewModel.TreeViewArea[0].Name == nameToExpand || viewModel.TreeViewArea[0].IsExpanded)
            {
                Console.WriteLine("_________________________________________________________________________");
                Console.WriteLine(viewModel.TreeViewArea[0].Name);
                viewModel.TreeViewArea[0].IsExpanded = true;
                foreach (TreeViewItem item in viewModel.TreeViewArea[0].Children)
                {
                    Console.WriteLine("|__" + item.Name);
                    if (item.Name == nameToExpand || item.IsExpanded)
                    {
                        item.IsExpanded = true;
                        foreach (TreeViewItem childrenItem in item.Children)
                        {
                            Console.WriteLine("|  |__" + childrenItem.Name);
                            if (childrenItem.Name == nameToExpand || childrenItem.IsExpanded)
                            {
                                childrenItem.IsExpanded = true;
                                foreach (TreeViewItem childrenChildrenItem in childrenItem.Children)
                                {
                                    Console.WriteLine("|  |  |__" + childrenChildrenItem.Name);
                                    if (childrenChildrenItem.Name == nameToExpand || childrenChildrenItem.IsExpanded)
                                    {
                                        childrenChildrenItem.IsExpanded = true;
                                        foreach (TreeViewItem childrenChildrenChildrenItem in childrenChildrenItem.Children)
                                        {
                                            Console.WriteLine("|  |  |  |__" + childrenChildrenChildrenItem.Name);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("_________________________________________________________________________");
            }
        }
    }
}
