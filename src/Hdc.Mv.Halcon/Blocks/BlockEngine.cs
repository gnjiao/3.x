using System.Linq;
using Core;

namespace Hdc.Mv.Halcon
{
    public class BlockEngine
    {
        public void Run(BlockSchema schema)
        {
            schema.Blocks.ForEach(x => x.Initialize());

            foreach (var fb in schema.Blocks)
            {
                foreach (var portReference in fb.PortReferences)
                {
                    var sourceFb = schema.Blocks.SingleOrDefault(
                        x => x.Name == portReference.SourceBlockName);

                    object sourcePortValue = sourceFb.GetPropertyValueByPropertyName(portReference.SourcePortName);

                    fb.SetPropertyValueByPropertyName(portReference.TargetPortName, sourcePortValue);
                }

                fb.Process();

                if (fb.Status != BlockStatus.Valid)// && fb.Status != BlockStatus.Warning)
                {
                    break;
                }
            }
        }
    }
}