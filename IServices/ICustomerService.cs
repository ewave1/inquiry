using Data.Entities;
using Data.Models;
using SmartSSO.Services.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface ICustomerService
    { 
        PagedResult<Customer> GetAll( );

        CustomerModel Get(int? id);

        RepResult<Customer> Create(CustomerModel model, string User);

        bool Delete(int id);
    }
}
