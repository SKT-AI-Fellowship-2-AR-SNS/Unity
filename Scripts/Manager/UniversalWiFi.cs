using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if !UNITY_EDITOR
using Windows.Devices.WiFi;
#endif

public class UniversalWiFi : IWiFiAdapter
{

    private bool IsReady { get; set; }
    private uint Signal { get; set; }
    private string Report { get; set; }
    private string Ssid { get; set; }
    List<string> list = new List<string>();
    public UniversalWiFi()
    {
        IsReady = true;
        Signal = 0u;
        Ssid = string.Empty;
        Report = string.Empty;
    }
    public uint GetSignal(string ssid)
    {
        Ssid = ssid;
        Scan();
        return Signal;
    }

    private async void Scan()
    {
        if (IsReady)
        {
            IsReady = false;
            uint signal = 0;
#if !UNITY_EDITOR
            var result = await WiFiAdapter.FindAllAdaptersAsync();
            if (result.Count >= 1)
            {
                var firstAdapter = result[0];
                await firstAdapter.ScanAsync();
                GenerateNetworkReport(firstAdapter.NetworkReport);
                if (!string.IsNullOrEmpty(Ssid))
                {
                    signal = GetNetworkSignal(firstAdapter.NetworkReport, Ssid);
                }
            }
#endif
            IsReady = true;
            Signal = signal;
        }
    }
#if !UNITY_EDITOR
    private byte GetNetworkSignal(WiFiNetworkReport report, string ssid)
    {
        var network = report.AvailableNetworks.Where(x => x.Ssid.ToLower() == ssid.ToLower()).FirstOrDefault();
        return network.SignalBars;
    }

    private void GenerateNetworkReport(WiFiNetworkReport report)
    {
        var networks = new List<string>();
        foreach (var network in report.AvailableNetworks)
        {
            /*networks.Add(string.Format("SSID: {0} -- SignalBars: {1} -- Db: {2} -- Mac: {3}",
network.Ssid, network.SignalBars, network.NetworkRssiInDecibelMilliwatts, network.Bssid));*/
            networks.Add(network.Bssid);
            list.Add(network.Bssid);
        }
        Report = string.Join(Environment.NewLine, networks.ToArray());
    }
#endif
    public List<string> GetNetworkReport()
    {
        Scan();
        return list;
    }
}
