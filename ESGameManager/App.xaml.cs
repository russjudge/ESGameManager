using ESGameManagerLibrary;
using RussJudge.UpdateCheck;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace ESGameManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string UpdateURLFile = "https://russjudge.com/software/ESGameManager.version";
        public static bool UpdateOnClose { get; private set; } = false;
        public static string UpdateInstallerPath { get; private set; } = string.Empty;
        protected override void OnExit(ExitEventArgs e)
        {
            if (UpdateOnClose && !string.IsNullOrEmpty(UpdateInstallerPath))
            {
                if (RussJudge.UpdateCheck.UpdateChecker.IsDownloadingSetupFile && RussJudge.UpdateCheck.UpdateChecker.LockingObject != null)
                {
                    RussJudge.UpdateCheck.UpdateChecker.LockingObject.WaitOne();
                }
                if (System.IO.File.Exists(UpdateInstallerPath))
                {
                    System.Diagnostics.Process.Start(UpdateInstallerPath);
                }
                UpdateOnClose = false;  //To ensure it doesn't get called twice by mistake.
            }
            base.OnExit(e);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Common.SetDispatcher(System.Windows.Threading.Dispatcher.CurrentDispatcher);
            System.Windows.Threading.Dispatcher.CurrentDispatcher.UnhandledException += CurrentDispatcher_UnhandledException;
            if (ESGameManager.Properties.Settings.Default.UpgradeRequired)
            {
                ESGameManager.Properties.Settings.Default.Upgrade();
                ESGameManager.Properties.Settings.Default.UpgradeRequired = false;
                ESGameManager.Properties.Settings.Default.Save();
            }

            base.OnStartup(e);

            var checker = new RussJudge.UpdateCheck.UpdateChecker(UpdateURLFile);
            checker.CheckForUpdate(true).ContinueWith((Result) =>
            {
                Console.WriteLine("Update check completed"); //, result = {0}", Result.Result.ToString());
                switch (Result.Result)
                {
                    case UpdateType.UpdateOnClose:
                        UpdateOnClose = true;
                        UpdateInstallerPath = checker.SetupFilePath;
                        break;
                    case UpdateType.UpdateNow:
                        UpdateInstallerPath = checker.SetupFilePath;

                        UpdateOnClose = true;
                        Shutdown(0);

                        break;
                }

            });


            //if (CheckForUpdate())
            //{
            //    Shutdown(0);
            //}
            //Shutdown(1);
            //TODO: Add Check for update.

            //Install file location: -- make the github.com repository?  Need to make sure it's accessible to non-logged-in users.
            //If update found, ask:  download now?
            //                       Install on exit
            //                       exit and install now.

            //QUESTION:  Will settings be retained on an update install????  If not, then cannot run installer to update.
        }
        //russjudge.com/software/esgamemanager.version

        private void CurrentDispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Common.FatalApplicationException(e.Exception);
            if (UpdateOnClose && !string.IsNullOrEmpty(UpdateInstallerPath))
            {
                if (RussJudge.UpdateCheck.UpdateChecker.IsDownloadingSetupFile && RussJudge.UpdateCheck.UpdateChecker.LockingObject != null)
                {
                    RussJudge.UpdateCheck.UpdateChecker.LockingObject.WaitOne();
                }
                if (System.IO.File.Exists(UpdateInstallerPath))
                {
                    System.Diagnostics.Process.Start(UpdateInstallerPath);
                }
                UpdateOnClose = false;  //To ensure it doesn't get called twice by mistake.
            }
        }

    }
}
