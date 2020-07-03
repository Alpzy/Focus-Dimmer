using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace Focus_Dimmer
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(FocusDimmer.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class FocusDimmer : AsyncPackage
    {
        public const string PackageGuidString = "43eb844c-abc7-4dca-842c-1309721e4bdc";

        private static bool isOn = false;

        public static bool IsOn { get{ return isOn; } set{ isOn = value;  Toggled?.Invoke(new Object(), new EventArgs()); } }

        public static event EventHandler Toggled;

        #region Package Members
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await ToggleOnOffCommand.InitializeAsync(this);
        }

        #endregion
    }
}
