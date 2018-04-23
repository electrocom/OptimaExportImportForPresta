using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace OptimaExportImportForPresta
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        
          static void Main()
    {
#if (!DEBUG)
           ServiceBase[] ServicesToRun;
           ServicesToRun = new ServiceBase[] 
	   { 
	        new b2bImport() 
	   };
           ServiceBase.Run(ServicesToRun);
#else
            b2bImport myServ = new b2bImport();
           myServ.Process();
           // here Process is my Service function
           // that will run when my service onstart is call
           // you need to call your own method or function name here instead of Process();
         #endif
    }
    }
}
