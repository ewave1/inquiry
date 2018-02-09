﻿using InquiryDemo.Filters;
using System.Web;
using System.Web.Mvc;

namespace WY.SSO
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionLogAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
