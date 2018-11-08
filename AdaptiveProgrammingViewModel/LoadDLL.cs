﻿using System;
using System.Threading;
using System.Windows.Input;

namespace AdaptiveProgrammingViewModel
{
    public class LoadDLL: ICommand
    {
        private MainViewModel viewModel;
        public event EventHandler CanExecuteChanged;
        public LoadDLL(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //Task.Run(() =>
            //{
            //    this.viewModel.BrowseDLLFile();
            //});
            Thread thread = new Thread(() =>
            {
                this.viewModel.LoadDLLFile();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}