using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Serilog;
using log4net;
using ReleaseSpence.Controllers;

namespace scheduled.tasks.BackgroundJobs
{
    public class PiezometerRepairJob
    {

        ILog _logger = LogManager.GetLogger(typeof(PiezometerRepairJob));

        [Queue("gmstask")]
        public void Run() {
            try
            {
                _logger.Warn("START PROCESS : PiezometerRepairJob");

                Reparador.OnTimedEvent();

                _logger.Warn($"\r\n>>>>>>>>>>> \r\n END PROCESS : PiezometerRepairJob \r\n>>>>>>>>>>>"); 
            }
            catch (Exception ex)
            {
                _logger.Error($"EXCEPTION DETECTED : PiezometerRepairJob \r\n {ex?.Message} \r\n {ex?.InnerException?.Message}"); 
            }
        }
    }
}
