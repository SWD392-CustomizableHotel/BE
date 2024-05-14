using Core.Entities;

namespace Core.Interfaces
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetAllUsers();
    }
}
