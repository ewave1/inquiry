using Data.Entities;
using Data.Models;
using PagedList;
using SmartSSO.Entities;
using SmartSSO.Services.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public  interface IInquiryService
    {
        InquiryModelRequest Get(int ?id);

        IPagedList<InquiryLog> GetAll(ManageUser user,string CreateUser,DateTime timeStart,DateTime timeEnd,int pageIndex);

        RepResult<InquiryLog> Create(InquiryModelRequest model,string User);

        bool Delete(int id);

        RepResult<bool> RemoveData(DateTime start, DateTime end);

        /// <summary>
        /// 工厂
        /// </summary>
        /// <returns></returns>
        List<DiscountSet> GetDiscountNames(DisCountType type);

        /// <summary>
        /// 获取粟
        /// </summary>
        /// <returns></returns>
        List<Material> Materials();

        /// <summary>
        /// 获取所有编码
        /// </summary>
        /// <returns></returns>
        List<SealCode> SealCodes();

        /// <summary>
        /// 获取编码
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="SizeA"></param>
        /// <param name="SizeB"></param>
        /// <returns></returns>
        SealCode GetSealCode(string Code, decimal? SizeA, decimal? SizeB);
    }
}
