{$reference USBRelay.dll}

begin
  WriteLn(USB.RelayManager.usb_just_test());
  USB.RelayManager.Init;
  WriteLn('Total devices : ', USB.RelayManager.DevicesCount);
  USB.RelayManager.OpenDevice(0);
  WriteLn('Device number   : ', USB.RelayManager.CurrentDeviceIndex);
  WriteLn('Device serial   : ', USB.RelayManager.RelaySerial);
  WriteLn('Device channels : ', USB.RelayManager.ChannelsCount);

  while true do
    begin
      var ch := ReadlnChar;
      case ch of
        '1' : begin
                WriteLn('Opening...');
                USB.RelayManager.OpenChannel(1);
                WriteLn('Channel 1 opened status : ', USB.RelayManager.ChannelOpened(1));
              end;
        '2' : begin
                WriteLn('Closing...');
                USB.RelayManager.CloseChannel(1);
                WriteLn('Channel 1 opened status : ', USB.RelayManager.ChannelOpened(1));
              end;
        'q' : break;
      end;
    end;
 USB.RelayManager.Close;
end.