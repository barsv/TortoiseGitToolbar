﻿using System.ComponentModel.Design;
using FizzWare.NBuilder;
using MattDavies.TortoiseGitToolbar.Config.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using TortoiseGitToolbar.IntegrationTests.Helpers;

namespace TortoiseGitToolbar.IntegrationTests
{
    [TestClass]
    public class ToolbarInvocationShould
    {
        [TestMethod]
        [HostType("VS IDE")]
        public void Launch_all_commands()
        {
            foreach (var toolbarCommand in EnumHelper.GetValues<ToolbarCommand>())
            {
                InvokeCommand(toolbarCommand);
            }
        }

        private static void InvokeCommand(ToolbarCommand toolbarCommand)
        {
            UIThreadInvoker.Invoke((ThreadInvoker) delegate
            {
                var menuItemCmd = new CommandID(PackageConstants.GuidTortoiseGitToolbarCmdSet, (int) toolbarCommand);

                using (var dialogboxPurger = new DialogBoxPurger(NativeMethods.IDOK, 1))
                {
                    dialogboxPurger.Start();

                    ExecuteCommand(menuItemCmd);
                }
            });
        }

        private static void ExecuteCommand(CommandID cmd)
        {
            object customin = null;
            object customout = null;
            var guidString = cmd.Guid.ToString("B").ToUpper();
            var cmdId = cmd.ID;
            var dte = VsIdeTestHostContext.Dte;
            dte.Commands.Raise(guidString, cmdId, ref customin, ref customout);
        }

        private delegate void ThreadInvoker();
    }
}