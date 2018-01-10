// 
// Copyright (c) 2014,SmartBooks
// All rights reserved.
// 
// 文件名称：UserManageService.cs
// 项目名称：WebManage
// 摘      要：简要描述本文件的内容
// 
// 当前版本：1.0
// 作      者：ya wang
// 完成日期：2014年05月14日
// 

using System;
using System.Data.Entity;
using System.Linq;
using SmartSSO.Entities;
using SmartSSO.Models;
using SmartSSO.Services.Util;
using SmartSSO.Extendsions;
using Data.Models;

namespace SmartSSO.Services.Impl
{
    public sealed class UserManageService : ServiceContext, IUserManageService
    {
        public PagedResult<ManageUser> GetAll()
        {
            var result = new PagedResult<ManageUser>
            {
                Result = DbContext.ManageUser.OrderByDescending(p => p.CreateTime).ToList()
            };

            return result;
        }

        public RepResult<ManageUser> Create(UserCreateRequest req)
        {
            var userPwdHash = req.Password?.ToMd5();
            var model = new ManageUser
            {
                UserName = req.UserName,
                UserPwd = userPwdHash,
                Nick = req.Nick,
                Discount = req.Discount,
                CreateTime = DateTime.Now,
                IsAdmin = req.IsAdmin
            };
            if (!ExistsUser(req.UserName))
            {
                DbContext.ManageUser.Add(model);
            }
            else if (!req.IsAdd)
            {
                model = DbContext.ManageUser.Find(req.UserName);
                if (!string.IsNullOrEmpty(userPwdHash))
                    model.UserPwd = userPwdHash;
                model.Discount = req.Discount;
                model.IsAdmin = req.IsAdmin;
            }
            else
                return new RepResult<ManageUser> { Code = -1, Msg = "登录账号已存在"};
            DbContext.SaveChanges();

            return new RepResult<ManageUser> { Data = model};
        }

        public bool ExistsUser(string username)
        {
            return DbContext.ManageUser.FirstOrDefault(p => p.UserName == username) != null;
        }

        public ManageUser Get(string username = "")
        {
            return DbContext.ManageUser.FirstOrDefault(p => p.UserName == username);
        }

        public bool Edit(ManageUser model)
        {
            DbContext.Entry(model).State = EntityState.Modified;
            return DbContext.SaveChanges() > 0;
        }

        public bool Delete(string username)
        {
            var model = Get(username);
            if (model != null)
            {
                DbContext.Entry(model).State = EntityState.Deleted;
                return DbContext.SaveChanges() > 0;
            }
            return false;
        }
    }
}