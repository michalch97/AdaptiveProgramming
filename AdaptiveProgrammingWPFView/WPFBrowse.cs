using System.Threading;
using System.Windows;
using AdaptiveProgrammingViewModel;
using Microsoft.Win32;

namespace AdaptiveProgrammingWPFView
{
    public class WPFBrowse : IBrowse
    {
        public string Browse()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Dynamic Library File(*.dll)|*.dll|XML Files (*.xml)|*.xml"
            };
            openFileDialog.ShowDialog();
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