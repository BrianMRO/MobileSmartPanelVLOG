using MobileSmartPanel.IN;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;
using PX.Objects.CR;
using System;
using System.Collections;

namespace PX.Objects.IN
{
    public class InventoryItemMaint_Extension : PXGraphExtension<InventoryItemMaint>
    {
        #region IsActive
        public static bool IsActive()
        {
            //Insert IsActive Logic Here;
            return true;
        }
        #endregion

        #region Data Views
        public PXSetup<ZZINSetup> BlogSetup;
        public PXFilter<TagParamFilter> tagparamfilter;
        #endregion

        #region Create Tag
        public PXAction<InventoryItem> createTag;
        [PXUIField(DisplayName = "Create Tag", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXButton]
        protected virtual IEnumerable CreateTag(PXAdapter adapter)
        {
            ZZINTagEntry graph = PXGraph.CreateInstance<ZZINTagEntry>();
            tagparamfilter.View.RequestRefresh();

            WebDialogResult dialogResult = tagparamfilter.AskExt(setTagStateFilter, true);
            if (dialogResult == WebDialogResult.OK || (Base.IsContractBasedAPI && dialogResult == WebDialogResult.Yes))
            {
                Base.Save.Press();
                InventoryItem item = PXCache<InventoryItem>.CreateCopy(Base.Item.Current);
                TagParamFilter filter = tagparamfilter.Current;

                if (item?.InventoryID != null)
                {
                    ZZINTag tag = CreateTagProc(graph, filter);
                    if (tag?.TagID != null) throw new PXRedirectRequiredException(graph, "Tag Entry");
                }
            }
            return adapter.Get();
        }
        #endregion

        #region CheckTagParams
        public PXAction<InventoryItem> checkTagParams;
        [PXUIField(DisplayName = "OK", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXLookupButton]
        public virtual IEnumerable CheckTagParams(PXAdapter adapter)
        {
            return adapter.Get();
        }
        #endregion

        #region setTagStateFilter
        private void setTagStateFilter(PXGraph aGraph, string ViewName)
        {
            checkTagParams.SetEnabled(
                tagparamfilter.Current.CustomerID != null &&
                tagparamfilter.Current.ContactID != null
                );
        }
        #endregion

        #region CreateTagProc
        protected virtual ZZINTag CreateTagProc(ZZINTagEntry graph, TagParamFilter filter)
        {
            ZZINTag tag = null;

            if (filter?.CustomerID != null && filter?.ContactID != null)
            {

                tag = graph.Document.Insert();
                graph.Document.Cache.SetValueExt<ZZINTag.inventoryID>(tag, Base.Item.Current.InventoryID);
                graph.Document.Cache.SetValueExt<ZZINTag.descr>(tag, Base.Item.Current.InventoryCD);
                graph.Document.Cache.SetValueExt<ZZINTag.customerID>(tag, filter.CustomerID);
                graph.Document.Cache.SetValueExt<ZZINTag.contactID>(tag, filter.ContactID);
                graph.Document.Update(tag);

                graph.Persist();
            }

            return tag;
        }
        #endregion

        #region TagParamFilter
        [PXHidden]
        [Serializable()]
        public partial class TagParamFilter : IBqlTable
        {
            #region CustomerID
            [PXDefault]
            [CustomerActive(
                typeof(Search<BAccountR.bAccountID, Where<True, Equal<True>>>), // TODO: remove fake Where after AC-101187
                Visibility = PXUIVisibility.SelectorVisible,
                DescriptionField = typeof(Customer.acctName),
                Filterable = true)]
            public virtual Int32? CustomerID { get; set; }
            public abstract class customerID : PX.Data.BQL.BqlInt.Field<customerID> { }
            #endregion

            #region ContactID
            [PXDBInt()]
            [PXDefault]
            [PXSelector(
                typeof(Search<Contact.contactID,
                    Where<Contact.bAccountID, Equal<Current<TagParamFilter.customerID>>,
                        And<Contact.contactType, Equal<ContactTypesAttribute.person>>>>),
                typeof(Contact.contactID),
                typeof(Contact.displayName),
                SubstituteKey = typeof(Contact.displayName),
                Filterable = true
                )]
            [PXUIField(DisplayName = "Contact")]
            public virtual int? ContactID { get; set; }
            public abstract class contactID : PX.Data.BQL.BqlInt.Field<contactID> { }
            #endregion
        }
        #endregion

        #region Event Handlers

        #region TagParamFilter_RowSelected
        protected virtual void _(Events.RowSelected<TagParamFilter> e)
        {
            TagParamFilter row = e.Row;
            InventoryItem item = Base.Item.Current;

            checkTagParams.SetEnabled(
                tagparamfilter.Current.CustomerID != null &&
                tagparamfilter.Current.ContactID != null
                );
        }
        #endregion

        #region TagParamFilter_CustomerID_FieldUpdated
        protected virtual void _(Events.FieldUpdated<TagParamFilter.customerID> e)
        {
            TagParamFilter row = (TagParamFilter) e.Row;
            int? oldCustomerID = (int?) e.OldValue;
            if(row?.CustomerID == null || row?.CustomerID != oldCustomerID)
            {
                e.Cache.SetValueExt<TagParamFilter.contactID>(row, null);
            }
        }
        #endregion

        #endregion
    }
}