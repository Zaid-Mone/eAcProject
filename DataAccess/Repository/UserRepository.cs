using DataAccess.Context;
using DataAccess.IRepository;
using DTOs.Auth;
using DTOs.Enums;
using DTOs.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Entities.Auth;

namespace DataAccess.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly eShopContext _context;
        public UserRepository(eShopContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public List<User> Search(string filter = "")
        {
            filter = filter.ToLower();
            if (string.IsNullOrEmpty(filter))
            {
                return this.FindAllByCondition(q => q.IsDeleted == false);
            }
            else
            {
                return this.FindAllByCondition(q => q.IsDeleted == false && (q.ArabicUserName.Contains(filter)
                || q.EnglishUserName.Contains(filter)
                || q.Email.Contains(filter)));
            }
        }

        public int Count()
        {
            return this.FindAllByCondition(q => q.IsDeleted == false).Count();
        }

        public string GetCurrentLoggedInUserEmail()
        {
            var userEmailClaim = _httpContextAccessor.HttpContext.User.FindFirst("Email");

            if (userEmailClaim is not null)
            {
                return userEmailClaim.Value;
            }
            throw new InvalidOperationException("No logged-in user found.");
        }

        public Guid GetCurrentLoggedInUserID()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("Id");
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return userId;
            }
            throw new InvalidOperationException("No logged-in user found.");
        }

        public string GetCurrentLoggedInUserRole()
        {
            var userRoleClaim = _httpContextAccessor.HttpContext.User.FindFirst("Role");
            if (userRoleClaim is not null)
            {
                return userRoleClaim.Value;
            }
            throw new InvalidOperationException("No logged-in user found.");
        }

        public async Task<UserInfoDTO> GetUserInfo(string userID)
        {
            Guid userId = Guid.Parse(userID);

            var userInfo = await (from user in _context.tblUsers
                                  join userRoles in _context.tblUserRoles on user.ID equals userRoles.UserId
                                  join role in _context.tblRoles on userRoles.RoleId equals role.ID
                                  //join admin in _context.tblAdmins on users.ID equals admin.UserID
                                  //join provider in _context.tblProviders on users.ID equals provider.UserID
                                  where user.ID == userId
                                  && user.IsDeleted == false
                                  select new UserInfoDTO
                                  {
                                      BirthDate = user.BirthDate,
                                      EmailConfirmed = user.EmailConfirmed,
                                      ArabicBio = user.ArabicBio,
                                      ArabicUserName = user.ArabicUserName,
                                      Avatar = user.Avatar,
                                      Email = user.Email,
                                      EnglishUserName = user.EnglishUserName,
                                      PhoneNumber = user.PhoneNumber,
                                      EnglishBio = user.EnglishBio,
                                      RoleName = role.EnglishRoleName,

                                      UserID = user.ID.ToString(),

                                      AdminID = (role.EnglishRoleName.Equals("Admin", StringComparison.OrdinalIgnoreCase) ?
                                      (from admin in _context.tblAdmins
                                       where admin.UserID == userId
                                       select admin.ID.ToString())
                                      .FirstOrDefault() : ""),

                                      ProviderID = (role.EnglishRoleName.Equals("Provider", StringComparison.OrdinalIgnoreCase) ?
                                      (from provider in _context.tblProviders
                                       where provider.UserID == userId
                                       select provider.ID.ToString())
                                      .FirstOrDefault() : ""),
                                  }).FirstOrDefaultAsync();
            if (userInfo.RoleName == "SuperAdmin")
            {
                userInfo.GroupID = (int)UserGroups.SuperAdmin;
            }
            else
            {
                userInfo.GroupID = (!string.IsNullOrEmpty(userInfo.AdminID) ? userInfo.GroupID = (int)UserGroups.Admin :
                !string.IsNullOrEmpty(userInfo.ProviderID) ? (int)UserGroups.Provider : (int)UserGroups.Member);
            }
            return userInfo;
        }

        public async Task<UserInfoDTO> GetUserInfoByEmail(string Email)
        {


            var userInfo = await (from user in _context.tblUsers
                                  join userRoles in _context.tblUserRoles on user.ID equals userRoles.UserId
                                  join role in _context.tblRoles on userRoles.RoleId equals role.ID
                                  //join admin in _context.tblAdmins on users.ID equals admin.UserID
                                  //join provider in _context.tblProviders on users.ID equals provider.UserID
                                  where user.Email == Email
                                  && user.IsDeleted == false
                                  select new UserInfoDTO
                                  {
                                      BirthDate = user.BirthDate,
                                      EmailConfirmed = user.EmailConfirmed,
                                      ArabicBio = user.ArabicBio,
                                      ArabicUserName = user.ArabicUserName,
                                      Avatar = user.Avatar,
                                      Email = user.Email,
                                      EnglishUserName = user.EnglishUserName,
                                      PhoneNumber = user.PhoneNumber,
                                      EnglishBio = user.EnglishBio,
                                      RoleName = role.EnglishRoleName,

                                      UserID = user.ID.ToString(),

                                      AdminID = (role.EnglishRoleName.Equals("Admin", StringComparison.OrdinalIgnoreCase) ?
                                      (from users in _context.tblUsers
                                       join admin in _context.tblAdmins on users.ID equals admin.UserID
                                       where users.Email == Email
                                       && admin.IsDeleted == false
                                       select admin.ID.ToString())
                                      .FirstOrDefault() : ""),

                                      ProviderID = (role.EnglishRoleName.Equals("Provider", StringComparison.OrdinalIgnoreCase) ?
                                      (from users in _context.tblUsers
                                       join provider in _context.tblProviders on users.ID equals provider.UserID
                                       where users.Email == Email
                                       && provider.IsDeleted == false
                                       select provider.ID.ToString())
                                      .FirstOrDefault() : ""),

                                  }).FirstOrDefaultAsync();
            userInfo.GroupID = (!string.IsNullOrEmpty(userInfo.AdminID) ? userInfo.GroupID = (int)UserGroups.Admin :
                !string.IsNullOrEmpty(userInfo.ProviderID) ? (int)UserGroups.Provider : (int)UserGroups.Member);
            return userInfo;
        }



        //public async Task<UserInfoDTO> GetUserInfo(string userID)
        //{
        //    Guid userId = Guid.Parse(userID);
        //    var userInfoDTO =await  _context.tblUsers.Where(q => q.ID == userId)
        //        .Select(q => new UserInfoDTO
        //        {
        //            UserID = q.ID.ToString(),
        //            Avatar = q.Avatar,
        //            ArabicBio = q.ArabicBio,
        //            ArabicUserName = q.ArabicUserName,
        //            BirthDate = q.BirthDate,
        //            Email = q.Email,
        //            EmailConfirmed = q.EmailConfirmed,
        //            EnglishBio = q.EnglishBio,
        //            EnglishUserName = q.EnglishUserName,
        //            PhoneNumber = q.PhoneNumber

        //        }).FirstOrDefaultAsync();
        //    return userInfoDTO;
        //}

        public void UserSeeding()
        {
            if (!_context.tblUsers.Any())
            {

                // Add to the User table
                var users = _context.tblUsers.Add(new User
                {
                    EnglishUserName = "Admin",
                    ArabicBio = "",
                    ArabicUserName = "Admin",
                    BirthDate = new DateTime(),
                    CreatedAt = DateTime.Now,
                    Email = "Admin@Admin.com",
                    EnglishBio = "",
                    LastLoginDate = new DateTime(),
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin1234*"),
                    PhoneNumber = "0000000000",
                    PhoneNumberConfirmed = false,
                    EmailConfirmed = false,
                    IsActive = false,
                    IsDeleted = false,
                    Avatar = $"https://ui-avatars.com/api/?name=Admin&length=1"
                });
                _context.SaveChanges();


                var roleID = _context.tblRoles.Where(q => q.IsDeleted == false && q.EnglishRoleName.Contains("SuperAdmin"))
                    .Select(q => q.ID)
                    .FirstOrDefault();


                // Add to UserRoles table
                var role = _context.tblUserRoles.Add(new UserRole
                {
                    UserId = users.Entity.ID,
                    RoleId = roleID,
                    IsDeleted = false,
                });
                _context.SaveChanges();


                // Add to Admin table
                var admin = _context.tblAdmins.Add(new Admin
                {
                    IsDeleted = false,
                    UserID = users.Entity.ID,
                });
                _context.SaveChanges();
            }
        }

        public async Task<Tuple<string, int>> Register(RegisterDTO registerDTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (registerDTO.PhoneNumber.Equals("string")
                        || registerDTO.ArabicUserName.Equals("string")
                        || registerDTO.EnglishUserName.Equals("string")
                        || registerDTO.Email.Equals("string"))
                    {
                        return Tuple.Create((registerDTO.Lang.Equals("en", StringComparison.CurrentCultureIgnoreCase))
                            ? "Something went wrong." : "حدث خطأ ما", (int)HTTPStatusCode.BadRequest);
                    }

                    // Save in User Table
                    var user = new User
                    {
                        ArabicUserName = registerDTO.EnglishUserName,
                        EnglishUserName = registerDTO.EnglishUserName,
                        Email = registerDTO.Email,
                        Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password),
                        ArabicBio = "",
                        EnglishBio = "",
                        BirthDate = registerDTO.BirthDate,
                        CreatedAt = DateTime.Now,
                        EmailConfirmed = false,
                        IsActive = false,
                        IsDeleted = false,
                        LastLoginDate = new DateTime(),
                        PhoneNumber = registerDTO.PhoneNumber,
                        PhoneNumberConfirmed = false,
                        Avatar = $"https://ui-avatars.com/api/?name={registerDTO.EnglishUserName}&length=1",
                    };

                    var check = await _context.tblUsers.AnyAsync(x => x.Email == registerDTO.Email);
                    if (check)
                    {
                        return Tuple.Create((registerDTO.Lang.Equals("en", StringComparison.CurrentCultureIgnoreCase))
                           ? "This Email is Already Exist" : "هذا البريد الإلكتروني موجود بالفعل", (int)HTTPStatusCode.BadRequest);

                    }

                    await _context.tblUsers.AddAsync(user);
                    await _context.SaveChangesAsync();

                    // Save in User Roles Table
                    await _context.tblUserRoles.AddAsync(new UserRole
                    {
                        UserId = user.ID,
                        RoleId = _context.tblRoles.Where(b => b.EnglishRoleName == "Member").FirstOrDefault().ID
                    });

                    await _context.SaveChangesAsync();

                    return Tuple.Create(registerDTO.Lang.Equals("en", StringComparison.CurrentCultureIgnoreCase)
                        ? "The user has been added successfully." : "تمت إضافة المستخدم بنجاح", (int)HTTPStatusCode.OK);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return Tuple.Create(registerDTO.Lang.Equals("en", StringComparison.CurrentCultureIgnoreCase)
                        ? "Something went wrong." : "حدث خطأ ما", (int)HTTPStatusCode.BadRequest);
                }
            }
        }

        public async Task<ProfileDTO> GetUserProfile(string userId, int groupId)
        {
            try
            {
                ProfileDTO result = new();
                Guid userID = Guid.Parse(userId);
                switch (groupId)
                {
                    case (int)UserGroups.Member:
                        result = await _context.tblMembers.Include(x => x.User)
                            .Where(x => x.User.ID == userID && x.IsDeleted == false)
                            .Select(x => new ProfileDTO
                            {
                                UserNameAr = x.User.ArabicUserName,
                                UserNameEn = x.User.EnglishUserName,
                                ArabicBio = x.User.ArabicBio,
                                Avatar = x.User.Avatar,
                                BirthDate = x.User.BirthDate,
                                Email = x.User.Email,
                                EmailConfirmed = x.User.EmailConfirmed,
                                GroupID = groupId,
                                EnglishBio = x.User.EnglishBio,
                                UserID = userId,
                                PhoneNumber = x.User.PhoneNumber,
                                MemberProfileDTO = new MemberProfileDTO
                                {
                                    MemberID = x.ID.ToString()
                                }
                            }).FirstOrDefaultAsync();
                        break;
                    case (int)UserGroups.Provider:
                        result = await _context.tblProviders.Include(x => x.User)
                            .Where(x => x.User.ID == userID && x.IsDeleted == false)
                            .Select(x => new ProfileDTO
                            {
                                UserNameAr = x.User.ArabicUserName,
                                UserNameEn = x.User.EnglishUserName,
                                ArabicBio = x.User.ArabicBio,
                                Avatar = x.User.Avatar,
                                BirthDate = x.User.BirthDate,
                                Email = x.User.Email,
                                EmailConfirmed = x.User.EmailConfirmed,
                                GroupID = groupId,
                                EnglishBio = x.User.EnglishBio,
                                UserID = userId,
                                PhoneNumber = x.User.PhoneNumber,
                                ProviderProfileDTO = new ProviderProfileDTO
                                {
                                    ProviderID = x.ID.ToString()
                                }
                            }).FirstOrDefaultAsync();
                        break;
                    case (int)UserGroups.Admin:
                    default:
                        result = await _context.tblAdmins.Include(x => x.User)
                            .Where(x => x.User.ID == userID && x.IsDeleted == false)
                            .Select(x => new ProfileDTO
                            {
                                UserNameAr = x.User.ArabicUserName,
                                UserNameEn = x.User.EnglishUserName,
                                ArabicBio = x.User.ArabicBio,
                                Avatar = x.User.Avatar,
                                BirthDate = x.User.BirthDate,
                                Email = x.User.Email,
                                EmailConfirmed = x.User.EmailConfirmed,
                                GroupID = groupId,
                                EnglishBio = x.User.EnglishBio,
                                UserID = userId,
                                PhoneNumber = x.User.PhoneNumber,
                                AdminProfileDTO = new AdminProfileDTO
                                {
                                    AdminID = x.ID.ToString()
                                }
                            }).FirstOrDefaultAsync();
                        break;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }

}
