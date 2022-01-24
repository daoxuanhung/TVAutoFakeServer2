using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace TVAutoFakeServer2
{
    class Devcon
    {
        public static bool InstallDriver(string inf, string hwid)
        {
            StringBuilder className = new StringBuilder(MAX_CLASS_NAME_LEN);
            Guid ClassGUID = Guid.Empty;

            if (!SetupDiGetINFClass(inf, ref ClassGUID, className, MAX_CLASS_NAME_LEN, 0))
                return false;

            IntPtr DeviceInfoSet = SetupDiCreateDeviceInfoList(ref ClassGUID, IntPtr.Zero);
            SP_DEVINFO_DATA DeviceInfoData = new SP_DEVINFO_DATA();
            if (!SetupDiCreateDeviceInfo(DeviceInfoSet, className.ToString(), ref ClassGUID, null, IntPtr.Zero, DICD_GENERATE_ID, DeviceInfoData))
                return false;

            if (!SetupDiSetDeviceRegistryProperty(DeviceInfoSet, DeviceInfoData, SPDRP_HARDWAREID, hwid, hwid.Length))
            {
                SetupDiDestroyDeviceInfoList(DeviceInfoSet);
                return false;
            }

            if (!SetupDiCallClassInstaller(DIF_REGISTERDEVICE, DeviceInfoSet, DeviceInfoData))
            {
                SetupDiDestroyDeviceInfoList(DeviceInfoSet);
                return false;
            }

            // http://stackoverflow.com/questions/11474317/updatedriverforplugandplaydevices-error-is-telling-me-im-not-doing-something
            try
            {
                bool reboot = false;
                if (!UpdateDriverForPlugAndPlayDevices(IntPtr.Zero, hwid, inf, 0, reboot))
                {
                    SetupDiCallClassInstaller(DIF_REMOVE, DeviceInfoSet, DeviceInfoData);
                    return false;
                }
            }
            catch (AccessViolationException) { }
            return true;
        }

        public static bool SetIP(string networkInterfaceName)
        {
            var networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(nw => nw.Name == networkInterfaceName);
            var ipProperties = networkInterface.GetIPProperties();
            var ipInfo = ipProperties.UnicastAddresses.FirstOrDefault(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork);
            var isDHCPenabled = ipProperties.GetIPv4Properties().IsDhcpEnabled;

            var process = new Process
            {
                StartInfo = new ProcessStartInfo("netsh", "interface set interface name = \"" + networkInterfaceName + "\" newname = \"TVAuto\"") { Verb = "runas" }
            };
            process.Start();
            process.WaitForExit();
            var successful = process.ExitCode == 0;
            process.Dispose();

            process = new Process
            {
                StartInfo = new ProcessStartInfo("netsh", "interface ip set address name = \"TVAuto\" source=static 103.92.26.100 255.255.255.255") { Verb = "runas" }
            };
            process.Start();
            process.WaitForExit();
            successful = process.ExitCode == 0;
            process.Dispose();

            return successful;
        }

        public static string GetNICName()
        {
            string name = string.Empty;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.Description == "Microsoft KM-TEST Loopback Adapter")
                    name = nic.Name;
            }

            return name;
        }

        // Consts
        const int MAX_CLASS_NAME_LEN = 32;
        const int SPDRP_HARDWAREID = 0x00000001;
        const int DICD_GENERATE_ID = 0x00000001;
        const int DIF_REGISTERDEVICE = 0x00000019;
        const int DIF_REMOVE = 0x00000005;

        // Pinvokes
        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiGetINFClass(string infName, ref Guid ClassGuid, [MarshalAs(UnmanagedType.LPStr)] StringBuilder ClassName, int ClassNameSize, int RequiredSize);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern IntPtr SetupDiCreateDeviceInfoList(ref Guid ClassGuid, IntPtr hwndParent);

        [DllImport("Setupapi.dll", SetLastError = true)]
        static extern bool SetupDiCreateDeviceInfo(IntPtr DeviceInfoSet, String DeviceName, ref Guid ClassGuid, string DeviceDescription, IntPtr hwndParent, Int32 CreationFlags, SP_DEVINFO_DATA DeviceInfoData);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiSetDeviceRegistryProperty(IntPtr DeviceInfoSet, SP_DEVINFO_DATA DeviceInfoData, uint Property, string PropertyBuffer, int PropertyBufferSize);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiCallClassInstaller(UInt32 InstallFunction, IntPtr DeviceInfoSet, SP_DEVINFO_DATA DeviceInfoData);

        [DllImport("newdev.dll", SetLastError = true)]
        static extern bool UpdateDriverForPlugAndPlayDevices(IntPtr hwndParent, string HardwareId, string FullInfPath, int InstallFlags, bool bRebootRequired);

        // Structs
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        class SP_DEVINFO_DATA
        {
            internal int cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA));
            [MarshalAs(UnmanagedType.Struct)]
            internal Guid classGuid = Guid.Empty; // temp
            internal int devInst = 0; // dumy
            internal long reserved = 0;
        }
    }
}
