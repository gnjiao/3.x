using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hdc.Mv.Inspection
{
    public class DefectResultCollection : Collection<DefectInfo>
    {
        public DefectResultCollection()
        {
        }

        public DefectResultCollection(IList<DefectInfo> list)
            : base(list)
        {
        }
    }
}