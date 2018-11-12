using System;
using System.Threading;
using System.Windows.Input;

namespace AdaptiveProgrammingViewModel
{
    public class Serialize : ICommand
    {
        private MainViewModel viewModel;
        public Serialize(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Thread thread = new Thread(() =>
            {
                this.viewModel.SerializeFile();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        public event EventHandler CanExecuteChanged;
    }
}