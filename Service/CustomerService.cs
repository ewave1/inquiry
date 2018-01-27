using Data.Entities;
using Data.Models;
using IServices;
using SmartSSO.Services.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CustomerService : ServiceContext, ICustomerService
    {
        public PagedResult<Customer> GetAll()
        {
            return new PagedResult<Customer>
            {
                Result = DbContext.Customer.OrderByDescending(p => p.CreateTime).ToList()
            };
        }
        public RepResult<Customer> Create(CustomerModel model, string User)
        {
            var customer = DbContext.Customer.Where(v => v.Id == model.Id).FirstOrDefault();
            if (customer == null)
            {
                if (DbContext.Customer.Where(v => v.CompanyName == model.CompanyName).Count() > 0)
                {
                    return new RepResult<Customer> { Code = -1, Msg = "客户名称已经存在" };
                }

                customer = DbContext.Customer.Add(new Customer
                {
                    CompanyName = model.CompanyName,
                    ContactName = model.ContactName,
                    ContactMobile = model.ContactMobile,
                    CreateTime = DateTime.Now,
                    CreateUser = User,
                    CustomerLevel = model.CustomerLevel,
                    Remark = model.Remark,
                });
            }
            else
            {
                customer.CompanyName = model.CompanyName;
                customer.ContactMobile = model.ContactMobile;
                customer.ContactName = model.ContactName;
                customer.UpdateTime = DateTime.Now;
                customer.UpdateUser = User;
                customer.CustomerLevel = model.CustomerLevel;
                customer.Remark = model.Remark;

            }
            DbContext.SaveChanges();
            return new RepResult<Customer> { Data = customer };
        }

        public bool Delete(int id)
        {
            var model = DbContext.Customer.Find(id);
            if (model != null)
            {
                DbContext.Entry(model).State = EntityState.Deleted;
                return DbContext.SaveChanges() > 0;
            }
            return false;
        }

        public CustomerModel Get(int? id)
        {
            if(id!=null)
            { 
                var customer = DbContext.Customer.Find(id);
                return new CustomerModel {
                    CompanyName = customer.CompanyName,
                    ContactMobile = customer.ContactMobile,
                    ContactName = customer.ContactName,
                    CreateTime = customer.CreateTime,
                    CustomerLevel = customer.CustomerLevel,
                    Remark = customer.Remark,
                    Id = customer.Id
                };
            }
            return null;
        }
    }
}
