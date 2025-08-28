namespace EventApp.Services.EventAvailability
{
    public interface IEventAvailabilityService
    {
        Task<bool> IsAvailableAsync(DateTime start, DateTime end, Guid? excludeEventId = null);
    }
}

