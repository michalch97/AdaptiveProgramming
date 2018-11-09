using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using AdaptiveProgrammingModel;
using AdaptiveProgrammingViewModel.Annotations;

namespace AdaptiveProgrammingViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private AssemblyView assemblyView = new AssemblyView();
        public ObservableCollection<TreeViewItem> TreeViewArea { get; set; }
        private JSONSerializer jsonSerializer;
        public string DLLPath { get; set; }
        public bool ChangeButtonState { get; set; }
        public ICommand BrowseDll
        {
            get { return new BrowseDLL(this); }
        }
        public ICommand LoadDll
        {
            get { return new LoadDLL(this); }
        }
        public MainViewModel()
        {
            TreeViewArea = new ObservableCollection<TreeViewItem>();
            BindingOperations.EnableCollectionSynchronization(TreeViewArea, assemblyView);
            jsonSerializer = new JSONSerializer();
            ChangeButtonState = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void BrowseDLLFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Dynamic Library File(*.dll)| *.dll"
            };
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName.Length == 0)
            {
                MessageBox.Show("No files selected");
            }
            else
            {
                DLLPath = openFileDialog.FileName;
                ChangeButtonState = true;
                OnPropertyChanged("ChangeButtonState");
                OnPropertyChanged("DLLPath");
            }
        }

        public void LoadDLLFile()
        {
            AssemblyMetadata assemblyMetadata = AssemblyLoader.LoadAssembly(DLLPath);
            //Stream stream = new FileStream("test.json", FileMode.Create, FileAccess.Write);
            //jsonSerializer.Serialize(assemblyMetadata,stream);
            //Stream stream2 = new FileStream("test.json",FileMode.Open, FileAccess.Read);
            //assemblyView.initializeAssembly(jsonSerializer.Deserialize(stream2));
            assemblyView.initializeAssembly(assemblyMetadata);
            lock (assemblyView)
            {
                TreeViewArea.Add(assemblyView);
            }
        }
    }
}