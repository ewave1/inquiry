using Data.Entities;
using Data.Models;
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
        PagedResult<InquiryLog> GetAll(ManageUser user);

        RepResult<InquiryLog> Create(InquiryModelRequest model,string User);

        bool Delete(int id);

        /// <summary>
        /// 工厂
        /// </summary>
        /// <returns></returns>
        List<DiscountSet> Factories();

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
