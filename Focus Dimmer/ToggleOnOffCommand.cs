using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace Focus_Dimmer
{
    public sealed class ToggleOnOffCommand
    {
        public const int CommandId = 0x0100;

        public static readonly Guid CommandSet = new Guid("7cef4032-34d2-4bfd-9b60-b1ae5e3f0305");

        private readonly AsyncPackage package;

        public EventHandler toggled;

        private ToggleOnOffCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.onToggled, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static ToggleOnOffCommand Instance
        {
            get;
            private set;
        }

        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in ToggleOnOffCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new ToggleOnOffCommand(package, commandService);
        }

        private void onToggled(object sender, EventArgs e)
        {
            FocusDimmer.IsOn = !FocusDimmer.IsOn;
        }
    }
}
