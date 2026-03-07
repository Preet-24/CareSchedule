using CareSchedule.Models;

namespace CareSchedule.Repositories.Interface
{
    public interface IUserRepository
    {
        (List<User> Items, int Total) Search(
            string? name,
            string? role,
            string? email,
            string? phone,
            string? status,
            int page,
            int pageSize,
            string? sortBy,
            string? sortDir);

        User? Get(int id);
        User Create(User entity);
        void Update(User entity);
    }
}