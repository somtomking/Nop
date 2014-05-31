using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Models
{
    public class PromptModel : BaseNopModel
    {
        public PromptModel()
        {
            TodayPromptProductList = new List<PromptProduct>();
            TomorrowPromptProductList = new List<PromptProduct>();
            FuturePromptProductList = new List<PromptProduct>();
        }

        public int CustomerId { set; get; }
        public int ActivityId { set; get; }
        public bool IsToday { set; get; }
        public bool IsTomorrow { set; get; }
        public bool IsFuture { set; get; }
        public IList<PromptProduct> TodayPromptProductList { set; get; }
        public IList<PromptProduct> TomorrowPromptProductList { set; get; }
        public IList<PromptProduct> FuturePromptProductList { set; get; }
        public bool PhoneEnabled { set; get; }
        public bool EmailEnabled { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
    }

    public class PromptProduct : BaseNopModel
    {
        public int ProductId { set; get; }
        //离开始时间和离结束时间
        public string Data { set; get; }
        public bool IsStrat { set; get; }
        public bool DisableOldPrice { set; get; }
        public DateTime StartTimeUtc { set; get; }
        public DateTime EndTimeUtc { set; get; }
        public string ProductPictureUrl { set; get; }
        public string ProductName { set; get; }
        public string ProductSename { set; get; }
        public string ProductDesc { set; get; }
        public string Price { set; get; }
        public string OldPrice { set; get; }
        public int DisplayOrder { set; get; }
        //活动价格
        public string SpecialPrice { set; get; }
        //促销剩余比例
        public double RemainingSales  { set; get; }
        //折扣
        public string Discount { set; get; }
        //已购买人数
        public int BuyedCount { set; get; }
        //客户购买最大数量
        public int CustomerMaximumQuantity { set; get; }
        /// <summary>
        /// 促销单的价格
        /// </summary>
        public string PcPlatformPrice { set; get; }

        public bool IsSoldOut { get; set; }
    }

    public class RemindModel : BaseNopModel
    {
        [NopResourceDisplayName("Remind.Fields.CustomerId")]
        [AllowHtml]
        public int CustomerId { get; set; }

        [NopResourceDisplayName("Remind.Fields.ProductId")]
        [AllowHtml]
        public int ProductId { get; set; }

        [NopResourceDisplayName("Remind.Fields.ActivityId")]
        [AllowHtml]
        public int ActivityId { get; set; }

        [NopResourceDisplayName("Remind.Fields.Phone")]
        [AllowHtml]
        public string Phone { get; set; }

        [NopResourceDisplayName("Remind.Fields.Email")]
        [AllowHtml]
        public string Email { get; set; }
    }
}