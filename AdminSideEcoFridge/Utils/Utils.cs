using AdminSideEcoFridge.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AdminSideEcoFridge.Models;

namespace AdminSideEcoFridge.Utils
{
    public enum ErrorCode
    {
        Success,
        Error
    }
    public class Utils
    {
        public static List<SelectListItem> SelectPlanType()
        {
            StoragePlanManager _storagMgr = new StoragePlanManager();
            var list = new List<SelectListItem>();

            foreach (var item in _storagMgr.ListOfPlans())
            {
                var r = new SelectListItem
                {
                    Text = item.StoragePlanName,
                    Value = item.StoragePlanId.ToString()
                };
                list.Add(r);
            }
            return list;
        }
    }
}
