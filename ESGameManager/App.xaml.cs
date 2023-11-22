using ESGameManagerLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ESGameManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Common.SetDispatcher(System.Windows.Threading.Dispatcher.CurrentDispatcher);
            System.Windows.Threading.Dispatcher.CurrentDispatcher.UnhandledException += CurrentDispatcher_UnhandledException;
            base.OnStartup(e);
            //Shutdown(1);
            //TODO: Add Check for update.

            //Install file location: -- make the github.com repository?  Need to make sure it's accessible to non-logged-in users.
            //If update found, ask:  download now?
            //                       Install on exit
            //                       exit and install now.

            //QUESTION:  Will settings be retained on an update install????  If not, then cannot run installer to update.
        }

        private void CurrentDispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Common.FatalApplicationException(e.Exception);
        }
    }
}
