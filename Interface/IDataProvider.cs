using System.Collections.Generic;
using BlazorApp1.Models;
namespace BlazorApp1.Interface
{
    public interface IDataProvider
    {
        void AddUsersRecord(Users users);
        void UpdateUsersRecord(Users users);
        void DeleteUsersRecord(int id);
        Users GetUsersSingleRecord(string id);
        List<Users> GetUsersRecords();
    }
}
