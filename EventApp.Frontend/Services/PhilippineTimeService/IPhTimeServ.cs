namespace EventApp.Frontend.Services.PhilippineTimeService
{
    public interface IPhTimeServ
    {
        DateTime ToPhTime(DateTime utcDate);
        DateTime Now();
    }
}
