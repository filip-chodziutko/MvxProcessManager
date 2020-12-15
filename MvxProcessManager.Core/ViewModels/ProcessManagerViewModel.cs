using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace MvxProcessManager.Core.ViewModels
{
    public class ProcessManagerViewModel : MvxViewModel
    {
        public ProcessManagerViewModel()
        {
            RefreshCommand = new MvxCommand(Refresh);
            KillCommand = new MvxCommand(Kill);
            Refresh();
        }

        public IMvxCommand RefreshCommand { get; set; }
        public IMvxCommand KillCommand { get; set; }

        private double _refreshFrequency = 1.0; // seconds
        private List<Process> _processes;
        private Process _selectedProcess;
        private bool _doRefresh;
        private string _processNameFilter = "";

        public void Refresh()
        {
            Processes = Process.GetProcesses().Where(p => p.ProcessName.ToLower().StartsWith(_processNameFilter)).ToList();
        }

        public void Kill()
        {
            Processes.Remove(SelectedProcess);
            SelectedProcess?.Kill();
            SelectedProcess = null;
            //Thread.Sleep(10);
            Refresh();
        }

        public string ProcessNameFilter
        {
            get => _processNameFilter;
            set
            {
                SetProperty(ref _processNameFilter, value.ToLower());
                Refresh();
            }
        }

        public double RefreshFrequency
        {
            get => _refreshFrequency;
            set => SetProperty(ref _refreshFrequency, value);
        }

        public List<Process> Processes
        {
            get => _processes;
            set => SetProperty(ref _processes, value);
        }

        public Process SelectedProcess
        {
            get => _selectedProcess;
            set => SetProperty(ref _selectedProcess, value);
        }

        public bool DoRefresh
        {
            get => _doRefresh;
            set => SetProperty(ref _doRefresh, value);
        }
    }
}
