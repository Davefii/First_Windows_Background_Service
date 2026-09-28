using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceFullServiceTesting
{
    public partial class MyFullServiceStateImplementation : ServiceBase
    {
        private string logdirectory;
        private string logfilePath;
        public MyFullServiceStateImplementation()
        {
            InitializeComponent();

            CanPauseAndContinue = true;

            CanShutdown = true;

            logdirectory = ConfigurationManager.AppSettings["LogDirectory"];

            if (string.IsNullOrEmpty(logdirectory))
            {
                throw new ConfigurationErrorsException("LogDirectory is not specified in the configuration file.");
            }
            if (!Directory.Exists(logdirectory))
            {
                Directory.CreateDirectory(logdirectory);
            }

            logfilePath = Path.Combine(logdirectory, "ServiceStateLog.txt");
        }
        private void LogServiceEvent(string Messege)
        {
            string logMessege = $"[{DateTime.Now:f}] {Messege}\n";
            File.AppendAllText(logfilePath, logMessege);
            if (Environment.UserInteractive)
            {
                Console.WriteLine(logMessege);
            }
        }
        protected override void OnStart(string[] args)
        {
            LogServiceEvent("Service Started");
        }

        protected override void OnPause()
        {
            LogServiceEvent("Service Paused");
        }

        protected override void OnContinue()
        {
            LogServiceEvent("Service Resumed");
        }

        protected override void OnShutdown()
        {
            LogServiceEvent("Service Shutdown due to system shutdown");
        }

        protected override void OnStop()
        {
            LogServiceEvent("Service Stopped right Now");
        }

        public void StartInConsole()
        {
            OnStart(null);
            Console.WriteLine("Press Enter to Stop the Service...");
            Console.ReadLine();
            OnStop();
            Console.ReadKey();
        }
    }
}
