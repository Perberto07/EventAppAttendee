namespace EventApp.Services.PhilippineTimeService
{
    public interface IPhTimeServ
    {
        DateTime ToPhilippineTime(DateTime utcDate);
        DateTime Now();
    }
}
