using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using USB;

namespace USBRelayTest
{
    class Program
    {
        static void Main(string[] args)
        {
            RelayManager.Init();
            Console.WriteLine("Total devices : " + RelayManager.DevicesCount().ToString());
            for (int i = 0; i < RelayManager.DevicesCount(); ++i)
            {
                RelayManager.OpenDevice(i);
                Console.WriteLine("Device number   : {0}", RelayManager.CurrentDeviceIndex());
                Console.WriteLine("Device serial   : {0}", RelayManager.RelaySerial());
                Console.WriteLine("Device channels : {0}", RelayManager.ChannelsCount());
                Console.WriteLine("--------------------------------");
            }

            Console.WriteLine("Enter 'q' for quit");
            
            while (true)
            {
                string s = Console.ReadLine();
                if (s == "1")
                {
                    Console.WriteLine("Opening...");
                    RelayManager.Open(0,1);
                    Console.WriteLine("Channel 1 opened status : " + RelayManager.ChannelOpened(1));
                }
                if (s == "2")
                {
                    Console.WriteLine("Closing...");
                    RelayManager.Close(0,1);
                    Console.WriteLine("Channel 1 opened status : " + RelayManager.ChannelOpened(1));
                }
                if (s == "3")
                {
                    Console.WriteLine("Opening...");
                    RelayManager.Open(1, 1);
                    Console.WriteLine("Channel 1 opened status : " + RelayManager.ChannelOpened(1));
                }
                if (s == "4")
                {
                    Console.WriteLine("Closing...");
                    RelayManager.Close(1, 1);
                    Console.WriteLine("Channel 1 opened status : " + RelayManager.ChannelOpened(1));
                }
                if (s == "5")
                {
                    Console.WriteLine("Inverting channel 1 on relay 0...");
                    RelayManager.Invert(0, 1);
                    Console.WriteLine("Channel 1 opened status : " + RelayManager.ChannelOpened(1));
                }
                if (s == "6")
                {
                    Console.WriteLine("Inverting channel 1 on relay 1 ...");
                    RelayManager.Invert(1, 1);
                    Console.WriteLine("Channel 1 opened status : " + RelayManager.ChannelOpened(1));
                }
                if (s == "q") break;
            }
            RelayManager.Close();
        }
    }
}