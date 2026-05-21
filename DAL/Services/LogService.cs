using DAL.Models;
using Microsoft.EntityFrameworkCore;
using DAL.DTOs.Log;

namespace DAL.Services
{
    public class LogService
    {
        private readonly GrillPizzaOrdersContext _context;

        public LogService(GrillPizzaOrdersContext context)
        {
            _context = context;
        }

        // write out the log into the database
        public void LogAction(string level, string message)
        {
            var logLevel = _context.LogLevels.FirstOrDefault(x => x.Title == level);
            if (logLevel == null) throw new Exception("Non-existent log level");

            _context.Logs.Add(new Log
            {
                LogLevelId = logLevel.Id,
                Message = message
            });
            _context.SaveChanges();
        }

        // get the last N logs
        public List<LogResponseDto> GetLogs(int n)
            => _context.Logs
            .Include(x => x.LogLevel)
            .OrderByDescending(x => x.Timestamp)
            .Take(n)
            .Select(x => new LogResponseDto
            {
                Id = x.Id,
                Timestamp = x.Timestamp,
                Message = x.Message,
                LogLevelTitle = x.LogLevel.Title
            })
            .ToList();

        // get the number of Logs
        public int GetLogCount() => _context.Logs.Count();
    }
}
