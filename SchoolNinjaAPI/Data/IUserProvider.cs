using SchoolNinjaAPI.Models;

namespace SchoolNinjaAPI.Data
{
    public interface IUserProvider
    {
        int GetUserId();
        User GetUser();
    }
}
