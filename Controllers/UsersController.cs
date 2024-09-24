using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp1.Models;
using BlazorApp1.Interface;

namespace BlazorApp1.Controllers
{
    public class UsersController : Controller
    {
        private readonly IDataProvider _dataProvider;

        public UsersController(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        [HttpGet]
        [Route ("api/Users/Get")]
        public IEnumerable<Users> Get()
        {
            return _dataProvider.GetUsersRecords();
        }

        [HttpGet]
        [Route("api/Users/Create")]
        public void Create([FromBody] Users users)
        {
            if (ModelState.IsValid)
            {
                Guid obj = Guid.NewGuid();
                //users.id = obj.ToInt();
                _dataProvider.AddUsersRecord(users);
            }
        }

        [HttpGet]
        [Route ("api/Users/Details/{id}")]
        public Users Details(string id)
        {
            return _dataProvider.GetUsersSingleRecord(id);
        }

        [HttpGet]
        [Route("api/Users/Edit")]
        public void Edit([FromBody] Users users)
        {
            if (ModelState.IsValid)
            {
                _dataProvider.AddUsersRecord(users);
            }
        }

        [HttpGet]
        [Route("api/Users/Delete/{id}")]
        public void DeleteConfirmed(int id)
        {
            _dataProvider.DeleteUsersRecord(id);
        }
    }
}
