using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using SmartSSO.Entities;
using SmartSSO.Models;
using SmartSSO.Services.Util;

namespace SmartSSO.Services
{
    public interface IUserManageService
    {
        PagedResult<ManageUser> GetAll();

        ManageUser Get(string username = "");

        bool Edit(ManageUser model);

        RepResult<ManageUser> Create(UserCreateRequest model);

        bool Delete(string username);

        bool ExistsUser(string username);
    }
}