namespace SWD.SheritonHotel.Domain.Queries;

public class GetBookingHistoryQuery
{
    public string UserId { get; }

    public GetBookingHistoryQuery(string userId)
    {
        UserId = userId;
    }
}