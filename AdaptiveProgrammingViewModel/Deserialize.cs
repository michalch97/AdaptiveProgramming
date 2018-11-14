using System;
using System.Threading;
using System.Windows.Input;

namespace AdaptiveProgrammingViewModel
{
    public class Deserialize : ICommand
    {
        private MainViewModel viewModel;
        public Deserialize(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.viewModel.DeserializeFile();
        }

        public event EventHandler CanExecuteChanged;
    }
}