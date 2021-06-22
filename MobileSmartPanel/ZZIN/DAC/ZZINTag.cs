using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.AR;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.IN;
using System;

namespace MobileSmartPanel.IN
{
    [Serializable]
    [PXCacheName("ZZINTag")]
    public class ZZINTag : IBqlTable
    {
        #region TagID
        [PXDBIdentity]
        public virtual int? TagID { get; set; }
        public abstract class tagID : PX.Data.BQL.BqlInt.Field<tagID> { }
        #endregion

        #region TagCD
        [PXDBString(15, IsKey = true, IsUnicode = true, InputMask = "")]
        [AutoNumber(typeof(ZZINSetup.tagNumberingID), typeof(AccessInfo.businessDate))]
        [PXSelector(
             typeof(ZZINTag.tagCD),
             typeof(ZZINTag.tagCD),
             typeof(ZZINTag.descr)
             )]
        [PXUIField(DisplayName = "Tag ID")]
        public virtual string TagCD { get; set; }
        public abstract class tagCD : PX.Data.BQL.BqlString.Field<tagCD> { }
        #endregion

        #region Descr
        [PXDBString(256, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Description")]
        public virtual string Descr { get; set; }
        public abstract class descr : PX.Data.BQL.BqlString.Field<descr> { }
        #endregion

        #region InventoryID
        [AnyInventory(Filterable = true)]
        [PXUIField(DisplayName = "Inventory ID")]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion

        #region SubItemID
        [PXDefault(typeof(Search<InventoryItem.defaultSubItemID,
            Where<InventoryItem.inventoryID, Equal<Current<ZZINTag.inventoryID>>,
            And<InventoryItem.defaultSubItemOnEntry, Equal<boolTrue>>>>),
            PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<ZZINTag.inventoryID>))]
        [SubItem(typeof(ZZINTag.inventoryID))]
        public virtual Int32? SubItemID { get; set; }
        public abstract class subItemID : PX.Data.BQL.BqlInt.Field<subItemID> { }
        #endregion

        #region CustomerID
        [CustomerActive(
            typeof(Search<BAccountR.bAccountID, Where<True, Equal<True>>>), // TODO: remove fake Where after AC-101187
            Visibility = PXUIVisibility.SelectorVisible,
            DescriptionField = typeof(Customer.acctName),
            Filterable = true)]
        [PXForeignReference(typeof(Field<ZZINTag.customerID>.IsRelatedTo<BAccount.bAccountID>))]
        public virtual Int32? CustomerID { get; set; }
        public abstract class customerID : PX.Data.BQL.BqlInt.Field<customerID> { }
        #endregion

        #region ContactID
        [PXDBInt()]
        [PXSelector(
            typeof(Search<Contact.contactID,
                Where<Contact.bAccountID, Equal<Current<ZZINTag.customerID>>,
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

        #region CreatedByID
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
        #endregion

        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
        #endregion

        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
        #endregion

        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
        #endregion

        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
        #endregion

        #region Tstamp
        [PXDBTimestamp()]
        [PXUIField(DisplayName = "Tstamp")]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion

        #region Noteid
        [PXNote()]
        public virtual Guid? Noteid { get; set; }
        public abstract class noteid : PX.Data.BQL.BqlGuid.Field<noteid> { }
        #endregion

        public static class Constants
        {
            static string TagNumberingID = "BLOGTAG";
            public class tagNumberingID : PX.Data.BQL.BqlString.Constant<tagNumberingID>
            {
                public tagNumberingID() : base(TagNumberingID) { }
            }
        }
    }
}