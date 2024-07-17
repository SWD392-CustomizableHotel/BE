using SWD.SheritonHotel.Domain.DTO;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardDataAsync();
    }
}
