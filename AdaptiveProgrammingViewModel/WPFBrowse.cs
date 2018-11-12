using System.Windows.Forms;

namespace AdaptiveProgrammingViewModel
{
    public class WPFBrowse : IBrowse
    {
        public string Browse()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Dynamic Library File(*.dll)|*.dll|Json files (*.json)|*.json"
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