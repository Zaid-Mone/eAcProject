using Models;
using DTOs.Users;
using DTOs.Auth;
using Models.Entities.Auth;

namespace DataAccess.IRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        List<User> Search(string filter = "");
        int Count();
        Guid GetCurrentLoggedInUserID();
        string GetCurrentLoggedInUserEmail();
        string GetCurrentLoggedInUserRole();
        Task<UserInfoDTO> GetUserInfo(string userID );
        void UserSeeding();
        Task<UserInfoDTO> GetUserInfoByEmail(string Email);
        Task<Tuple<string,int>> Register(RegisterDTO registerDTO);
        Task<ProfileDTO> GetUserProfile(string userId, int groupId);
    }
}
