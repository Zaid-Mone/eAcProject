using Common.JWT;
using DataAccess.Helper;
using DataAccess.IRepository;
using DataAccess.Repository;
using DTOs.Auth;
using DTOs.Enums;
using eShop.CustomAttributes;
using eShop.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Auth;
using System.Net.Http;

namespace eShop.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly ILoggerHelper _loggerHelper;
        public AuthController(ITokenGenerator tokenGenerator,
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository,
            IAdminRepository adminRepository,
            ILoggerHelper loggerHelper)
        {
            _tokenGenerator = tokenGenerator;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _adminRepository = adminRepository;
            _loggerHelper = loggerHelper;
        }



        //[HttpPost("Register")]
        [HttpPost]
        public async Task<ActionResult> Register(RegisterDTO registerDTO)
        {
            try
            {
                var register = await _userRepository.Register(registerDTO);
                if (register.Item2 == 200)
                    return Ok(register);
                else
                    return BadRequest(register);
            }
            catch (Exception ex)
            {

                await _loggerHelper.AddLog(HttpContext, registerDTO);
                return BadRequest(ex.Message);

            }
        }


        //[HttpPost("Login")]
        [HttpPost]
        public ActionResult Login(LoginDTO loginDTO)
        {
            try
            {
                // chcek if the user send request with empty email or pass exist 
                if (string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.Password))
                {
                    var missingField = string.IsNullOrEmpty(loginDTO.Email) ? "email" : "password";
                    _loggerHelper.AddLog(HttpContext, loginDTO);
                    return Unauthorized($"Please enter your {missingField}");
                }



                // chcek if the email exist 
                var user = _userRepository.FindByCondition(x => x.Email == loginDTO.Email);
                //if(user is null) { return NotFound("The email you entered does not exist or is registered."); }
                if (user is null)
                {
                    _loggerHelper.AddLog(HttpContext, loginDTO);
                    return NotFound("Your email is incorrect.");

                }


                if (user.IsLockedOut && user.LockedOutDate > DateTime.Now)
                {
                    var minutesRemaining = (int)(user.LockedOutDate - DateTime.Now).TotalMinutes;
                    _loggerHelper.AddLog(HttpContext, loginDTO);
                    return Unauthorized($"Your account is locked out. Please try again in {minutesRemaining:F0} minutes.");
                }


                // check password match
                var passValidate = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password);
                if (!passValidate)
                {
                    IncrementFailedLoginAttempts(user);
                    _loggerHelper.AddLog(HttpContext, loginDTO);
                    return Unauthorized("Password not matching");
                }


                // Generate token
                var token = _tokenGenerator.GenerateToken(user.ID.ToString());

                // update user logged in date 
                user.IsActive = true;
                user.LastLoginDate = DateTime.Now;
                user.FailedLoginAttempts = 0;
                user.IsLockedOut = false;
                user.LockedOutDate = new DateTime();
                _userRepository.Update(user);
                _userRepository.Commit();


                return Ok(new
                {
                    token = token
                });
            }
            catch (Exception ex)
            {
                _loggerHelper.AddLog(HttpContext, loginDTO);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [CustomAuth(groupIds: [(int)UserGroups.Provider, (int)UserGroups.Admin, (int)UserGroups.-+Member, (int)UserGroups.SuperAdmin])]
        public async Task<ActionResult> GetUserInfo()
        {
            try
            {
                int groupId = (int)HttpContext.Items["GroupID"];
                string userId = (string)HttpContext.Items["UserId"];
                var userInfo = await _userRepository.GetUserProfile(userId, groupId);
                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                await _loggerHelper.AddLog(HttpContext);
                return BadRequest(ex.Message);
            }
        }

        private void IncrementFailedLoginAttempts(User user)
        {
            // note that the No. of max faild Login Attempts is 5
            if (user != null)
            {
                user.FailedLoginAttempts++;
                if (user.FailedLoginAttempts >= 5)
                {
                    // Lock out the user
                    user.IsLockedOut = true;
                    user.IsActive = false;
                    user.LockedOutDate = DateTime.Now.AddMinutes(30);
                }
                _userRepository.Update(user);
                _userRepository.Commit();
            }
        }


        private void ResetFailedLoginAttempts(User user)
        {

            if (user != null)
            {
                user.FailedLoginAttempts = 0;
                user.IsLockedOut = false;
                user.LockedOutDate = new DateTime(0, 0, 0);
                _userRepository.Update(user);
                _userRepository.Commit();
            }
        }


    }
}
