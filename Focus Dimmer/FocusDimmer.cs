using System;
using System.Runtime.InteropServices;
using System.Threading;
using Focus_Dimmer.Commands;
using Focus_Dimmer.Enums;
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

        private static bool isOn = Properties.Settings.Default.defaultOnOff;
        private static Modes mode = (Modes) Properties.Settings.Default.defaultDimmingMode;

        public static bool IsOn { get { return isOn; } set{ isOn = value;  ToggledOnOff?.Invoke(new Object(), new EventArgs()); } }
        public static Modes Mode { get { return mode; } set { mode = value; ToggledMode?.Invoke(new Object(), new EventArgs()); } }

        public static event EventHandler ToggledOnOff;
        public static event EventHandler ToggledMode;

        #region Package Members
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await ToggleOnOffCommand.InitializeAsync(this);
            await ToggleModeCommand.InitializeAsync(this);
            await Focus_Dimmer.Commands.DefaultSettingsCommand.InitializeAsync(this);
        }

        #endregion
    }
}
