using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.ViewModels;

namespace MvxProcessManager.Core.ViewModels
{
    public class ProcessManagerViewModel : MvxViewModel
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
            }
        }
    }
}
