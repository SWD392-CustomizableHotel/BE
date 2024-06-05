using Entities;

namespace Interfaces
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetAllUsers();
    }
}
