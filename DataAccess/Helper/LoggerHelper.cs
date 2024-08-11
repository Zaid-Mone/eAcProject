using DataAccess.Context;
using DTOs.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DataAccess.Helper
{
    public class LoggerHelper : ILoggerHelper
    {
        private readonly ILogger _logger;
        private readonly eShopContext _context;
        public LoggerHelper(ILogger<LoggerHelper> logger, eShopContext context)
        {
            this._logger = logger;
            _context = context;
        }

        public async Task AddLog(HttpContext http, dynamic obj = null)
        {
            try
            {
                var logger = new WebLog();
                logger.CreationDate = DateTime.Now;
                logger.Controller = http.Request.Path.Value;
                logger.Method = http.Request.Method;
                logger.UserID = http.Items["UserId"] as string;
                logger.Token = http.Items["Token"] as string;
                logger.HttpRequestType = (int)Enum.Parse(typeof(HttpRequestType), http.Request.Method);
                if (http.Request.Method == "GET")
                {
                    logger.Params = "";
                }
                else
                {
                    logger.Params = JsonConvert.SerializeObject(obj);
                }

                await _context.tblWebLogs.AddAsync(logger);
            }
            catch (Exception)
            {

                throw;
            }


        }
    }

    public interface ILoggerHelper {
        Task AddLog(HttpContext http, dynamic obj = null);

    }
}
