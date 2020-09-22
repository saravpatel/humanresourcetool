using ClosedXML.Excel;
using HRTool.CommanMethods;
using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class CurrencyConverterController : Controller
    {

        #region const
        CurrencyConverterMethod CurrencyMethod = new CurrencyConverterMethod();
        EvolutionEntities _db = new EvolutionEntities();
        #endregion
        // GET: CurrencyConverter
        public ActionResult Index()
        {
            CurrencyConverterModel model = new CurrencyConverterModel();
            model.BindCurrencyList = CurrencyMethod.BindCurrencyListRecord();
            var CurrencyList = CurrencyMethod.GetCurrencyListRecord();
            foreach (var item in CurrencyList)
            {
                CurrencyConverterModel m = new CurrencyConverterModel();
                m.Id = item.Id;
                m.Name = item.Name;
                m.Code = item.Code;
                m.FixedRate = Convert.ToDecimal(Math.Round(decimal.Parse(item.FixedRate.ToString()), 4).ToString());
                m.LiveRate = Convert.ToDecimal(Math.Round(decimal.Parse(item.LiveRate.ToString()), 4).ToString());
                m.FreezingDate = item.FreezingDate;
                m.IsFixed = (bool)item.IsFixed;
                model.CurrencyList.Add(m);
            }
            return View(model);
        }
        private Currency GetBaseCurrencyFromSettings()
        {
            var baseCurrency = _db.Currencies.Where(x => x.IsFixed == true).FirstOrDefault();
            int baseCurrencyi = 2;
            try
            {
                baseCurrencyi = baseCurrency.Id;
            }
            catch (Exception)
            {
                baseCurrencyi = 2;
            }
            //   var settings = _ApplicationSettingService.GetCompanySettings();
            var currency = _db.Currencies.Where(x => x.Id == baseCurrencyi).FirstOrDefault();
            return currency;
        }
        public ActionResult ApplyLiveRates()
        {
            var list = _db.Currencies.ToList();
            var currency = GetBaseCurrencyFromSettings();
            if (currency != null)
                CopyLiveRatesToFixedRate(list);
            return RedirectToAction("Index");
        }
        public ActionResult ConvertCurrency(CurrencyConverterModel model)
        {
            string currencyUrl = "http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";
            string rate = "";
            string incurrcodetoeuro = model.SelectedFromCurrency;
            string outcurrcodetoeuro = model.SelectedToCurrency;
            string incurrcode = incurrcodetoeuro;
            string outcurrcode = outcurrcodetoeuro;
            try
            {
                if (model.FromAmount == 0)
                {
                    model.ToAmount = 0;
                }
                else
                {
                    // If input currency code and output currency code are same then return
                    // currency value as output currency value
                    if (incurrcode.Equals(outcurrcode))
                    {
                        model.ToAmount = model.FromAmount;
                    }
                    else
                    {
                        //Fetch Currency Feed XML File
                        XDocument xmlDoc = XDocument.Load(currencyUrl);
                        foreach (XElement element in xmlDoc.Root.Descendants())
                        {
                            XName name = element.Name;
                            if (name.LocalName == "Cube")
                            {
                                foreach (XElement elem in element.DescendantNodes())
                                {
                                    //string time = elem.Attribute("time").Value;
                                    foreach (XElement element1 in elem.DescendantNodes())
                                    {
                                        if (element1.Attribute("currency").Value.Equals(incurrcode))
                                        {
                                            // the value of 1 equivalent euro to input currency
                                            //ex input currency code as "USD", 1 EURO = 1.36 USD
                                            // then, incurrcodetoeuro = 1.36
                                            incurrcodetoeuro = element1.LastAttribute.Value.ToString();
                                        }
                                        else if (element1.Attribute("currency").Value.Equals(outcurrcode))
                                        {
                                            // the value of 1 equivalent euro 
                                            // ex output currency code as "USD", 1 EURO = 1.36 USD
                                            // then, outcurrcodetoeuro = 1.36
                                            outcurrcodetoeuro = element1.LastAttribute.Value.ToString();
                                        }
                                    }
                                }
                            }
                        }
                        // Since the base currency is euro, return outcurrcodetoeuro
                        // if the input currency code is equal to "EUR" (Euro)
                        if (incurrcode.Equals("EUR"))
                        {
                            rate = outcurrcodetoeuro;
                            Double currVal = double.Parse(rate) * model.FromAmount;
                            model.ToAmount = currVal;
                        }
                        // If output currency code is "EUR
                        // return value = (1/incurrcodetoeuro) * the value to be converted
                        else if (outcurrcode.Equals("EUR"))
                        {
                            rate = incurrcodetoeuro;
                            Double currVal = (1 / double.Parse(rate)) * model.FromAmount;
                            model.ToAmount = currVal;
                        }
                        // return value = 1/incurrcodetoeuro  outcurrcodetoeuro  the value to be converted
                        else
                        {
                            Double fromVal = double.Parse(incurrcodetoeuro);
                            Double toVal = double.Parse(outcurrcodetoeuro);
                            Double baseResult = (1 / fromVal);
                            Double currVal = baseResult * toVal * model.FromAmount;
                            model.ToAmount = currVal;
                        }
                    }
                }
                //Return coverted currency value rounded to 4 decimals
                model.ToAmount = Math.Round(model.ToAmount, 4);
                return Json(model.ToAmount, JsonRequestBehavior.AllowGet);
            }
            catch (FormatException fex)
            {
                return Json(model.ToAmount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(model.ToAmount, JsonRequestBehavior.AllowGet);
            }
        }
        private void FillLiveRates(List<Currency> list, string baseCurrencyCode, bool applyLiveRates = false)
        {
            string currencyUrl = "http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";
            string rate = "";
            string incurrcodetoeuro = baseCurrencyCode;

            foreach (var model in list)
            {
                string outcurrcodetoeuro = model.Code;
                string incurrcode = incurrcodetoeuro;
                string outcurrcode = outcurrcodetoeuro;
                try
                {

                    // If input currency code and output currency code are same then return
                    // currency value as output currency value
                    if (incurrcode.Equals(outcurrcode))
                    {
                        model.LiveRate = 1;
                    }
                    else
                    {
                        //Fetch Currency Feed XML File
                        XDocument xmlDoc = XDocument.Load(currencyUrl);
                        foreach (XElement element in xmlDoc.Root.Descendants())
                        {
                            XName name = element.Name;
                            if (name.LocalName == "Cube")
                            {
                                foreach (XElement elem in element.DescendantNodes())
                                {
                                    //string time = elem.Attribute("time").Value;
                                    foreach (XElement element1 in elem.DescendantNodes())
                                    {
                                        if (element1.Attribute("currency").Value.Equals(incurrcode))
                                        {
                                            // the value of 1 equivalent euro to input currency
                                            //ex input currency code as "USD", 1 EURO = 1.36 USD
                                            // then, incurrcodetoeuro = 1.36
                                            incurrcodetoeuro = element1.LastAttribute.Value.ToString();
                                        }
                                        else if (element1.Attribute("currency").Value.Equals(outcurrcode))
                                        {
                                            // the value of 1 equivalent euro 
                                            // ex output currency code as "USD", 1 EURO = 1.36 USD
                                            // then, outcurrcodetoeuro = 1.36
                                            outcurrcodetoeuro = element1.LastAttribute.Value.ToString();
                                        }

                                    }

                                }
                            }

                        }

                        // Since the base currency is euro, return outcurrcodetoeuro
                        // if the input currency code is equal to "EUR" (Euro)

                        if (incurrcode.Equals("EUR"))
                        {
                            rate = outcurrcodetoeuro;
                            Double currVal = double.Parse(rate);
                            model.LiveRate = (decimal)currVal;
                        }

                        // If output currency code is "EUR
                        // return value = (1/incurrcodetoeuro) * the value to be converted

                        else if (outcurrcode.Equals("EUR"))
                        {
                            rate = incurrcodetoeuro;
                            Double currVal = (1 / double.Parse(rate));
                            model.LiveRate = (decimal)currVal;

                        }

                        // return value = 1/incurrcodetoeuro * outcurrcodetoeuro * the value to be converted

                        else
                        {
                            Double fromVal = double.Parse(incurrcodetoeuro);
                            Double toVal = double.Parse(outcurrcodetoeuro);
                            Double baseResult = (1 / fromVal);
                            Double currVal = baseResult * toVal;
                            model.LiveRate = (decimal)currVal;
                        }
                    }
                    //Return coverted currency value rounded to 4 decimals
                    model.LiveRate = Math.Round(model.LiveRate ?? 0, 4);
                    // return Ok(model.LiveRate); 
                    if (applyLiveRates)
                    {
                        model.FixedRate = model.LiveRate;
                        model.LastModified = DateTime.Now;
                    }
                }
                catch (FormatException fex)
                {
                    //return Ok(model.ToAmount);
                }
                catch (Exception ex)
                {
                    // return Ok(model.ToAmount);
                }
            }
        }
        private void CopyLiveRatesToFixedRate(List<Currency> list)
        {
            foreach (var item in list)
            {
                Currency listdata = _db.Currencies.Where(x => x.Id == item.Id).FirstOrDefault();
                listdata.FixedRate = listdata.LiveRate;
                listdata.FreezingDate = DateTime.Now;
                _db.SaveChanges();
            }
        }
        public ActionResult ChangeCurrency(int Id, double FixedRate, bool IsFixed)
        {
            var list = _db.Currencies.Where(x => x.Id == Id).FirstOrDefault();
            if (list != null)
            {
                list.FixedRate = (decimal)FixedRate;
                list.IsFixed = IsFixed;
                list.FreezingDate = DateTime.Now;
                _db.SaveChanges();
            }
            return Json(Id, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ExportCurrencyList()
        {
            CurrencyConverterModel model = new CurrencyConverterModel();
            var CurrencyList = CurrencyMethod.GetCurrencyListRecord();
            DataTable dt = new DataTable("Currency");
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Code", typeof(string));
            dt.Columns.Add("Fixed Rate", typeof(decimal));
            dt.Columns.Add("Date Freezing", typeof(DateTime));
            dt.Columns.Add("Live Rate", typeof(decimal));
            dt.Columns.Add("Fixed?", typeof(string));
            foreach (var item in CurrencyList)
            {
                List<string> lstStrRow = new List<string>();
                lstStrRow.Add(item.Name);
                lstStrRow.Add(item.Code);
                lstStrRow.Add(Convert.ToDecimal(Math.Round(decimal.Parse(item.FixedRate.ToString()), 4).ToString()).ToString());
                lstStrRow.Add(item.FreezingDate.ToString());
                lstStrRow.Add(Convert.ToDecimal(Math.Round(decimal.Parse(item.LiveRate.ToString()), 4).ToString()).ToString());
                lstStrRow.Add((bool)item.IsFixed ? "Yes" : "No");
                string[] newArray = lstStrRow.ToArray();
                dt.Rows.Add(newArray);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= CurrencyReports_" + DateTime.Now.ToShortDateString() + ".xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return View();
        }
    }
}