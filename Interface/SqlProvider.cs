using BlazorApp1.Connection;
using BlazorApp1.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace BlazorApp1.Interface
{
    public class SqlProvider : IDataProvider
    {
        private readonly PostgreSql.PostgreSqlContext _context;
        private readonly ILogger _logger;

        public SqlProvider(PostgreSql.PostgreSqlContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("SqlProvider");
        }

        public void AddUsersRecord(Users users)
        {
            _context.users.Add(users);
            _context.SaveChanges();
        }

        public void UpdateUsersRecord(Users users)
        {
            _context.users.Update(users); // Используйте Update для обновления записи
            _context.SaveChanges();
        }

        public void DeleteUsersRecord(int id)
        {
            var entity = _context.users.FirstOrDefault(x => x.id == id);
            if (entity != null)
            {
                _context.users.Remove(entity);
                _context.SaveChanges();
            }
        }

        public Users GetUsersSingleRecord(string id)
        {
            return _context.users.FirstOrDefault(x => id.ToString() == id);
        }

        public List<Users> GetUsersRecords()
        {
            return _context.users.ToList();
        }
    }
}
