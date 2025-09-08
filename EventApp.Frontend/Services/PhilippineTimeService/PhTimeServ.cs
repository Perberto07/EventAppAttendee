namespace EventApp.Frontend.Services.PhilippineTimeService
{
    public class PhTimeServ : IPhTimeServ
    {
        private readonly TimeZoneInfo _phTimeZone;

        public PhTimeServ()
        {
            _phTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");
        }
        public DateTime ToPhTime(DateTime utcDate)
        {
            // Ensure the input is treated as UTC
            var utc = DateTime.SpecifyKind(utcDate, DateTimeKind.Utc);
            return TimeZoneInfo.ConvertTimeFromUtc(utc, _phTimeZone);
        }

        public DateTime Now()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _phTimeZone);
        }
    }
}
