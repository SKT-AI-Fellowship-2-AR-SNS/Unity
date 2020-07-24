using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System;

public class sirv : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Upload());
    }
    IEnumerator GetInfo()
    {
        string url = "https://unkidgen.sirv.com/path/tmp.jpg?crop.type=face&info";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        var r = JObject.Parse(result);
        int cnt = 0;
        //Debug.Log(r.SelectToken("processingSettings").SelectToken("crop").SelectToken("faces").SelectToken("faces"));
        try
        {
            var a = r.SelectToken("processingSettings").SelectToken("crop").SelectToken("faces").SelectToken("faces");
            foreach (var item in a)
            {
                cnt++;
            }
            Debug.Log(cnt);
        }
        catch
        {
            Debug.Log(cnt);
        }

        //Debug.Log(r.SelectToken("smartcrop").ToString());
    }
    IEnumerator Upload()
    {
        string jsonStr = "{\n " +
            "\"clientId\": \"FK4MFqmZQb09gnNRClocIYQfXvX\",\n " +
            "\"clientSecret\": \"hQl3ARwvb6ZZ8JOf2wPGE6X3CCKYWGUWBLPTygT/LwRQLB58Aa7b+19WYaTyZd1Gg/pvVhm8nhq/jiybWtp9xg==\"\n" +
            "}";
        var formData = System.Text.Encoding.UTF8.GetBytes(jsonStr);
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        form.Add(new MultipartFormDataSection(formData));
        
        UnityWebRequest www = UnityWebRequest.Post("https://api.sirv.com/v2/token", form);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(formData);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        string result = www.downloadHandler.text;
        string[] tmp = result.Split(new char[] { ',' });
        string tmp1 = tmp[0];
        string token = tmp1.Substring(14,tmp1.Length-15);

        ///////////////////
        jsonStr = "C:/Users/Administrator/Downloads/a2.jpg";
        formData = File.ReadAllBytes(jsonStr);
        form = new List<IMultipartFormSection>();
        form.Add(new MultipartFormDataSection(formData));

        www = UnityWebRequest.Post("https://api.sirv.com/v2/files/upload?filename=%2Fpath%2Fmy.jpg", form);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(formData);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("authorization", "Bearer " + token);
        www.SetRequestHeader("Content-Type", "image/jpeg");
        yield return www.SendWebRequest();

        ///////////////////
        string url = "https://unkidgen.sirv.com/path/my.jpg?crop.type=face&info";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        result = request.downloadHandler.text;
        var r = JObject.Parse(result);
        int cnt = 0;
        try
        {
            var a = r.SelectToken("processingSettings").SelectToken("crop").SelectToken("faces").SelectToken("faces");
            foreach (var item in a)
            {
                cnt++;
            }
            Debug.Log(cnt);
        }
        catch
        {
            Debug.Log(cnt);
        }

        ///////////////////
        for(int i = 0; i < cnt; i++)
        {
            url = "https://unkidgen.sirv.com/path/my.jpg?crop.type=face&crop.face="+i;
            request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            var data = request.downloadHandler.data;

            form = new List<IMultipartFormSection>();
            form.Add(new MultipartFormFileSection("image", data, url, "image/jpg"));

            www = UnityWebRequest.Post("https://stg-va.sktnugu.com/api/v1/face/recognize", form);
            www.SetRequestHeader("app-id", "FHJEF7O455");
            www.SetRequestHeader("group-id", "SMB2NA4ND0");
            yield return www.SendWebRequest();

            result = www.downloadHandler.text;
            Debug.Log(result);
        }
        
    }
}
