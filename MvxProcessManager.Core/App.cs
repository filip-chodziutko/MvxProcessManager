using MvvmCross.ViewModels;
using MvxProcessManager.Core.ViewModels;

namespace MvxProcessManager.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<ProcessManagerViewModel>();
        }
    }
}
