using System;
using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Catalog;

using Nop.Core.Domain.Common;

using G = Nop.Plugin.Widgets.CustomProductGroup.Domain;
using Nop.Plugin.Widgets.CustomProductGroup.Domain;
namespace Nop.Plugin.Widgets.CustomProductGroup.Services
{
    public interface ICustomProductGroupService
    {
        #region CustomProductGroup

        void InsertCustomProductGroup(G.CustomProductGroup customProductGroup);

        void UpdateCustomProductGroup(G.CustomProductGroup customProductGroup);

        void DeleteCustomProductGroup(G.CustomProductGroup customProductGroup);

        IList<G.CustomProductGroup> GetAllCustomProductGroups(Customer customer = null, PlantformType plantformType = PlantformType.Default, bool onlyShowEnable = false);

        G.CustomProductGroup GetCustomProductGroupById(int customProductGroupId);

        void SetCustomerCustomProductGroup(Customer customer, string customProductGroupIds);

        #endregion

        #region CustomProductGroupItem

        void InsertCustomProductGroupItem(G.CustomProductGroupItem customProductGroupItem);

        void UpdateCustomProductGroupItem(G.CustomProductGroupItem customProductGroupItem);

        void DeleteCustomProductGroupItem(G.CustomProductGroupItem customProductGroupItem);

        void DeleteCustomProductGroupItems(int customProductGroupId);

        G.CustomProductGroupItem GetCustomProductGroupItemById(int customProductGroupItemId);

        IList<G.CustomProductGroupItem> GetCustomProductGroupItems(
            int customProductGroupId,
            int count,
            PlantformType plantform = PlantformType.Default);

        IPagedList<Product> GetCustomProductGroupItems(
            int customProductGroupId,
            int pageIndex = 0,
            int pageSize = 2147483647,  //Int32.MaxValue
            ProductSortingEnum orderBy = ProductSortingEnum.Position,
            PlantformType plantform = PlantformType.Default);

        #endregion
    }
}
