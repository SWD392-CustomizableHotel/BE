namespace SWD.SheritonHotel.Services.Interfaces;

public interface IAdminService
{
    Task AssignStaffToServiceAsync(string staffId, int serviceId);
}