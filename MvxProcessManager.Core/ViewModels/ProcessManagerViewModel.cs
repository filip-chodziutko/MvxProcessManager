using System.Diagnostics;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace MvxProcessManager.Core.ViewModels
{
    public class ProcessManagerViewModel : MvxViewModel
    {
        public ProcessManagerViewModel()
        {
            RefreshCommand = new MvxCommand(Refresh);
            Refresh();
        }

        public IMvxCommand RefreshCommand { get; set; }

        private double _refreshFrequency = 1.0; // seconds
        private Process[] _processes;
        private Process _selectedProcess;
        private bool _doRefresh;

        public void Refresh()
        {
            Processes = Process.GetProcesses();
        }

        public double RefreshFrequency
        {
            get => _refreshFrequency;
            set
            {
                SetProperty(ref _refreshFrequency, value);
            }
        }

        public Process[] Processes
        {
            get => _processes;
            set
            {
                SetProperty(ref _processes, value);
            }
        }

        public Process SelectedProcess
        {
            get => _selectedProcess;
            set
            {
                SetProperty(ref _selectedProcess, value);
            }
        }

        public bool DoRefresh
        {
            get => _doRefresh;
            set
            {
                SetProperty(ref _doRefresh, value);
            }
        }


    }
}
