using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;
using AdaptiveProgrammingModel;
using AdaptiveProgrammingViewModel.Annotations;

namespace AdaptiveProgrammingViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public AssemblyMetadata assemblyMetadata;
        public AssemblyView AssemblyView { get; set; }
        public IDLLSerializer serializer;
        public ObservableCollection<TreeViewItem> TreeViewArea { get; set; }
        public string DLLPath { get; set; }
        public bool ChangeLoadButtonState { get; set; }
        public bool ChangeSerializeButtonState { get; set; }
        public IBrowse DLLFileBrowser { get; set; }
        public ICommand BrowseDll
        {
            get { return new BrowseDLL(this); }
        }
        public ICommand LoadDll
        {
            get { return new LoadDLL(this); }
        }
        public ICommand Serialize
        {
            get { return new Serialize(this); }
        }
        public ICommand Deserialize
        {
            get { return new Deserialize(this); }
        }
        public MainViewModel()
        {
            TreeViewArea = new ObservableCollection<TreeViewItem>();
            AssemblyView assemblyView = new AssemblyView();
            //jsonSerializer = new JSONSerializer();
            serializer = new XMLSerializer();
            ChangeLoadButtonState = false;
            OnPropertyChanged("ChangeLoadButtonState");
            ChangeSerializeButtonState = false;
            OnPropertyChanged("ChangeSerializeButtonState");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void BrowseDLLFile()
        {
            string path = DLLFileBrowser.Browse();
            if (path != null && path.Contains(".dll"))
            {
                DLLPath = path;
                ChangeLoadButtonState = true;
                OnPropertyChanged("ChangeLoadButtonState");
                OnPropertyChanged("DLLPath");
            }
            else
            {
                TraceAP.WarningLog("You should chose a DLL or XML file", "MainViewModel");
                return;
            }
        }

        public void LoadDLLFile()
        {
            assemblyMetadata = AssemblyLoader.LoadAssembly(DLLPath);
            AssemblyView = new AssemblyView();
            AssemblyView.initializeAssembly(assemblyMetadata);
            TraceAP.InfoLog("AssemblyView initialized", "MainViewModel");
            TreeViewArea.Clear();
            TreeViewArea.Add(AssemblyView);
            ChangeLoadButtonState = false;
            OnPropertyChanged("ChangeLoadButtonState");
            ChangeSerializeButtonState = true;
            OnPropertyChanged("ChangeSerializeButtonState");
        }

        public void SerializeFile()
        {
            using (Stream stream = new FileStream("../../../SerializationFile/assembly.xml", FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(assemblyMetadata, stream);
            }
            ChangeSerializeButtonState = false;
            OnPropertyChanged("ChangeSerializeButtonState");
        }

        public void DeserializeFile()
        {
            AssemblyView = new AssemblyView();
            string path = DLLFileBrowser.Browse();
            if (path != null && path.Contains(".xml"))
            {
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                assemblyMetadata = serializer.Deserialize(stream);
                AssemblyView.initializeAssembly(assemblyMetadata);
                lock (AssemblyView)
                {
                    TreeViewArea.Clear();
                    TreeViewArea.Add(AssemblyView);
                }
                ChangeLoadButtonState = false;
                OnPropertyChanged("ChangeLoadButtonState");
            }
            else
            {
                return;
            }
        }
    }
}