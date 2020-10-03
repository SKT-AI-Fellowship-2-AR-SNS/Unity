using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IWiFiAdapter
{
    uint GetSignal(string ssid);
    List<string> GetNetworkReport();
}