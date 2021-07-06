using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scheduled.tasks.BackgroundJobs
{
    public class _BackgroundJobs
    {
        public static void Loader() {
            RecurringJob.RemoveIfExists(nameof(PiezometerRepairJob));
            RecurringJob.AddOrUpdate<PiezometerRepairJob>(
                nameof(PiezometerRepairJob),
                job => job.Run(),
                "0 */6 * * *",
                TimeZoneInfo.Local,
                "gmstask"
                );

            RecurringJob.RemoveIfExists(nameof(PiezometerDeleteRecordJob));
            RecurringJob.AddOrUpdate<PiezometerDeleteRecordJob>(
                nameof(PiezometerDeleteRecordJob),
                job => job.Run(),
                "0 */5 * * *",
                TimeZoneInfo.Local,
                "gmstask"
                );
        }
    }
}
