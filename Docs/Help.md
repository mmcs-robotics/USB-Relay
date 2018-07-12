# Usage of managed USBRelay.dll
This library works only with one kind of relay boards – HID boards with no virtual COM-ports, with special prototol. All information about this you can find in original project wiki. So if demo application didn't find any relay board – sorry, but it seems you need to find another library.
## Installation
... not needed :)
1. For using this library you need two DLL's – USBRelay.dll and USB_RELAY_DEVICE.dll, builded in proper configuration (for more info about build configurations see [Readme.md](https://github.com/mmcs-robotics/USB-Relay/blob/master/README.md)).
2. Both DLL should be in one folder, you can add them to your project, then open «Properties» page for this files and select «Copy to output directory» – «Copy if newer».
3. Add Refrence to USBRelay.dll to you project.
4. Add `using USB;` to your namespaces list;
5. Everything should be OK.

## Common issues
While you use the library in your project, you will have a static class `USB.RelayManager` available.
This class maintain list of connected Relay boards, one of witch is «selected» board. So in common you should first select one of connected relays, then use functions you need – open/close/invert any port of relay board, get serial number of relay board, or check status of any relay port.
Several functions need to call Init() first, but Open/Close/Invert not need to do this – they have two args: board number (0-based) and relay number (1-based). After calling any of this functions they left affected board «selected».

## Functions
These functions can be used without initialization (previous call of `Init()` routine):
- `static bool RelayManager.Open(int deviceIndex, int channelIndex)` – opens one relay port on specified relay device. `deviceIndex` is 0-based, `channelIndex` is 1-based. Calling of `Init()` not nessesary to call this function (if not `Init()` was called, it will be called implicitly). If port already opened, then nothing happens.
- `static bool RelayManager.Close(int deviceIndex, int channelIndex)` – closes one relay port on specified relay device. `deviceIndex` is 0-based, `channelIndex` is 1-based. Calling of `Init()` not nessesary to call this function (if not `Init()` was called, it will be called implicitly). If port already closed, then nothing happens.
- `static bool RelayManager.Invert(int deviceIndex, int channelIndex)` – inverts one relay port on specified relay device. `deviceIndex` is 0-based, `channelIndex` is 1-based. Calling of Init() not nessesary to call this function (if not Init() was called, it will be called implicitly).

These routines need `Init()` call before using:
- `static bool RelayManager.Init() ` – initializes the library. This function should be called before all other.
- `static void RelayManager.Close()` – finalizes library and clear dll allocated memory.
- `static bool RelayManager.Inited()` – re-checks if RelayManager was inited, and returns true if everything is OK.
- `static int RelayManager.DevicesCount()` – returns number of relays, detected in system. If library wasn't inited – returns -1.
- `static bool RelayManager.SelectDevice(int deviceIndex)` – selects one of connected relay boards. If Current device was opened - it closes.
- `static bool RelayManager.OpenDevice()` – opens relay device selected by `Select(...)` routine. 
- `static bool RelayManager.OpenDevice(int deviceIndex)` – opens relay device by index, (from 0 to DevicesCount() - 1).
- `static int RelayManager.RelayStatus()` – gets status of all relay channels on the device. Bits 0/1/2/3/4/5/6/7/8 indicates channel 1/2/3/4/5/6/7/8 status. Each bit value 1 means ON, 0 means OFF. Relay should be opened, otherwise returns 0.
- `static int RelayManager.ChannelsCount()` – returns total number of channels for opened relay device, 0 if something wrong (not inited or device not opened).
- `static string RelayManager.RelaySerial()` – reads identifier string of device. Usually 5-character long.
- `static string RelayManager.CurrentDeviceIndex()` – returns active relay device index (0-based).
- `static bool RelayManager.CloseDevice()` – closes current device (selected and opened).
- `static bool RelayManager.OpenChannel(int channelIndex)` – opens specified channel on current relay device. Channel index is 1-based.
- `static bool RelayManager.CloseChannel(int channelIndex)` – closes specified channel on current relay device. Channel index is 1-based.
- `static bool RelayManager.OpenAllChannels()` – opens all channels of selected and opened relay.
- `static bool RelayManager.CloseAllChannels()` – closes all channels of selected and opened relay.
