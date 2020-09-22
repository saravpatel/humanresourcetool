using HRTool.CommanMethods.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Settings
{
    public class CurrencyConverterModel
    {

        public CurrencyConverterModel()
        {
            BindCurrencyList = new List<SelectListItem>();
            CurrencyList = new List<CurrencyConverterModel>();
        }
        public IList<SelectListItem> BindCurrencyList { get; set; }
        public string SelectedFromCurrency { get; set; }
        public string SelectedToCurrency { get; set; }
        public double FromAmount { get; set; }
        public double ToAmount { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Nullable<decimal> FixedRate { get; set; }
        public Nullable<decimal> LiveRate { get; set; }
        public Nullable<System.DateTime> FreezingDate { get; set; }
        public IList<CurrencyConverterModel> CurrencyList { get; set; }

        public bool IsFixed { get; set; }

    }



}