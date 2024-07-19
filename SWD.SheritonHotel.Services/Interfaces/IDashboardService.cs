using SWD.SheritonHotel.Domain.DTO.Responses;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardDataAsync();
    }
}
