using System;
using System.ComponentModel.Design;
using Focus_Dimmer.Enums;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace Focus_Dimmer.Commands
{
    public sealed class ToggleModeCommand
    {
        public const int CommandId = 0x0101;

        public static readonly Guid CommandSet = new Guid("7cef4032-34d2-4bfd-9b60-b1ae5e3f0305");

        private readonly AsyncPackage package;

        public EventHandler toggled;

        private ToggleModeCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.onToggled, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static ToggleModeCommand Instance
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
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new ToggleModeCommand(package, commandService);
        }

        private void onToggled(object sender, EventArgs e)
        {
            FocusDimmer.Mode = FocusDimmer.Mode == Modes.DimGray ? Modes.Transparent : Modes.DimGray;
        }
    }
}
