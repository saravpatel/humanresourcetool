using HRTool.DataModel;
using HRTool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.CommanMethods.Settings
{
    public class AddAssetsMethod
    {
        #region const
        EvolutionEntities _db = new EvolutionEntities();
        #endregion
        public List<SelectListItem> BindAssetsOwnerList()
        {
            List<SelectListItem> model = new List<SelectListItem>();
            var CurrencyList = (from i in _db.Cmp_Customer
                                select i).ToList();
            foreach (var item in CurrencyList)
            {
                string OwnerName = string.Format("{0}  {1}", item.FirstName, item.LastName);
                model.Add(new SelectListItem { Text = OwnerName, Value = item.Id.ToString() });
            }
            return model;
        }

       


        public void SaveAssets(int Id, string Name, int Assets1, int Assets2, string ImahePath, int UserId, int OwnerId)
        {
            if (Id > 0)
            {
                Asset Assets = _db.Assets.Where(x => x.Id == Id).FirstOrDefault();
                Assets.Name = Name;
                Assets.AssetType = Assets1;
                Assets.AssetType2 = Assets2;
                Assets.CmpCustomerID = OwnerId;
                if (!string.IsNullOrEmpty(ImahePath))
                {
                    Assets.PhotoPath = ImahePath;
                }
                Assets.UserIDLastModifiedBy = UserId;
                Assets.LastModified = DateTime.Now;
                _db.SaveChanges();
            }
            else
            {
                Asset AssetsAdd = new Asset();
                AssetsAdd.Name = Name;
                AssetsAdd.AssetType = Assets1;
                AssetsAdd.AssetType2 = Assets2;
                AssetsAdd.CmpCustomerID = OwnerId;
                AssetsAdd.PhotoPath = ImahePath;
                AssetsAdd.UserIDCreatedBy = UserId;
                AssetsAdd.Archived = false;
                AssetsAdd.UserIDCreatedBy = UserId;
                AssetsAdd.CreatedDate = DateTime.Now;
                _db.Assets.Add(AssetsAdd);
                _db.SaveChanges();
            }
        }
        public List<Asset> getAllList()
        {
            return _db.Assets.Where(x=>x.Archived==false).ToList();
        }
        public SystemListValue GetAssetType1Name(int Id)
        {
            return _db.SystemListValues.Where(x => x.Id == Id).FirstOrDefault();
        }
        public SystemListValue GetAssetType2Name(int Id)
        {
            return _db.SystemListValues.Where(x => x.Id == Id).FirstOrDefault();
        }
        public Cmp_Customer GetOwnerName(int Id)
        {
            return _db.Cmp_Customer.Where(x => x.Id == Id).FirstOrDefault();
        }
        public Asset getAssetByIdList(int AssetId)
        {
            return _db.Assets.Where(x => x.Id == AssetId).FirstOrDefault();
        }
        public int GetListAssetType1ById(int AssetId)
        {
            var AassetIdrecord = _db.Assets.Where(x => x.Id == AssetId).FirstOrDefault();
            return (int)AassetIdrecord.AssetType;
        }
        public int GetListAssetType2ById(int AssetId)
        {
            var AassetIdrecordtype = _db.Assets.Where(x => x.Id == AssetId).FirstOrDefault();
            return (int)AassetIdrecordtype.AssetType2;
        }
        public void DeleteAsset(int Id)
        {
            Asset assets = _db.Assets.Where(x => x.Id == Id).FirstOrDefault();
            assets.Archived = true;
            assets.LastModified = DateTime.Now;
            assets.UserIDLastModifiedBy = SessionProxy.UserId;
            //_db.Assets.Remove(assets);
            _db.SaveChanges();
        }
    }
}