using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRTool.Models.Settings
{
    public class AddAssetsViewModel
    {
        public AddAssetsViewModel()
        {
            AssetsType_1_List = new List<SelectListItem>();
            AssetsType_2_List = new List<SelectListItem>();
            AssetsOwnerList = new List<SelectListItem>();
            AssetsList = new List<AddAssetsViewModel>();
        }
        public int Id { get; set; }

        public string Name { get; set; }
        public int AssetsTypeId_1 { get; set; }
        public int AssetsTypeId_2 { get; set; }
        public int OwnerId { get; set; }
        public string AssetsTypeName_1 { get; set; }
        public string AssetsTypeName_2 { get; set; }
        public string OwnerName { get; set; }
        public IList<SelectListItem> AssetsType_1_List { get; set; }
        public IList<SelectListItem> AssetsType_2_List { get; set; }
        public IList<SelectListItem> AssetsOwnerList { get; set; }
        public IList<AddAssetsViewModel> AssetsList { get; set; }
        public string Picture { get; set; }
    }
}