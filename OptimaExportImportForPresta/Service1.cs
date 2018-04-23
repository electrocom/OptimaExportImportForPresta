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

namespace OptimaExportImportForPresta
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }

        public  void Process()
        {
            ComarchOptimaImportOrder comarchOptimaImportOrder =new ComarchOptimaImportOrder();
            EventLog eventLog = new EventLog();
            comarchOptimaImportOrder.ComarchOptimaImportOrderStart(eventLog);
        }
    }
}
