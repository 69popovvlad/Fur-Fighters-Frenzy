using Core.Ioc;
using Core.Ui;
using UnityEngine;

namespace Client.Bootstrappers
{
    public class UiBootstrapper : BootstrapperBase
    {
        public override string Name => "Loading ui";

        public override void Run()
        {
            var windowsSystem = Object.FindFirstObjectByType<WindowsSystem>();
            windowsSystem.Initialize();
            
            Ioc.Instance.RegisterInstance(typeof(WindowsSystem), windowsSystem);

            // var windowData = new LoadingWindow.LoadingWindowData(Bootstrapper.ProgressObserver, false);
            // windowsSystem.ShowWindow<LoadingWindow>(windowData, true);

            base.Run();
        }
    }
}