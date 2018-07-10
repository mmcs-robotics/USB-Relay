# USB-Relay
This project is based on [usb-relay-hid](https://github.com/pavel-a/usb-relay-hid) repository for USB HID devices control. All licence information, docs, wiki can be found there. This project is addition – it provides managed DLL library for USB-Relay control for Standard .NET applications.

# Contents
Here are provided 3 projects:
1. lib – folder, containing C project for unmanaged DLL. It can be used in C/C++ projects, but the USB_RELAY_DEVICE.dll can't be used directly in .NET managed application.
2. NetDLL – project for building managed DLL library – USBRelay.dll, it uses unmanaged dll (USB_RELAY_DEVICE.dll), and tested for .NET Framework 4.6. There some issues about using managed and unmanaged libraries together, so please read notice below carefully.
3. NetDLLTest – simple C# console application, demonstrates usage of USBRealy.dll.
4. bin – pre-built binaries (if you don't want to build it by yourself).
For documentation please see [Help.md](https://github.com/mmcs-robotics/USB-Relay/blob/master/Docs/Help.md).

# Configurations
There two typical configurations to build projects:
1. If you are intending to use this library in your Standard .NET application with «AnyCPU» target, it's recommended to build usb-relay-dll project for x86 target (it's pure C, so it hasn't AnyCPU target option), then NetDLL for AnyCPU target, it should work. NetDLLTest also should be built for AnyCPU target.
2. In several cases the library should be built for x64 target. For example, builded for AnyCPU target DLL won't work with PascalABC.Net – because, instead of C#, it can't mix underlying x86 code (unmanaged DLL) with x64 managed code. So you should build all projects with x64 targer.
3. Both DLL's can be found in «bin» folder, builded for «AnyCPU» and «x64» targets.

# Help
Some improvements were made to the original project library, so you don't need to work with Relay handlers – basic functions allow you to manage relay board with just two indexes – relay board index (if you have more then one board connected), and relay port. For more information see Docs folder.
