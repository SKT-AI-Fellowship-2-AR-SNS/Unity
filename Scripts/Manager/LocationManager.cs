﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class LocationManager : MonoBehaviour
{
    [SerializeField]
    Text text;

    private IWiFiAdapter WiFiAdapter;
    // Start is called before the first frame update
    void Start()
    {
        WiFiAdapter = new UniversalWiFi();
        wifi();
        InvokeRepeating("wifi", 30, 60);
    }
    /*void Update()
    {
        wifi();
    }*/
    void wifi()
    {
        StartCoroutine("Scan");
    }
    IEnumerator Scan()
    {
#if UNITY_EDITOR
        string url = "http://3.34.20.225:3000/main/getLocation";

        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        //byte[] data = Encoding.UTF8.GetBytes("{\n   \"bssid1\" : \"00:08:9f:01:cc:9c\",\n   \"bssid2\" : \"10:e3:c7:05:a9:c7\"\n}");
        string[] report = new string[2];
        report[0] = "00:08:9f:01:cc:9c"; report[1] = "10:e3:c7:05:a9:c7";
        byte[] data = Encoding.UTF8.GetBytes("{\n   \"bssid1\" : \"" + report[0] + "\",\n   \"bssid2\" : \"" + report[1] + "\"\n}");
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        var j = JObject.Parse(result);
        string time = j["data"].ToString();
        text.text = time;
#endif
#if !UNITY_EDITOR
        if (WiFiAdapter != null)
        {
            var report = WiFiAdapter.GetNetworkReport();
            string url = "http://3.34.20.225:3000/main/getLocation";

            List<IMultipartFormSection> form = new List<IMultipartFormSection>();
            byte[] data = Encoding.UTF8.GetBytes("{\n   \"bssid1\" : \"" + report[0] + "\",\n   \"bssid2\" : \"" + report[1] + "\"\n}");
            UnityWebRequest request = UnityWebRequest.Post(url, form);

            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            string result = request.downloadHandler.text;
            var j = JObject.Parse(result);
            string time = j["data"].ToString();

            text.text = time;
        }
#endif
    }
}