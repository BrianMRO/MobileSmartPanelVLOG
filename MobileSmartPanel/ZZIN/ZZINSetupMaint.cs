using PX.Data;
using PX.Data.BQL.Fluent;

namespace MobileSmartPanel.IN
{
    public class ZZINSetupMaint : PXGraph<ZZINSetupMaint>
    {
        #region Data Views
        [PXViewName("Blog Preferences")]
        public PXSelect<ZZINSetup> Preferences;

        public PXSave<ZZINSetup> Save;
        public PXCancel<ZZINSetup> Cancel;
        #endregion


    }
}
