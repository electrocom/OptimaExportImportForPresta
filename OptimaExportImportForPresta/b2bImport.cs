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
           

            comarchOptimaImportOrder.ComarchOptimaImportOrderStart();
        }

        public  void Process()
        {
            Timer _timer = new Timer();
            // In miliseconds 60000 = 1 minute
            // This timer will tick every 1 minute
      
            _timer.Interval += 1000*60* Convert.ToInt32(Properties.Settings.Default.Interval);
            
            _timer.Enabled = true;
           
            _timer.Elapsed += OnTimedEvent;

           
            ComarchOptimaImportOrder comarchOptimaImportOrder = new ComarchOptimaImportOrder();
            comarchOptimaImportOrder.ComarchOptimaImportOrderStart();

#if (DEBUG)
            while (true) ;
#endif


        }

    }
}
