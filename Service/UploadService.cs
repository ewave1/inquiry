using Common;
using Data.Entities;
using Data.Models;
using IServices;
using PagedList;
using SmartSSO.Services.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Services
{
    public class UploadService: ServiceContext,IUploadService
    {

        public IPagedList<UploadFile> GetAll(string CreateUser, DateTime timeStart, DateTime timeEnd, FILETYPE fileType, int pageIndex)
        {
            return   DbContext.UploadFile.Where(v => v.UpdateTime >= timeStart && v.UpdateTime < timeEnd && (v.FileType == fileType || fileType == FILETYPE.None))
                .OrderByDescending(v => v.UpdateTime)
                .ToPagedList(pageIndex, Const.PageSize); 
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public RepResult<UploadFile> UploadFile(string User, HttpRequestBase Request,FILETYPE filetype= FILETYPE.其它)
        {
            HttpPostedFileBase Upfile = Request.Files["file"];
            if (Upfile == null)
                return new RepResult<Data.Entities.UploadFile> { Msg ="请先选择上传的文件",Code = -2};
            string filename = Path.GetFileName(Upfile.FileName);
            string fileExtension = Path.GetExtension(filename);//文件扩展名
            string NotExtension = Path.GetFileNameWithoutExtension(filename);//获取无扩展名
            string FileName = NotExtension + "-" + DateTime.Now.ToString("yyyyMMdd") + "-" + Guid.NewGuid() + fileExtension;
            var strSavePath = Request.MapPath("~/Content/Upload/ToExecl");

            if (!Directory.Exists(strSavePath))
            {
                Directory.CreateDirectory(strSavePath);
            }
            var path = Path.Combine(strSavePath, FileName);
            Upfile.SaveAs(path);

            UploadFile uploadfile = new Data.Entities.UploadFile {
                FileName = filename,
                FileType =filetype ,
                FileUrl ="http://"+ Request.Url.Authority+ "/Content/Upload/ToExecl/"+ FileName,
                LocalPath = path ,
                UpdateTime = DateTime.Now,
                UpdateUser = User,
            };
            DbContext.UploadFile.Add(uploadfile);
            DbContext.SaveChanges();

            return new RepResult<Data.Entities.UploadFile> { Data = uploadfile};
        }


        public bool Delete(int id)
        {
            var model = DbContext.UploadFile.Find(id);
            if (model != null)
            {
                DbContext.Entry(model).State = EntityState.Deleted;
                return DbContext.SaveChanges() > 0;
            }
            return false;
        }
    }
}
