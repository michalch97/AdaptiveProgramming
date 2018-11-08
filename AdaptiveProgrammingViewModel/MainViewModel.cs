using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private AssemblyMetadata assemblyMetadata;
        private AssemblyView assemblyView = new AssemblyView();
        public ObservableCollection<TreeViewItem> TreeViewArea { get; set; }
        public string DLLPath { get; set; }
        public bool ChangeButtonState { get; set; } = false;
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
            assemblyMetadata = AssemblyLoader.LoadAssembly(DLLPath);
            assemblyView.initializeAssembly(assemblyMetadata);
            //assemblyView.assemblyMetadata = assemblyMetadata;
            //assemblyView.IsExpanded = true;
            //Dispatcher.CurrentDispatcher.Invoke(() => TreeViewArea.Add(assemblyView));
            lock (assemblyView)
            {
                TreeViewArea.Add(assemblyView);
            }
        }
    }
}