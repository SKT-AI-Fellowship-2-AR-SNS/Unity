using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;
using DG.Tweening;

public class LoginManager : DontDestroy<LoginManager>
{
    [SerializeField]
    public string face_id;
    [SerializeField]
    public string token;
    [SerializeField]
    public string time;
    [SerializeField]
    public bool IsMyAlbum;

    [SerializeField]
    public int UID = 1;

    private IWiFiAdapter WiFiAdapter = new UniversalWiFi();
    public string[] bssid;
    public string loc="";
    public void OnKakaoLogin()
    {
        /*string url = "http://3.34.20.225:3000/users/kakao";
            "https://kauth.kakao.com/oauth/authorize?" +
            "client_id=edfe37094c314b31e73b9a24820274a7" +
            "&redirect_uri=https://www.naver.com" +
            "&response_type=code";*/
        /*Application.OpenURL(url);
        //StartCoroutine("LoadLogin");
        InvokeRepeating("Login", 2, 2);*/
        
    }
    public void OnTween()
    {
        if(!DOTween.IsTweening(EventSystem.current.currentSelectedGameObject))
            EventSystem.current.currentSelectedGameObject.transform.DOScale(1.06f, 0.1f).SetLoops(2, LoopType.Yoyo);
    }
    protected override void OnStart()
    {
        base.OnStart();
        bssid = new string[2];
        StartCoroutine("GetSirvToken");
        WiFiAdapter = new UniversalWiFi();
        wifi();
        InvokeRepeating("wifi", 10, 1000);
    }
    void wifi()
    {
        StartCoroutine("Scan");
    }
    IEnumerator Scan()
    {
#if UNITY_EDITOR
        string url = "http://54.180.5.47:3000/main/getLocation";

        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        //byte[] data = Encoding.UTF8.GetBytes("{\n   \"bssid1\" : \"00:08:9f:01:cc:9c\",\n   \"bssid2\" : \"10:e3:c7:05:a9:c7\"\n}");
        string[] report = new string[2];
        report[0] = "00:08:9f:01:cc:9c"; report[1] = "10:e3:c7:05:a9:c7";
        bssid[0] = report[0]; bssid[1] = report[1];
        byte[] data = Encoding.UTF8.GetBytes("{\n   \"bssid1\" : \"" + report[0] + "\",\n   \"bssid2\" : \"" + report[1] + "\"\n}");
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        string result = request.downloadHandler.text;
        //print(result);
        var j = JObject.Parse(result);
        //string time = j["data"].ToString();
        loc = j["data"].ToString();
        //text.text = time;
#endif
#if !UNITY_EDITOR
        if (WiFiAdapter != null)
        {
            var report = WiFiAdapter.GetNetworkReport();
            bssid[0] = report[0]; bssid[1] = report[1];
            string url = "http://54.180.5.47:3000/main/getLocation";

            List<IMultipartFormSection> form = new List<IMultipartFormSection>();
            byte[] data = Encoding.UTF8.GetBytes("{\n   \"bssid1\" : \"" + report[0] + "\",\n   \"bssid2\" : \"" + report[1] + "\"\n}");
            UnityWebRequest request = UnityWebRequest.Post(url, form);

            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            string result = request.downloadHandler.text;
            var j = JObject.Parse(result);
            //string time = j["data"].ToString();
            loc = j["data"].ToString();
            //text.text = time;
        }
#endif
    }
    IEnumerator GetSirvToken()
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
        token = tmp1.Substring(14, tmp1.Length - 15);
    }
}
