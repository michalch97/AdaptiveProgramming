using System.Windows;
using AdaptiveProgrammingViewModel;
using Microsoft.Win32;

namespace AdaptiveProgrammingView
{
    public class WPFBrowse : IBrowse
    {
        public string Browse()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Dynamic Library File(*.dll)|*.dll|Json files (*.json)|*.json"
            };
            if (openFileDialog.FileName.Length == 0)
            {
                MessageBox.Show("No files selected");
                return null;
            }
            else
            {
                return openFileDialog.FileName;
            }
        }
    }
}