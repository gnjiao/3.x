using System;
using System.Collections.ObjectModel;
using HalconDotNet;
using Hdc.Mv.Inspection;

namespace Hdc.Mv
{
    [Serializable]
    public class ActiveComputerDeviceInspectorInitializer : IInspectorInitializer
    {
        public static ActiveComputerDeviceInspectorInitializer Singleton { get; set; }

        public ActiveComputerDeviceInspectorInitializer()
        {
            if (Singleton == null)
            {
                Singleton = this;
            }
            else
            {
                //throw new InvalidOperationException($"{nameof(ActiveComputerDeviceInspectorInitializer)}.Singleton is not null");
                return;

            }
        }

        public void Dispose()
        {
            HOperatorSet.ReleaseAllComputeDevices();
        }

        public void Initialize()
        {
            Init(DeviceIdentifier);
        }

        public static void Init(int deviceIdentifier)
        {

            /*            // Compute Unit
                        HTuple hv_DeviceIdentifiers = null, hv_DeviceHandle = null;
                        HTuple hv_Name = null, hv_Vendor = null, hv_MaxWidth = null;
                        HTuple hv_MaxHeight = null;

                        HOperatorSet.QueryAvailableComputeDevices(out hv_DeviceIdentifiers);
                        HOperatorSet.OpenComputeDevice(hv_DeviceIdentifiers.TupleSelect(0), out hv_DeviceHandle);
                        HOperatorSet.GetComputeDeviceInfo(hv_DeviceIdentifiers, "name", out hv_Name);
                        HOperatorSet.GetComputeDeviceInfo(hv_DeviceIdentifiers, "vendor", out hv_Vendor);
                        HOperatorSet.GetComputeDeviceInfo(hv_DeviceIdentifiers, "image2d_max_width",out hv_MaxWidth);
                        HOperatorSet.GetComputeDeviceInfo(hv_DeviceIdentifiers, "image2d_max_height",out hv_MaxHeight);
                        HOperatorSet.InitComputeDevice(hv_DeviceHandle, "image_to_world_plane");
                        HOperatorSet.InitComputeDevice(hv_DeviceHandle, "mirror_image");
                        HOperatorSet.InitComputeDevice(hv_DeviceHandle, "mean_image");
                        HOperatorSet.InitComputeDevice(hv_DeviceHandle, "median_image");
                        //            HOperatorSet.InitComputeDevice(hv_DeviceHandle, "all");
                        HOperatorSet.ActivateComputeDevice(hv_DeviceHandle);*/

            var ids = HComputeDevice.QueryAvailableComputeDevices();

            //var deviceIdentifier = (int)ids.TupleSelect(0);
            ComputeDevice = new HComputeDevice(deviceIdentifier);
            ComputeDevice.OpenComputeDevice(deviceIdentifier);

            var name = HComputeDevice.GetComputeDeviceInfo(deviceIdentifier, "name");
            var vendor = HComputeDevice.GetComputeDeviceInfo(deviceIdentifier, "vendor");
            var image2d_max_width = HComputeDevice.GetComputeDeviceInfo(deviceIdentifier, "image2d_max_width");
            var image2d_max_height = HComputeDevice.GetComputeDeviceInfo(deviceIdentifier, "image2d_max_height");

            ComputeDevice.InitComputeDevice("image_to_world_plane");
            //            ComputeDevice.InitComputeDevice("mirror_image");
            //            ComputeDevice.InitComputeDevice("mean_image");
            //            ComputeDevice.InitComputeDevice("median_image");

            //            ComputeDevice.ActivateComputeDevice();
        }

        public void LoadConfigFile(string fileName)
        {
            //            throw new System.NotImplementedException();
        }

        public static HComputeDevice ComputeDevice { get; set; }

        //        public Collection<string> 

        public int DeviceIdentifier { get; set; }
    }
}