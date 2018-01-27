using Data.Entities;
using Data.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IServices
{ 
    public interface IStorageService
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="CreateUser"></param>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <param name="fileType"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        IPagedList<Storage> GetAll(string CreateUser, DateTime timeStart, DateTime timeEnd,  int pageIndex);




        /// <summary>
        /// 上传库存文件
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Request"></param>
        /// <param name="filetype"></param>
        /// <returns></returns>
        RepResult<Storage> UploadStorage(string User, HttpRequestBase Request);

        bool Delete(int id);

    }
}
