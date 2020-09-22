using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace HRTool.CommanMethods.Settings
{
    public class CurrencyConverterMethod
    {
        #region const
        EvolutionEntities _db = new EvolutionEntities();
        #endregion
        public List<SelectListItem> BindCurrencyListRecord()
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var CurrencyList = (from i in _db.Currencies
                                select i).ToList();
            foreach (var item in CurrencyList)
            {
                if (item.IsFixed == true)
                {
                    model.Add(new SelectListItem { Text = item.Name, Value = item.Code.ToString(), Selected = true });
                }
                else
                {
                    model.Add(new SelectListItem { Text = item.Name, Value = item.Code.ToString() });
                }
            }
            return model;
        }

        public List<Currency> GetCurrencyListRecord()
        {
            return _db.Currencies.ToList();
        }
    }


}