﻿using System;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Storage.FileSystem;
using Nefarius.DsHidMini.ControlApp.Util;

namespace Nefarius.DsHidMini.ControlApp.Drivers
{
    public static class BthPS3FilterDriver
    {
        private const uint IOCTL_BTHPS3PSM_ENABLE_PSM_PATCHING = 0x002AAC04;
        private const uint IOCTL_BTHPS3PSM_DISABLE_PSM_PATCHING = 0x002AAC08;
        private const uint IOCTL_BTHPS3PSM_GET_PSM_PATCHING = 0x002A6C0C;

        private static readonly string BTHPS3PSM_CONTROL_DEVICE_PATH = "\\\\.\\BthPS3PSMControl";

        private static string ErrorMessage =>
            "BthPS3 filter driver access failed. Is Bluetooth turned on? Are the drivers installed?";

        /// <summary>
        ///     True if filter driver is currently loaded and operational, false otherwise.
        /// </summary>
        public static bool IsFilterAvailable
        {
            get
            {
                if (!BluetoothHelper.IsBluetoothRadioAvailable)
                    return false;

                using var handle = PInvoke.CreateFile(
                    BTHPS3PSM_CONTROL_DEVICE_PATH,
                    FILE_ACCESS_FLAGS.FILE_GENERIC_READ | FILE_ACCESS_FLAGS.FILE_GENERIC_WRITE,
                    FILE_SHARE_MODE.FILE_SHARE_READ | FILE_SHARE_MODE.FILE_SHARE_WRITE,
                    null,
                    FILE_CREATION_DISPOSITION.OPEN_EXISTING,
                    FILE_FLAGS_AND_ATTRIBUTES.FILE_ATTRIBUTE_NORMAL, null
                );

                var error = (WIN32_ERROR)Marshal.GetLastWin32Error();

                return error is WIN32_ERROR.ERROR_SUCCESS or WIN32_ERROR.ERROR_ACCESS_DENIED;
            }
        }

        /// <summary>
        ///     Gets or sets current filter patching state.
        /// </summary>
        public static unsafe bool IsFilterEnabled
        {
            get
            {
                if (!BluetoothHelper.IsBluetoothRadioAvailable)
                    return false;

                using var handle = PInvoke.CreateFile(
                    BTHPS3PSM_CONTROL_DEVICE_PATH,
                    FILE_ACCESS_FLAGS.FILE_GENERIC_READ | FILE_ACCESS_FLAGS.FILE_GENERIC_WRITE,
                    FILE_SHARE_MODE.FILE_SHARE_READ | FILE_SHARE_MODE.FILE_SHARE_WRITE,
                    null,
                    FILE_CREATION_DISPOSITION.OPEN_EXISTING,
                    FILE_FLAGS_AND_ATTRIBUTES.FILE_ATTRIBUTE_NORMAL, null
                );

                if (handle.IsInvalid)
                    throw new Exception(ErrorMessage);

                var payloadBuffer = Marshal.AllocHGlobal(Marshal.SizeOf<BTHPS3PSM_GET_PSM_PATCHING>());
                var payload = new BTHPS3PSM_GET_PSM_PATCHING { DeviceIndex = 0 };

                try
                {
                    Marshal.StructureToPtr(payload, payloadBuffer, false);

                    PInvoke.DeviceIoControl(
                        handle,
                        IOCTL_BTHPS3PSM_GET_PSM_PATCHING,
                        payloadBuffer.ToPointer(),
                        (uint)Marshal.SizeOf<BTHPS3PSM_GET_PSM_PATCHING>(),
                        payloadBuffer.ToPointer(),
                        (uint)Marshal.SizeOf<BTHPS3PSM_GET_PSM_PATCHING>(),
                        null,
                        null
                    );

                    payload = Marshal.PtrToStructure<BTHPS3PSM_GET_PSM_PATCHING>(payloadBuffer);
                }
                finally
                {
                    Marshal.FreeHGlobal(payloadBuffer);
                }

                return payload.IsEnabled > 0;
            }
            set
            {
                using var handle = PInvoke.CreateFile(
                    BTHPS3PSM_CONTROL_DEVICE_PATH,
                    FILE_ACCESS_FLAGS.FILE_GENERIC_READ | FILE_ACCESS_FLAGS.FILE_GENERIC_WRITE,
                    FILE_SHARE_MODE.FILE_SHARE_READ | FILE_SHARE_MODE.FILE_SHARE_WRITE,
                    null,
                    FILE_CREATION_DISPOSITION.OPEN_EXISTING,
                    FILE_FLAGS_AND_ATTRIBUTES.FILE_ATTRIBUTE_NORMAL, null
                );

                if (handle.IsInvalid)
                    throw new Exception(ErrorMessage);

                var payloadEnableBuffer = Marshal.AllocHGlobal(Marshal.SizeOf<BTHPS3PSM_ENABLE_PSM_PATCHING>());
                var payloadEnable = new BTHPS3PSM_ENABLE_PSM_PATCHING { DeviceIndex = 0 };
                var payloadDisableBuffer = Marshal.AllocHGlobal(Marshal.SizeOf<BTHPS3PSM_DISABLE_PSM_PATCHING>());
                var payloadDisable = new BTHPS3PSM_DISABLE_PSM_PATCHING { DeviceIndex = 0 };

                try
                {
                    Marshal.StructureToPtr(payloadEnable, payloadEnableBuffer, false);
                    Marshal.StructureToPtr(payloadDisable, payloadDisableBuffer, false);

                    if (value)
                        PInvoke.DeviceIoControl(
                            handle,
                            IOCTL_BTHPS3PSM_ENABLE_PSM_PATCHING,
                            payloadEnableBuffer.ToPointer(),
                            (uint)Marshal.SizeOf<BTHPS3PSM_ENABLE_PSM_PATCHING>(),
                            null,
                            0,
                            null,
                            null
                        );
                    else
                        PInvoke.DeviceIoControl(
                            handle,
                            IOCTL_BTHPS3PSM_DISABLE_PSM_PATCHING,
                            payloadDisableBuffer.ToPointer(),
                            (uint)Marshal.SizeOf<BTHPS3PSM_DISABLE_PSM_PATCHING>(),
                            null,
                            0,
                            null,
                            null
                        );
                }
                finally
                {
                    Marshal.FreeHGlobal(payloadEnableBuffer);
                    Marshal.FreeHGlobal(payloadDisableBuffer);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct BTHPS3PSM_ENABLE_PSM_PATCHING
        {
            public uint DeviceIndex;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct BTHPS3PSM_DISABLE_PSM_PATCHING
        {
            public uint DeviceIndex;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
        private struct BTHPS3PSM_GET_PSM_PATCHING
        {
            public uint DeviceIndex;

            public readonly uint IsEnabled;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0xC8)]
            public readonly string SymbolicLinkName;
        }
    }
}