using PX.Data;
using PX.Data.BQL.Fluent;

namespace MobileSmartPanel.IN
{
    public class ZZINTagEntry : PXGraph<ZZINTagEntry, ZZINTag>
    {
        [PXViewName("Tag")]
        public SelectFrom<ZZINTag>.View Document;

        [PXViewName("Current Tag")]
        public SelectFrom<ZZINTag>
            .Where<ZZINTag.tagID.IsEqual<ZZINTag.tagID.FromCurrent>>
            .View CurrentTag;

        public PXSetup<ZZINSetup> Setup;

        #region Constructor
        public ZZINTagEntry()
        {
            ZZINSetup setup = Setup.Current;
        }
        #endregion
    }
}
