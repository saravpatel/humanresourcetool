using HRTool.CommanMethods.Settings;
using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HRTool.CommanMethods;
using System.Web.Script.Serialization;

namespace HRTool.Controllers
{
    [CustomAuthorize]
    public class AddAssetsController : Controller
    {

        #region Const
        AddAssetsMethod AddAssets = new AddAssetsMethod();
        OtherSettingMethod _otherSettingMethod = new OtherSettingMethod();
        #endregion
        //
        // GET: /AddAssets/
        public ActionResult Index()
        {
            return View();
        }
        public List<AddAssetsViewModel> returnList()
        {
            List<Asset> data = AddAssets.getAllList();
            List<AddAssetsViewModel> model = new List<AddAssetsViewModel>();
            string FilePath = ConfigurationManager.AppSettings["AssetsFilePath"].ToString();
            foreach (var item in data)
            {
                AddAssetsViewModel AssetList = new AddAssetsViewModel();
                AssetList.Id = item.Id;
                AssetList.Name = item.Name;
                AssetList.AssetsTypeName_1 = AddAssets.GetAssetType1Name(Convert.ToInt32(item.AssetType)).Value;
                AssetList.AssetsTypeName_2 = AddAssets.GetAssetType2Name(Convert.ToInt32(item.AssetType2)).Value;
                var Name = AddAssets.GetOwnerName(Convert.ToInt32(item.CmpCustomerID));
                var NameOwner = string.Format("{0} {1}", Name.FirstName, Name.LastName);
                AssetList.OwnerName = NameOwner;
                if (!string.IsNullOrEmpty(item.PhotoPath))
                {
                    AssetList.Picture = item.PhotoPath;
                }
                model.Add(AssetList);
            }
            return model;
        }
        public ActionResult List()
        {
            List<AddAssetsViewModel> model = returnList();
            return PartialView("_PartialAddAssetsList", model);
        }
        public ActionResult CreateAssestRecord(int Id, string Name, int Assets1, int Assets2, int OwnerId)
        {
            try
            {
                Asset AssetsUpdate = new Asset();
                string FilePath = string.Empty;
                string fileName = string.Empty;
                if (Request.Files.Count > 0)
                {
                    FilePath = ConfigurationManager.AppSettings["AssetsFilePath"].ToString();
                    HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                    fileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(hpf.FileName), DateTime.Now.ToString("ddMMyyyyhhmmss"), Path.GetExtension(hpf.FileName));
                    string path = Path.Combine(HttpContext.Server.MapPath(FilePath), fileName);
                    hpf.SaveAs(path);
                }
                AddAssets.SaveAssets(Id, Name, Assets1, Assets2, fileName, SessionProxy.UserId, OwnerId);
                List<AddAssetsViewModel> model = returnList();
                return PartialView("_PartialAddAssetsList", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult AddEditAssetSkillsSet(int Id)
        {
            string FilePath = ConfigurationManager.AppSettings["AssetsFilePath"].ToString();
            AddAssetsViewModel model = new AddAssetsViewModel();
            model.Id = Id;
            if (Id > 0)
            {
                var AssetsType_1 = _otherSettingMethod.getAllSystemValueListByKeyName("Asset Type List");
                foreach (var item in AssetsType_1)
                {
                    var SelectassetId = AddAssets.GetListAssetType1ById(Id);
                    if (item.Id == SelectassetId)
                    {
                        model.AssetsType_1_List.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        model.AssetsType_1_List.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                    }
                }

                var AssetsType_2 = _otherSettingMethod.getAllSystemValueListByKeyName("Asset Type 2 List");
                foreach (var item in AssetsType_2)
                {
                    var SelectassetIdAsset = AddAssets.GetListAssetType2ById(Id);
                    if (item.Id == SelectassetIdAsset)
                    {
                        model.AssetsType_2_List.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
                    }
                    else
                    {
                        model.AssetsType_2_List.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                    }
                }

                var AssetsOwnerList = AddAssets.BindAssetsOwnerList();
                foreach (var item in AssetsOwnerList)
                {
                    var SelectassetIdowner = AddAssets.GetOwnerName(Convert.ToInt32(item.Value));
                    if ((Convert.ToInt32(item.Value)) == SelectassetIdowner.Id)
                    {
                        model.AssetsOwnerList.Add(new SelectListItem() { Text = @item.Text, Value = @item.Value.ToString(), Selected = true });
                    }
                    else
                    {
                        model.AssetsOwnerList.Add(new SelectListItem() { Text = @item.Text, Value = @item.Value.ToString() });
                    }

                }
                var Assets = AddAssets.getAssetByIdList(Id);
                model.Name = Assets.Name;
                if (!string.IsNullOrEmpty(Assets.PhotoPath))
                {
                    model.Picture = Assets.PhotoPath;
                }
            }
            else
            {
                List<AddAssetsViewModel> modelList = new List<AddAssetsViewModel>();
                var AssetsType_1 = _otherSettingMethod.getAllSystemValueListByKeyName("Asset Type List");
                model.AssetsTypeId_1 = _otherSettingMethod.getSystemListId("Asset Type List");
                model.AssetsTypeId_2 = _otherSettingMethod.getSystemListId("Asset Type 2 List");
                foreach (var item in AssetsType_1)
                {
                    model.AssetsType_1_List.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }
                var AssetsType_2 = _otherSettingMethod.getAllSystemValueListByKeyName("Asset Type 2 List");
                foreach (var item in AssetsType_2)
                {
                    model.AssetsType_2_List.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }
                var AssetsOwnerList = AddAssets.BindAssetsOwnerList();
                foreach (var item in AssetsOwnerList)
                {
                    model.AssetsOwnerList.Add(new SelectListItem() { Text = @item.Text, Value = @item.Value.ToString() });
                }
            }
            return PartialView("_PartialAddAssets", model);
        }
        public ActionResult AddEditOtherSetting(int Id)
        {
            OtherSettingViewModel model = new OtherSettingViewModel();
            model.Id = Id;
            if (Id > 0)
            {
                var SystemLists = _otherSettingMethod.getSystemListById(Id);
                var SystemListValueList = _otherSettingMethod.getAllSystemValueListByNameId(Id);
                model.SystemListName = SystemLists.SystemListName;
                model.Archived = (bool)SystemLists.Archived;
                foreach (var item in SystemListValueList)
                {
                    OtherSettingValueViewModel otherSettingValueViewModel = new OtherSettingValueViewModel();
                    otherSettingValueViewModel.Id = item.Id;
                    otherSettingValueViewModel.SystemListID = item.SystemListID;
                    otherSettingValueViewModel.Value = item.Value;
                    otherSettingValueViewModel.Archived = (bool)item.Archived;
                    model.valueList.Add(otherSettingValueViewModel);
                }
            }
            return PartialView("_partialAddOtherSetting", model);
        }

        public ActionResult saveOtherSetting(int Id, string ListName, string ListValue, bool Flag)
        {
            List<OtherSettingViewModel> model = null;
            if (Flag == false)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                if (!string.IsNullOrEmpty(ListValue))
                {
                    string[] listValueArray = js.Deserialize<string[]>(ListValue);
                    _otherSettingMethod.SaveData(Id, ListName, listValueArray, SessionProxy.UserId);
                }
            }
            else if (Flag == true)
            {
                //List<systemListData> ListData = new List<systemListData>();
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<systemListData> ListData = js.Deserialize<List<systemListData>>(ListValue);
                _otherSettingMethod.EditData(Id, ListName, ListData, SessionProxy.UserId);
            }
            string FilePath = ConfigurationManager.AppSettings["AssetsFilePath"].ToString();
            AddAssetsViewModel assmodel = new AddAssetsViewModel();
            assmodel.Id = Id;
            //if (Id > 0)
            //{
            //    var AssetsType_1 = _otherSettingMethod.getAllSystemValueListByKeyName("Asset Type List");
            //    foreach (var item in AssetsType_1)
            //    {
            //        //var SelectassetId = AddAssets.GetListAssetType1ById(Id);
            //        //if (item.Id == SelectassetId)
            //        //{
            //        //    assmodel.AssetsType_1_List.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
            //        //}
            //        //else
            //        //{
            //            assmodel.AssetsType_1_List.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            //        //}
            //    }

            //    var AssetsType_2 = _otherSettingMethod.getAllSystemValueListByKeyName("Asset Type 2 List");
            //    foreach (var item in AssetsType_2)
            //    {
            //        //var SelectassetIdAsset = AddAssets.GetListAssetType2ById(Id);
            //        //if (item.Id == SelectassetIdAsset)
            //        //{
            //        //    assmodel.AssetsType_2_List.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString(), Selected = true });
            //        //}
            //        //else
            //        //{
            //            assmodel.AssetsType_2_List.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
            //        //}
            //    }

            //    var AssetsOwnerList = AddAssets.BindAssetsOwnerList();
            //    foreach (var item in AssetsOwnerList)
            //    {
            //        var SelectassetIdowner = AddAssets.GetOwnerName(Convert.ToInt32(item.Value));
            //        if ((Convert.ToInt32(item.Value)) == SelectassetIdowner.Id)
            //        {
            //            assmodel.AssetsOwnerList.Add(new SelectListItem() { Text = @item.Text, Value = @item.Value.ToString(), Selected = true });
            //        }
            //        else
            //        {
            //            assmodel.AssetsOwnerList.Add(new SelectListItem() { Text = @item.Text, Value = @item.Value.ToString() });
            //        }

            //    }
            //    //var Assets = AddAssets.getAssetByIdList(Id);
            //    //assmodel.Name = Assets.Name;
            //    //if (!string.IsNullOrEmpty(Assets.PhotoPath))
            //    //{
            //    //    assmodel.Picture = Assets.PhotoPath;
            //    //}
            //}
            //else
            //{
                List<AddAssetsViewModel> modelList = new List<AddAssetsViewModel>();
                var AssetsType_1 = _otherSettingMethod.getAllSystemValueListByKeyName("Asset Type List");
                assmodel.AssetsTypeId_1 = _otherSettingMethod.getSystemListId("Asset Type List");
                assmodel.AssetsTypeId_2 = _otherSettingMethod.getSystemListId("Asset Type 2 List");
            foreach (var item in AssetsType_1)
                {
                    assmodel.AssetsType_1_List.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }
                var AssetsType_2 = _otherSettingMethod.getAllSystemValueListByKeyName("Asset Type 2 List");
                foreach (var item in AssetsType_2)
                {
                    assmodel.AssetsType_2_List.Add(new SelectListItem() { Text = @item.Value, Value = @item.Id.ToString() });
                }
                var AssetsOwnerList = AddAssets.BindAssetsOwnerList();
                foreach (var item in AssetsOwnerList)
                {
                    assmodel.AssetsOwnerList.Add(new SelectListItem() { Text = @item.Text, Value = @item.Value.ToString() });
                }
           // }
            return PartialView("_PartialAddAssets", assmodel);
        }


        public ActionResult DeleteAssetRecord(int Id)
        {
            AddAssets.DeleteAsset(Id);
            return RedirectToAction("Index");
        }
    }
}