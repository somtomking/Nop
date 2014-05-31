using System;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core;

namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    /// <summary>
    /// 限时抢购提醒表
    /// </summary>
    public class ActivityRemind : BaseEntity
    {
        public int CustomerId { get; set; }
        public int ActivityId { get; set; }
        public int ProductId { get; set; }
        public DateTime StartDateTimeUtc { get; set; }
        public DateTime EndDateTimeUtc { get; set; }
        public bool IsNeededEmail { get; set; }
        public bool IsNeededSms { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOnUtc { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Activity Activity { get; set; }
        public virtual Product Product { get; set; }
    }
}
