using Common.JWT;
using DataAccess.Helper;
using DataAccess.IRepository;
using DataAccess.Repository;
using DTOs.Auth;
using DTOs.Common;
using eShop.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace eShop.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonController : ControllerBase
    {

        private readonly ILoggerHelper _loggerHelper;
        private readonly IResourcesRepository _resourcesRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IConfiguration  _configuration;
        public CommonController(
            ILoggerHelper loggerHelper,
            IResourcesRepository resourcesRepository, ILanguageRepository languageRepository, IConfiguration configuration)
        {

            _loggerHelper = loggerHelper;
            _resourcesRepository = resourcesRepository;
            _languageRepository = languageRepository;
            _configuration = configuration;
        }

        [HttpGet]
        [ResponseCache(Duration = 12 * 3600)]
        public async Task<ActionResult> GetResources(Guid langID, int ModuleID = 0)
        {
            try
            {
                var langsDict = new Dictionary<string, string>();

                var langs = await _resourcesRepository.getLangObject(langID, ModuleID);
                langs.ForEach(x =>
                {
                    langsDict.Add(x.key, x.value);
                });

                return Ok(langsDict);
            }
            catch (Exception ex)
            {
                await _loggerHelper.AddLog(HttpContext);
                return BadRequest();
            }
        }
        [HttpGet]
        [ResponseCache(Duration = 12 * 3600)]
        public async Task<ActionResult> GetLanguages()
        {
            try
            {

                var langs = _languageRepository.FindAllByCondition(x=> !x.IsDeleted);

                return Ok(langs);
            }
            catch (Exception ex)
            {
                await _loggerHelper.AddLog(HttpContext);
                return BadRequest();
            }
        }

        [HttpGet]
        [ResponseCache(Duration = 12 * 3600)]
        public async Task<ActionResult> GetConfig()
        {
            try
            {

                var config = _configuration.GetSection("eShopSettings").Get<ConfigDTO>();

                return Ok(config);
            }
            catch (Exception ex)
            {
                await _loggerHelper.AddLog(HttpContext);
                return BadRequest();
            }
        }

    }
}
