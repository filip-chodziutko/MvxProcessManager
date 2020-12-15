using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            ChangePriorityCommand = new MvxCommand(ChangePriority);

            Processes = new MvxObservableCollection<Process>(Process.GetProcesses());
        }

        public IMvxCommand RefreshCommand { get; set; }
        public IMvxCommand KillCommand { get; set; }
        public IMvxCommand ChangePriorityCommand { get; set; }
        public IEnumerable<ProcessPriorityClass> ProcessPriorities => Enum.GetValues(typeof(ProcessPriorityClass)).Cast<ProcessPriorityClass>();

        private double _refreshFrequency = 1.0; // seconds
        private MvxObservableCollection<Process> _processes = new MvxObservableCollection<Process>();
        private Process _selectedProcess;
        private ProcessPriorityClass _selectedProcessPriority = ProcessPriorityClass.Normal;
        private bool _doRefresh;
        private string _processNameFilter = "";

        public void Refresh()
        {
            IEnumerable<Process> receivedProcesses = Process.GetProcesses().Where(p => p.ProcessName.ToLower().StartsWith(_processNameFilter));
            var tmpId = SelectedProcess?.Id;
            Processes = new MvxObservableCollection<Process>(receivedProcesses);
            var stillSelected = Processes.Where(p => p.Id == tmpId);
            if (stillSelected.Any())
            {
                SelectedProcess = stillSelected.First();
            }
        }

        public void Kill()
        {
            if (SelectedProcess != null)
            {
                SelectedProcess.Kill();
                SelectedProcess = null;
            }
        }

        public void ChangePriority()
        {
            if (SelectedProcess != null)
            {
                SelectedProcess.PriorityClass = SelectedProcessPriority;
                RaisePropertyChanged(() => SelectedProcess);
            }
        }

        public ProcessPriorityClass SelectedProcessPriority
        {
            get => _selectedProcessPriority;
            set => SetProperty(ref _selectedProcessPriority, value);
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

        public MvxObservableCollection<Process> Processes
        {
            get => _processes;
            set
            {
                SetProperty(ref _processes, value);
                //RaisePropertyChanged(() => VisibleProcesses);
            }
        }

        //public MvxObservableCollection<Process> VisibleProcesses
        //{
        //    get => new MvxObservableCollection<Process>(
        //            _processes.Where(p => p.ProcessName.ToLower().StartsWith(_processNameFilter)
        //        ));
        //}

        public Process SelectedProcess
        {
            get => _selectedProcess;
            set => SetProperty(ref _selectedProcess, value);
        }

        public bool DoRefresh
        {
            get => _doRefresh;
            set
            {
                SetProperty(ref _doRefresh, value);
                if (value)
                {
                    Task refreshTask = new Task(() =>
                    {
                        while (_doRefresh)
                        {
                            Refresh();
                            Thread.Sleep((int)RefreshFrequency*1000);
                        }
                        Console.Out.WriteLine("Finished");
                    });
                    refreshTask.Start();
                }
            }
        }
    }
}
