using System;
using System.Linq;
using System.Threading;
using Hdc.Mv.ImageAcquisition;
using Hdc.Mv.Inspection;

namespace Hdc.Mv
{
    [Serializable]
    public class E2VChangeScanDirectionPlugin : IHalconFrameGrabberPlugin
    {
        public void Initialize(IHalconFrameInspectionController frameInspectionController)
        {
            var init = frameInspectionController.FrameGrabberSchema.MvSchema
                .FrameGrabberInitializers.SingleOrDefault(
                    x => x is E2VChangeScanDirectionInitializer) as E2VChangeScanDirectionInitializer;

            if (init == null)
                throw new Exception("E2VChangeScanDirectionInitializer cannot be found");

            frameInspectionController
                .FrameInspectionStateChangedEvent
                .Subscribe(
                    x =>
                    {
                        if (x.State != FrameState.Grabbed)
                            return;

                        if (x.FrameInfo.SurfaceIndex == 0)
                        {
                            // change e2v ScanDirection
                            if (!init.Port.IsOpen)
                                init.Port.Open();

                            init.Port.Write("w scdi 1");
                            init.Port.Write(new byte[] {13}, 0, 1);
                            Thread.Sleep(Delay);
                        }
                        else if (x.FrameInfo.SurfaceIndex == 1)
                        {
                            // change e2v ScanDirection
                            if (!init.Port.IsOpen)
                                init.Port.Open();

                            init.Port.Write("w scdi 0");
                            init.Port.Write(new byte[] {13}, 0, 1);
                            Thread.Sleep(Delay);
                        }
                    });
        }

        /// <summary>
        /// in milliseconds
        /// </summary>
        public int Delay { get; set; }
    }
}