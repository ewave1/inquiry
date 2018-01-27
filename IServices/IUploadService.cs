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
    public interface IUploadService
    {

        IPagedList<UploadFile> GetAll(string CreateUser, DateTime timeStart, DateTime timeEnd,FILETYPE fileType , int pageIndex);

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Request"></param>
        /// <param name="filetype"></param>
        /// <returns></returns>
        RepResult<UploadFile> UploadFile(string User, HttpRequestBase Request, FILETYPE filetype = FILETYPE.其它);


        bool Delete(int id);
    }
}
