using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace USB
{
    public static class RelayManager
    {
        private static bool inited = false;
        private static bool deviceOpened = false;
        private static int openedeDeviceIndex = 0;
        private static int channelsCount = 0;

        [DllImport("USB_RELAY_DEVICE.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe public static extern int usb_just_test();

        [DllImport("USB_RELAY_DEVICE.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe private static extern void rl_finalize();

        /// <summary>
        /// Finalizes library and clear dll allocated memory.
        /// </summary>
        public static void Close()
        {
            if (inited)
            {
                rl_finalize();
                inited = false;
            }
        }

        [DllImport("USB_RELAY_DEVICE.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe private static extern int rl_count();

        /// <summary>
        /// Returns number of relays, detected in system. 
        /// </summary>
        /// <returns>Count of relay devices, if library was inited, and -1 otherwise</returns>
        public static int DevicesCount()
        {
            if (!inited) return -1;
            return rl_count();
        }

        [DllImport("USB_RELAY_DEVICE.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe private static extern int rl_init();

        /// <summary>
        /// Trying to initialize a library. This function should be called before all other.
        /// </summary>
        public static bool Init()
        {
            deviceOpened = false;
            openedeDeviceIndex = 0;
            inited = (rl_init() == 0);
            return inited;
        }

        [DllImport("USB_RELAY_DEVICE.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe private static extern int rl_closeDevice();

        /// <summary>
        /// Closes opened relay device.
        /// </summary>
        /// <returns>True if relay was opened, and false otherwise</returns>
        public static bool CloseDevice()
        {
            if (!inited || !deviceOpened) return false;
            if (rl_closeDevice() == 0)
            {
                deviceOpened = false;
                return true;
            }
            return false;
        }

        [DllImport("USB_RELAY_DEVICE.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe private static extern int rl_channelsCount();

        [DllImport("USB_RELAY_DEVICE.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe private static extern int rl_rewind(int newDeviceIndex);

        /// <summary>
        /// Selects relay device from list by index. If Current device was opened - it closes.
        /// </summary>
        /// <param name="deviceIndex">Relay index, 0-based</param>
        /// <returns>True if selection was successfull, false if failed to select specified device</returns>
        public static bool SelectDevice(int deviceIndex)
        {
            if (!inited || deviceIndex == openedeDeviceIndex) return false;
            if (deviceOpened) CloseDevice();
            deviceOpened = false;
            if (rl_rewind(deviceIndex) != 0) return false;
            openedeDeviceIndex = deviceIndex;
            return true;
        }
        /// <summary>
        /// Returns total number of channels for opened relay device.
        /// </summary>
        /// <returns>Number of relay channels, 0 if something wrong (not inited or device not opened)</returns>
        public static int ChannelsCount()
        {
            if (!inited || !deviceOpened) return 0;
            return channelsCount;
        }

        [DllImport("USB_RELAY_DEVICE.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe private static extern int rl_openDevice();

        /// <summary>
        /// Opens relay device, selected by Select routine.
        /// </summary>
        /// <returns>True on success, false otherwise</returns>
        public static bool OpenDevice()
        {
            if (!inited || deviceOpened) return false;
            if (rl_openDevice() != 0) return false;
            deviceOpened = true;
            channelsCount = rl_channelsCount();
            return true;
        }

        /// <summary>
        /// Opens relay device by number (0-based).
        /// </summary>
        /// <param name="deviceIndex">Device index from 0 to DevicesCount - 1</param>
        /// <returns>False if failed or device already opened</returns>
        public static bool OpenDevice(int deviceIndex)
        {
            if (!inited) return false;
            if (deviceIndex != openedeDeviceIndex && !SelectDevice(deviceIndex)) return false;

            if (rl_openDevice() != 0) return false;
            deviceOpened = true;
            openedeDeviceIndex = deviceIndex;
            channelsCount = rl_channelsCount();
            return true;
        }

        [DllImport("USB_RELAY_DEVICE.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe private static extern int rl_openChannel(int channelIndex);

        /// <summary>
        /// Opens one channel of selected and opened relay.
        /// </summary>
        /// <param name="channelIndex">Channel index, 1-based</param>
        /// <returns></returns>
        public static bool OpenChannel(int channelIndex)
        {
            if (!inited || !deviceOpened) return false;
            return rl_openChannel(channelIndex) == 0;
        }

        [DllImport("USB_RELAY_DEVICE.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe private static extern int rl_closeChannel(int channelIndex);

        /// <summary>
        /// Closes one channel of selected and opened relay.
        /// </summary>
        /// <param name="channelIndex">Channel index, 1-based</param>
        /// <returns></returns>
        public static bool CloseChannel(int channelIndex)
        {
            if (!inited || !deviceOpened) return false;
            return rl_closeChannel(channelIndex) == 0;
        }

        [DllImport("USB_RELAY_DEVICE.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe private static extern int rl_openAllChannels();

        /// <summary>
        /// Opens all channels of selected and opened relay.
        /// </summary>
        /// <returns></returns>
        public static bool OpenAllChannels()
        {
            if (!inited || !deviceOpened) return false;
            return rl_openAllChannels() == 0;
        }


        [DllImport("USB_RELAY_DEVICE.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe private static extern int rl_closeAllChannels();

        /// <summary>
        /// Closes all channels of selected and opened relay.
        /// </summary>
        /// <returns></returns>
        public static bool CloseAllChannels()
        {
            if (!inited || !deviceOpened) return false;
            return rl_closeAllChannels() == 0;
        }

        [DllImport("USB_RELAY_DEVICE.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe private static extern void rl_getSerialNumber(IntPtr charBuffer, int buffSize);

        /// <summary>
        /// Reads identifier string of device. Usually 5-character long.
        /// </summary>
        /// <returns></returns>
        public static string RelaySerial()
        {
            if (!inited || !deviceOpened) return "none";
            unsafe
            {
                // Initializing array in unmanaged memory
                int size = 255;
                IntPtr ptrToBuffer = Marshal.AllocHGlobal(size);
                try
                {
                    rl_getSerialNumber(ptrToBuffer, 255);

                    string rez = Marshal.PtrToStringAnsi(ptrToBuffer);

                    return rez;
                }
                finally
                {
                    Marshal.FreeHGlobal(ptrToBuffer);
                }
            }
        }


        [DllImport("USB_RELAY_DEVICE.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe private static extern int rl_status();

        /// <summary>
        /// Get status of all relay channels on the device.
        /// bit 0/1/2/3/4/5/6/7/8 indicate channel 1/2/3/4/5/6/7/8 status
        /// Each bit value 1 means ON, 0 means OFF.
        /// Relay should be opened, otherwise returns 0.
        /// </summary>
        /// <returns>Bitmask of all relay channels state, if the value > 0. Negative values mean error.</returns>
        public static int RelayStatus()
        {
            return rl_status();
        }

        /* Indicates init status, 1 - relay was inited, 0 - failed to init */
        [DllImport("USB_RELAY_DEVICE.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe private static extern int rl_inited();

        /// <summary>
        /// Re-checks and return inited status.
        /// </summary>
        /// <returns></returns>
        static public bool Inited()
        {
            return inited = rl_inited() != 0;
        }

        /* Returns current device index, from 0 to relay count - 1. If something wrong - returns -1 */
        [DllImport("USB_RELAY_DEVICE.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        unsafe private static extern int rl_current_device_index();
        /// <summary>
        /// Re-checks and return current device number.
        /// </summary>
        /// <returns></returns>
        static public int CurrentDeviceIndex()
        {
            return openedeDeviceIndex = rl_current_device_index();
        }

        /// <summary>
        /// Checks if relay channel is opened. Relay device should be opened.
        /// </summary>
        /// <param name="channelIndex">Index of relay channel, 1-based</param>
        /// <returns>True if everything is OK and channel is opened, false otherwise (closed or error)</returns>
        public static bool ChannelOpened(int channelIndex)
        {
            if (!inited || !deviceOpened) return false;
            int bitMask = 1;
            while (channelIndex > 1)
            {
                --channelIndex;
                bitMask = bitMask << 1;
            }

            return (rl_status() & bitMask) != 0;
        }

        /// <summary>
        /// Opens one channel on selected relay device.
        /// </summary>
        /// <param name="deviceIndex">Index of device, 0-based</param>
        /// <param name="channelIndex">Channel of selected device, 1-based</param>
        /// <returns></returns>
        public static bool Open(int deviceIndex, int channelIndex)
        {
            if (deviceIndex < 0 || channelIndex <= 0) return false;
            if (!inited && !Init()) return false;
            if (deviceIndex >= DevicesCount()) return false;
            OpenDevice(deviceIndex);
            if (!deviceOpened || channelIndex > channelsCount) return false;
            return OpenChannel(channelIndex);
        }

        /// <summary>
        /// Closes one channel on selected relay device.
        /// </summary>
        /// <param name="deviceIndex">Index of device, 0-based</param>
        /// <param name="channelIndex">Channel of selected device, 1-based</param>
        /// <returns></returns>
        public static bool Close(int deviceIndex, int channelIndex)
        {
            if (deviceIndex < 0 || channelIndex <= 0) return false;
            if (!inited && !Init()) return false;
            if (deviceIndex >= DevicesCount()) return false;
            OpenDevice(deviceIndex);
            if (!deviceOpened || channelIndex > channelsCount) return false;
            return CloseChannel(channelIndex);
        }

        /// <summary>
        /// Inverts one channel on selected relay device.
        /// </summary>
        /// <param name="deviceIndex">Index of device, 0-based</param>
        /// <param name="channelIndex">Channel of selected device, 1-based</param>
        /// <returns></returns>
        public static bool Invert(int deviceIndex, int channelIndex)
        {
            if (deviceIndex < 0 || channelIndex <= 0) return false;
            if (!inited && !Init()) return false;
            if (deviceIndex >= DevicesCount()) return false;
            OpenDevice(deviceIndex);
            if (!deviceOpened || channelIndex > channelsCount) return false;

            if( ChannelOpened(channelIndex)) 
                return CloseChannel(channelIndex);
            else
                return OpenChannel(channelIndex);
        }
    }
}
