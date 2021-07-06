using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire;
using ReleaseSpence.Controllers;

namespace scheduled.tasks.BackgroundJobs
{
    public class PiezometerDeleteRecordJob
    {
        ILog _logger = LogManager.GetLogger(typeof(PiezometerDeleteRecordJob));

        [Queue("gmstask")]
        public void Run()
        {
            try
            {
                _logger.Warn("START PROCESS : PiezometerDeleteRecordJob");

                Reparador.DeleteRecord();

                _logger.Warn($"\r\n>>>>>>>>>>> \r\n END PROCESS : PiezometerDeleteRecordJob \r\n>>>>>>>>>>>");
            }
            catch (Exception ex)
            {
                _logger.Error($"EXCEPTION DETECTED : PiezometerDeleteRecordJob \r\n {ex?.Message} \r\n {ex?.InnerException?.Message}");
            }
        }
    }
}