namespace Nop.Plugin.Widgets.ProductSpecialSale.Domain
{
    public enum ActivityType
    {
        /// <summary>
        ///     普通类型活动
        /// </summary>
        Common = 0,

        /// <summary>
        ///     限时抢购活动
        /// </summary>
        BuyLimit = 1,

        /// <summary>
        ///     每日优选活动
        /// </summary>
        DailyPreferably = 2,

        /// <summary>
        ///     新品首发活动
        /// </summary>
        NewStarter = 3,
    }
}