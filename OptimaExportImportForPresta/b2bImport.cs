using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Timers;

namespace OptimaExportImportForPresta
{
    public partial class b2bImport : ServiceBase
    {
        public b2bImport()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Process();
        }

        protected override void OnStop()
        {
        }
        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            ComarchOptimaImportOrder comarchOptimaImportOrder = new ComarchOptimaImportOrder();
            EventLog eventLog = new EventLog();


            eventLog.Source = "IntegracjaB2B";

            comarchOptimaImportOrder.ComarchOptimaImportOrderStart(eventLog);
        }

        public  void Process()
        {
            Timer _timer = new Timer();
            // In miliseconds 60000 = 1 minute
            // This timer will tick every 1 minute
      
            _timer.Interval += 1000*60* Convert.ToInt32(Properties.Settings.Default.Interval);
            
            _timer.Enabled = true;
           
            _timer.Elapsed += OnTimedEvent;

            // Create the source, if it does not already exist.
            if (!EventLog.SourceExists("IntegracjaB2B"))
            {
                //An event log source should not be created and immediately used.
                //There is a latency time to enable the source, it should be created
                //prior to executing the application that uses the source.
                //Execute this sample a second time to use the new source.
                EventLog.CreateEventSource("IntegracjaB2B", "IntegracjaB2Blog");
              
            }

            ComarchOptimaImportOrder comarchOptimaImportOrder = new ComarchOptimaImportOrder();
            EventLog eventLog = new EventLog();


            eventLog.Source = "IntegracjaB2B";

            comarchOptimaImportOrder.ComarchOptimaImportOrderStart(eventLog);
#if (DEBUG)
            while (true) ;
#endif


        }

    }
}
